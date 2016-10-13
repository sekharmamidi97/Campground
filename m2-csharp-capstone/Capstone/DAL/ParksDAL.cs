using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class ParksDAL
    {
        private string connectionString;

        public ParksDAL(string dBconnectionString)
        {
            connectionString = dBconnectionString;
        }
        public List<Parks> GetParks()
        {
            List<Parks> output = new List<Parks>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * From Park ORDER BY name", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Parks p = new Parks();
                        p.Id = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.Established = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);
                        output.Add(p);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
            return output;
        }
    }
}
