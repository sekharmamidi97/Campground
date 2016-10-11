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
    public class ProjectSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Project;User ID=te_student;Password=techelevator";

        private int projectId = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("delete from project_employee", conn);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("delete from project", conn);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("Insert into project (name, from_date, to_date) values ('JoshesEvilProject', '2010-12-12', '2011-12-12'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                projectId = (int)cmd2.ExecuteScalar();


            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetAllProjectsTest()
        {
            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            List<Project> projects = dal.GetAllProjects();

            Assert.AreEqual(1, projects.Count);
            Assert.AreEqual("JoshesEvilProject", projects[0].Name);


        }

        [TestMethod()]
        public void AssignEmployeeToProjectTest()
        {
            int initialCount = GetProjectCount();
            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            dal.AssignEmployeeToProject(projectId, 1);

            Assert.AreEqual(initialCount + 1, GetProjectCount());

        }

        [TestMethod()]
        public void RemoveEmployeeFromProjectTest()
        {
            int initialCount = GetProjectCount();
            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            dal.AssignEmployeeToProject(projectId, 1);
            Assert.AreEqual(initialCount + 1, GetProjectCount());
            dal.RemoveEmployeeFromProject(projectId, 1);
            Assert.AreEqual(initialCount, GetProjectCount());
        }

        [TestMethod()]
        public void CreateProjectTest()
        {

            int intialCount = GetProjectCountAgain();
            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            Project newproj = new Project();
            newproj.Name = "moreJoshes";
            newproj.StartDate = new DateTime(2011, 12, 15);
            newproj.EndDate = new DateTime(2012, 6, 15);



            bool result = dal.CreateProject(newproj);

            //List<Department> departments = dal.CreateDepartment(newdept);

            Assert.AreEqual(intialCount + 1, GetProjectCountAgain());
        }


        private int GetProjectCount()
        {
            int output = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from project_employee;", conn);
                output = (int)cmd.ExecuteScalar();
            }
            return output;
        }
        private int GetProjectCountAgain()
        {
            int output = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from project;", conn);
                output = (int)cmd.ExecuteScalar();
            }
            return output;
        }
    }
}