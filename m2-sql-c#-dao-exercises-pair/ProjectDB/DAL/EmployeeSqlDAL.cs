using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * from employee", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_Id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_Id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);
                        output.Add(e);

                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
            return output;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select * from employee WHERE first_name= @first_name AND last_name = @last_name", conn);
                    cmd.Parameters.AddWithValue("@first_name", firstname);
                    cmd.Parameters.AddWithValue("@last_name", lastname);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_Id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_Id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);
                        output.Add(e);

                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
            return output;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Employee Left Outer Join Project_employee ON Project_employee.employee_id = employee.employee_id WHERE project_employee.employee_id IS NULL", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_Id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_Id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);
                        output.Add(e);
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
