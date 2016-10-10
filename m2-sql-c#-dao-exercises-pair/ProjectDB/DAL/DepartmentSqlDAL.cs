using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> output = new List<Department>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * from department", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Department d = new Department();
                        d.Name = Convert.ToString(reader["name"]);
                        d.Id = Convert.ToInt32(reader["department_id"]);
                        output.Add(d);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
            return output;
        }

        public bool CreateDepartment(Department newDepartment)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT department (name) Values (@name)", conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected >0);
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE department set Department.Name = @name WHERE department_id = @department_id", conn);
                    cmd.Parameters.AddWithValue("@department_id", updatedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

        }

    }
}
