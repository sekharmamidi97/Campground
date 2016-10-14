using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;

        public CampgroundDAL(string dBconnectionString)
        {
            connectionString = dBconnectionString;
        }

        public Campground GetCampgroundById(int campgroundId)
        {
            Campground output = new Campground();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * From Campground WHERE campground_id = @campground_id", conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        output = GetCampgroundFromReader(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;
        }

 

        public List<Campground> GetCampground(int parkId)
        {
            List<Campground> output = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * From Campground WHERE park_id = @park_id", conn);
                    cmd.Parameters.AddWithValue("@park_id", parkId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground c = GetCampgroundFromReader(reader);
                        output.Add(c);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return output;

        }

        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {

            Campground c = new Campground();
            c.Id = Convert.ToInt32(reader["campground_id"]);
            c.ParkId = Convert.ToInt32(reader["park_id"]);
            c.Name = Convert.ToString(reader["name"]);
            c.DateOpen = Convert.ToInt32(reader["open_from_mm"]);
            c.DateClosed = Convert.ToInt32(reader["open_to_mm"]);
            c.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return c;
        }

        public string NumberToMonth(int i)
        {
            Dictionary<int, string> months = new Dictionary<int, string>();

            months.Add(1, "January");
            months.Add(2, "February");
            months.Add(3, "March");
            months.Add(4, "April");
            months.Add(5, "May");
            months.Add(6, "June");
            months.Add(7, "July");
            months.Add(8, "August");
            months.Add(9, "September");
            months.Add(10, "October");
            months.Add(11, "November");
            months.Add(12, "December");

            return months[i];

        }
        /*public List<Campground> AvailableCampground(int campgroundId, DateTime startDate, DateTime endDate)
        {
            List<Campground> output = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * From Campground INNER JOIN site ON campground.campground_id = site.campground_id INNER JOIN reservation ON site.site_id = reservation.site_id WHERE reservation.from_date = @from_date AND reservation.to_date = @to_date");
                    cmd.Parameters.AddWithValue("@from_date", startDate);
                    cmd.Parameters.AddWithValue("@to_date", endDate);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Site s = new Site();
                        s.Id = Convert.ToInt32 */

        //                    }
        //               }
    }

}
