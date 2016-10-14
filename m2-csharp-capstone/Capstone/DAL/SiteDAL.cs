using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteDAL
    {
        private string connectionString;

        public SiteDAL(string dBconnectionString)
        {
            connectionString = dBconnectionString;
        }
        public List<Reservation> GetReservation(int campgroundId)
        {
            List<Reservation> output = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * from reservation inner join site on site.site_id = reservation.site_id inner join campground on campground.campground_id = site.campground_id where campground.campground_id = @campground_id", conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation r = new Reservation();
                        r.Id = Convert.ToInt32(reader["reservation_id"]);
                        r.SiteId = Convert.ToInt32(reader["site_id"]);
                        r.Name = Convert.ToString(reader["name"]);
                        r.StartDate = Convert.ToDateTime(reader["from_date"]);
                        r.EndDate = Convert.ToDateTime(reader["to_date"]);
                        output.Add(r);
                    }
                  

                }
                
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }
        public List<Site> GetSites(int campgroundId)
        {
            List<Site> output = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * from site inner join campground on campground.campground_id = site.campground_id where campground.campground_id = @campground_id", conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Id = Convert.ToInt32(reader["site_id"]);
                        s.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        s.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        s.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        if (Convert.ToInt32(reader["accessible"]) == 1)
                        {
                            s.isAccessible = true;
                        }
                        else
                        {
                            s.isAccessible = false;
                        }
                        if (Convert.ToInt32(reader["utilities"]) == 1)
                        {
                            s.hasUtilities = true;
                        }
                        else
                        {
                            s.hasUtilities = false;
                        }
                        s.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        output.Add(s);
                    }


                }

            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }
        public int MakeReservation(string name, int siteId, DateTime startDate, DateTime endDate)
        {
            int reservationNumber = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Insert into reservation(name, site_id, from_date, to_date) values (@name, @site_id, @from_date, @to_date)", conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@site_id", siteId);
                    cmd.Parameters.AddWithValue("@from_date", startDate);
                    cmd.Parameters.AddWithValue("@to_date", endDate);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Select * from reservation", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation r = new Reservation();
                        r.Name = Convert.ToString(reader["name"]);
                        r.SiteId = Convert.ToInt32(reader["site_id"]);
                        r.StartDate = Convert.ToDateTime(reader["from_date"]);
                        r.EndDate = Convert.ToDateTime(reader["to_date"]);
                        r.Id = Convert.ToInt32(reader["reservation_id"]);
                        reservationNumber = r.Id;
                        
                    }
                }

            }
            catch (SqlException ex)
            {
                throw;
            }
            return reservationNumber;
        }

    }
   
    
}
