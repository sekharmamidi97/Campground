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
                    int breakerTwo =  1;
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
            Console.WriteLine("    Name | Open | Close |Cost");
            currentCampList.ForEach(currentCamp =>
            {
                count = count + 1;
                Console.WriteLine($"{count}) {currentCamp.Name} | {dal.NumberToMonth(currentCamp.DateOpen)} | {dal.NumberToMonth(currentCamp.DateClosed)} | {currentCamp.DailyFee.ToString("c")}");
            });
            
    


        }
    }

}

