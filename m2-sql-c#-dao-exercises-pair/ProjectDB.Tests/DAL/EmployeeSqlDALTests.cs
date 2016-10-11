using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using ProjectDB.Models;

namespace ProjectDB.DAL.Tests
{
    [TestClass()]
    public class EmployeeSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project;User ID=te_student;Password=techelevator";

        private int employeeId = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd2 = new SqlCommand("delete from project_employee", conn);
                cmd2.ExecuteNonQuery();


                SqlCommand cmd3 = new SqlCommand("delete from project", conn);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand("delete from employee", conn);
                cmd.ExecuteNonQuery();

                //SqlCommand cmd1 = new SqlCommand("delete from department", conn);
                //cmd1.ExecuteNonQuery();                                          
                

                SqlCommand cmd4 = new SqlCommand("Insert into employee (first_name, last_name, job_title, department_id, birth_date, gender, hire_date) values ('Lebron', 'James', 'MVP', 1, '1984-11-25', 'm', '2010-11-17'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                employeeId = (int)cmd4.ExecuteScalar();


            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetAllEmployeesTest()
        {
            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dal.GetAllEmployees();

            Assert.AreEqual(1, employees.Count);
            Assert.AreEqual("Lebron", employees[0].FirstName);
            Assert.AreEqual("James", employees[0].LastName);
        }

        [TestMethod()]
        public void SearchTest()
        {
            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dal.Search("Lebron", "James");

            Assert.AreEqual(1, employees.Count);
            Assert.AreEqual(employeeId, employees[0].EmployeeId);
        }

        [TestMethod()]
        public void GetEmployeesWithoutProjectsTest()
        {
            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dal.GetEmployeesWithoutProjects();

            Assert.AreEqual(1, employees.Count);
            Assert.AreEqual("Lebron", employees[0].FirstName);
            Assert.AreEqual("James", employees[0].LastName);
        }
    }
}