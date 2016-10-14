using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;
using Capstone.DAL;


namespace Capstone.CLI
{
    public class ProjectCLI
    {
        const string databaseConnection = @"Data Source=.\SQLExpress;Initial Catalog=parks;User ID=te_student;Password=techelevator";
        public void RunCLI()
        {
            int breaker = 1;
            while (breaker != 55)
            {

                GetAllParks();
                string command = Console.ReadLine().ToLower();


                if (command == "q")
                {
                    break;
                }
                else
                {
                    int breakerTwo = 1;
                    while (breakerTwo != 55)
                    {

                        int commandNumber = int.Parse(command);
                        GetParkInfo(commandNumber);
                        Console.WriteLine("Select a Command: ");
                        Console.WriteLine("1) View Campgrounds");
                        Console.WriteLine("2) Search for Reservation");
                        Console.WriteLine("3) Return to Previous Screen");

                        string subCommand = Console.ReadLine();
                        int menuChoice = int.Parse(subCommand);
                        if (menuChoice == 1)
                        {
                            GetCampgroundNow(commandNumber);
                            Console.WriteLine();
                        }
                        else if (menuChoice == 2)
                        {

                            Console.WriteLine("1) Search For Available Reservations");
                            Console.WriteLine("2) Return to previous screen");

                            string choice = Console.ReadLine();

                            while (choice != "1897")
                            {
                                if (choice == "1")
                                {

                                    GetCampgroundNow(commandNumber);
                                    Console.WriteLine("b) Back to Previous Menu");
                                    Console.WriteLine("Please input a Campground Id:");
                                    string campgroundIdString = Console.ReadLine().ToLower();
                                    if (campgroundIdString == "b")
                                    {
                                        break;
                                    }
                                    int campgroundIdInt = int.Parse(campgroundIdString);
                                    //if (commandNumber == 2)
                                    //{
                                    //    campgroundIdInt = campgroundIdInt + 3;
                                    //}
                                    //else if(commandNumber == 3)
                                    //{
                                    //    campgroundIdInt = campgroundIdInt + 6;
                                    //}
                                    Console.WriteLine();
                                    Console.WriteLine("Please input a start date for your reservation mm/dd/yyyy:");
                                    string startDateString = Console.ReadLine();
                                    DateTime startDate = DateTime.Parse(startDateString);
                                    Console.WriteLine();
                                    Console.WriteLine("Please input an end date for your reservation mm/dd/yyyy");
                                    string endDateString = Console.ReadLine();
                                    DateTime endDate = DateTime.Parse(endDateString);
                                    ReservationAvailable(campgroundIdInt, startDate, endDate);
                                    Console.WriteLine($"Which site should be reserved(enter 0 to cancel)?");
                                    string siteToBook = Console.ReadLine();
                                    int siteToBookInt = int.Parse(siteToBook);
                                    if (siteToBookInt == 0)
                                    {
                                        break;
                                    }
                                    Console.WriteLine("What name should the reservation be made under?");
                                    string reservationName = Console.ReadLine();
                                    Console.WriteLine();
                                    ReserveSite(reservationName, siteToBookInt, startDate, endDate);
                                    break;


                                }
                                else if (choice == "2")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Please Try Again");
                                }
                            }
                        }
                        else if (menuChoice == 3)
                        {
                            break;
                        }
                    }
                }
            }
            return;


        }

        private void GetAllParks()
        {
            int count = 0;
            ParksDAL dal = new ParksDAL(databaseConnection);
            List<Parks> parkList = dal.GetParks();
            parkList.ForEach(park =>
            {
                count = count + 1;
                Console.WriteLine($"{count}) {park.Name}");
            });
            Console.WriteLine("q) quit");
        }
        private void GetParkInfo(int parkID)
        {

            ParksDAL dal = new ParksDAL(databaseConnection);
            List<Parks> parkList = dal.GetParks();

            Console.WriteLine($"{parkList[parkID - 1].Name}");
            Console.WriteLine($"Location: {parkList[parkID - 1].Location}");
            Console.WriteLine($"Established: {parkList[parkID - 1].Established}");
            Console.WriteLine($"Area: {parkList[parkID - 1].Area}");
            Console.WriteLine($"Visitors: {parkList[parkID - 1].Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{parkList[parkID - 1].Description}");
            Console.WriteLine();


        }
        private void GetCampgroundNow(int parkID)
        {
            int count = 0;
            CampgroundDAL dal = new CampgroundDAL(databaseConnection);
            List<Campground> currentCampList = dal.GetCampground(parkID);
            Console.WriteLine("    Name | Open | Close | Daily Fee");
            currentCampList.ForEach(currentCamp =>
            {
                count = count + 1;
                Console.WriteLine($"{currentCamp.Id}) {currentCamp.Name} | {dal.NumberToMonth(currentCamp.DateOpen)} | {dal.NumberToMonth(currentCamp.DateClosed)} | {currentCamp.DailyFee.ToString("c")}");
            });

        }
        private void ReservationAvailable(int campground, DateTime start, DateTime end)
        {
            SiteDAL dal = new SiteDAL(databaseConnection);
            CampgroundDAL dal2 = new CampgroundDAL(databaseConnection);
            Campground campgroundObject = dal2.GetCampgroundById(campground);
            List<Reservation> reservations = dal.GetReservation(campground);
            //List<Reservation> booked = new List<Reservation>();
            List<Site> sites = dal.GetSites(campground);
            List<Reservation> available = new List<Reservation>();
            Console.WriteLine("Site Id | Max Occupancy | Accessible | Max RV Size | Utilities | Daily Fee");
            reservations.ForEach(res =>
            {
                bool overlap = (end >= res.StartDate && end <= res.EndDate) ||
                                (start >= res.StartDate && start <= res.EndDate) ||
                                (start <= res.StartDate && end >= res.EndDate);

                //bool overlap = (res.StartDate <= end && start <= res.EndDate) || 
                //               (start <= res.StartDate && res.StartDate <= end) || 
                //               (start <= res.EndDate && res.EndDate <= end) || 
                //               (res.StartDate <= start && res.EndDate >= end);
                if (overlap)
                {
                    sites.Remove(sites.Find(s => s.Id == res.SiteId));
                }
               
            });
            if (sites.Count == 0)
            {
                Console.WriteLine("I am sorry, there are no reservations available during your specified date range. Please try again.");
            }

            sites.ForEach(availableSite =>
            {
                Console.WriteLine($"{availableSite.Id} {availableSite.MaxOccupancy} {availableSite.isAccessible} {availableSite.MaxRVLength} {availableSite.hasUtilities} {campgroundObject.DailyFee}");

            });

        }
        private void ReserveSite(string name, int site, DateTime start, DateTime end)
        {
            SiteDAL dal = new SiteDAL(databaseConnection);
            //List<Reservation> reservations = 
            int makeRes = dal.MakeReservation(name, site, start, end);
            //reservations.ForEach(res =>
            //{
            Console.WriteLine($"The reservation has been made and the confirmation id is {makeRes}");
            //});
            
        }

    }
}

