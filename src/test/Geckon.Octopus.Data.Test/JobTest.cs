using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Geckon.Octopus.Data.Test
{
    [TestFixture]
    public class JobTest
    {
        #region Setup and Teardown

        private DatabaseDataContext _DB;

        [SetUp]
        public void SetUp()
        {
            _DB = new DatabaseDataContext();

            _DB.Test_CleanAndInsertDummyData( Regex.Replace(System.Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "bin\\plugins\\") );
        }

        [TearDown]
        public void TearDown()
        {
            _DB.Test_Clean();
            _DB.Dispose();
        }

        #endregion

        [Test]
        public void Should_Get_Job_All()
        {
            IEnumerable<Job> jobs = _DB.Job_GetBy( null, null, null, null, true);

            int count = 0;

            foreach (Job job in jobs)
                count++;

            Assert.AreEqual(1, count);
        }

        [Test]
        public void Should_Get_Job_By_StatusID()
        {
            IEnumerable<Job> jobs = _DB.Job_GetBy( null, 999, null, null, true);

            int count = 0;

            foreach( Job job in jobs )
                count ++;

            Assert.AreEqual( 1, count );
        }

        [Test]
        public void Should_Get_Job_By_ClientID()
        {
            IEnumerable<Job> jobs = _DB.Job_GetBy(null, null, null, null, true);

            int count = 0;

            foreach( Job job in jobs )
                count ++;

            Assert.AreEqual( 1, count );
        }

        [Test]
        public void Should_Get_Job_By_ClientID_And_Status()
        {
            IEnumerable<Job> jobs = _DB.Job_GetBy(null, 999, null, null, true);

            int count = 0;

            foreach( Job job in jobs )
                count ++;

            Assert.AreEqual( 1, count );
        }

        [Test]
        public void Should_Get_UnFinished_Jobs()
        {
            IEnumerable<Job> jobs = _DB.Job_GetUnfinishedJobs();

            int count = 0;

            foreach( Job job in jobs )
                count ++;

            Assert.AreEqual( 1, count );
        }
    }
}
