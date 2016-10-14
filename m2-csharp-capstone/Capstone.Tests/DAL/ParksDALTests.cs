using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class ParksDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=parks;User ID=te_student;Password=techelevator";
        private int parkId = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from reservation", conn);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("delete from site", conn);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("delete from campground", conn);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("delete from park", conn);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand("insert into Park (name, location, establish_date, area, visitors, description) Values('dummypark', 'lalaland','2016-10-14', 1000, 500, 'the doodiest park in the world'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                parkId = (int)cmd4.ExecuteScalar();


            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();

        }

        [TestMethod()]
        public void GetParksTest()
        {
            ParksDAL dal = new ParksDAL(connectionString);
            List<Parks> parkss = dal.GetParks();

            Assert.AreEqual(1, parkss.Count);
            Assert.AreEqual("dummypark", parkss[0].Name);
        }
    }
}