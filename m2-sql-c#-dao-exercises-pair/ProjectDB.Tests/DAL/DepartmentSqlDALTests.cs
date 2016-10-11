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
    public class DepartmentSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project;User ID=te_student;Password=techelevator";

        private int departmentId = 0;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd3 = new SqlCommand("delete from project_employee", conn);
                cmd3.ExecuteNonQuery();


                SqlCommand cmd0 = new SqlCommand("delete from employee", conn);
                cmd0.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand("delete from department", conn);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("insert into Department (name) Values ('DummyDepartment'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                //cmd2.ExecuteNonQuery();
                departmentId = (int)cmd2.ExecuteScalar();

               
            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetDepartmentsTest()
        {
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);
            List<Department> departments = dal.GetDepartments();

            Assert.AreEqual(1, departments.Count);
            Assert.AreEqual("DummyDepartment", departments[0].Name);
        }

        [TestMethod()]
        public void CreateDepartmentTest()
        {
            int intialCount = GetDepartmentCount();
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);
            Department newdept = new Department();
            newdept.Name = "moreDummies";
            bool result = dal.CreateDepartment(newdept);

            //List<Department> departments = dal.CreateDepartment(newdept);

            Assert.AreEqual(intialCount + 1, GetDepartmentCount());


        }


        private int GetDepartmentCount()
        {
            int output = 0;
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from department;", conn);
                output = (int)cmd.ExecuteScalar();
            }
            return output;
        }

        [TestMethod()]
        public void UpdateDepartmentTest()
        {
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);

            Department newdept = new Department();
            newdept.Name = "moreCrashTestDummies";
            newdept.Id = departmentId;
            
            bool result = dal.UpdateDepartment(newdept);

            Assert.AreEqual(true, result);


        }
    }
}