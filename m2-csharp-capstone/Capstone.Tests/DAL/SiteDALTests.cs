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
    public class SiteDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=parks;User ID=te_student;Password=techelevator";
        private int siteId = 0;
        private int reservationId = 0;
        private int parkId = 0;
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

                SqlCommand cmd3 = new SqlCommand("delete from park", conn);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand("insert into Park (name, location, establish_date, area, visitors, description) Values('dummypark', 'lalaland','2016-10-14', 1000, 500, 'the doodiest park in the world'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                parkId = (int)cmd4.ExecuteScalar();

                SqlCommand cmd5 = new SqlCommand("insert into Campground (park_id, name, open_from_mm, open_to_mm, daily_fee) Values((select park_id from park where name ='dummypark'), 'dummycampground', 1, 12, 25.00); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                campgroundId = (int)cmd5.ExecuteScalar();

                SqlCommand cmd6 = new SqlCommand("insert into Site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) Values((select campground_id from campground where name ='dummycampground'), 1, 6, 1, 500, 0); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                siteId = (int)cmd6.ExecuteScalar();

                SqlCommand cmd7 = new SqlCommand("insert into Reservation(site_id, name, from_date, to_date) Values ((select site_id from site where max_rv_length = 500), 'smith', '2016-10-12', '2016-10-15'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                reservationId = (int)cmd7.ExecuteScalar();

            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();

        }


        [TestMethod()]
        public void GetReservationTest()
        {
            SiteDAL dal = new SiteDAL(connectionString);
            List<Reservation> reservations = dal.GetReservation(campgroundId);

            Assert.AreEqual(1, reservations.Count);
            Assert.AreEqual("smith", reservations[0].Name);
        }

        [TestMethod()]
        public void GetSitesTest()
        {
            SiteDAL dal = new SiteDAL(connectionString);
            List<Site> sites = dal.GetSites(campgroundId);

            Assert.AreEqual(1, sites.Count);
            Assert.AreEqual(500, sites[0].MaxRVLength);
        }

        [TestMethod()]
        public void MakeReservationTest()
        {

            int initialCount = reservationCount();
            SiteDAL dal = new SiteDAL(connectionString);
            //Reservation reservations = new Reservation();
            //reservations.Name = "jones";
            //reservations.SiteId = siteId;
            DateTime StartDate = new DateTime(2016, 10, 12);
            DateTime EndDate = new DateTime(2016, 10, 15);
            dal.MakeReservation("shaw", siteId, StartDate, EndDate);
            Assert.AreEqual(initialCount + 1, reservationCount());

        }

        private int reservationCount()
        {
            int output = 0;
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select Count(*) from reservation", conn);
                output = (int)cmd.ExecuteScalar();
            }
            return output;
        }
    }
}