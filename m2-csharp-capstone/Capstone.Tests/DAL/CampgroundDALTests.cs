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
    public class CampgroundDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=parks;User ID=te_student;Password=techelevator";
        private int campgroundId = 0;

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

                SqlCommand cmd3 = new SqlCommand("insert into Campground (park_id, name, open_from_mm, open_to_mm, daily_fee) Values(1, 'dummycampground', 1, 12, 25.00); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                campgroundId = (int)cmd3.ExecuteScalar();


            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();

        }

        [TestMethod()]
        public void GetCampgroundTest()
        {
            CampgroundDAL dal = new CampgroundDAL(connectionString);
            List<Campground> campgrounds = dal.GetCampground(1);

            Assert.AreEqual(1, campgrounds.Count);
            Assert.AreEqual("dummycampground", campgrounds[0].Name);
            
        }
    }
}