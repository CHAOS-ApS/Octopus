using System;
using System.Linq;
using System.Text.RegularExpressions;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Controller.Service.Test;
using Geckon.Octopus.Data;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Controller.Service.Test
{
    [TestFixture]
    public class ControllerServiceTest : ControllerService
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
                db.Test_CleanAndInsertDummyData(Regex.Replace(System.Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "lib\\"));
        }

        [TearDown]
        public void TearDown()
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
                db.Test_Clean();
        }

        #endregion
        #region Test methods

        [Test]
        public void Should_Add_Job_To_Database()
        {
            IJobData job =  AddJob(DTO.JobData.JobXML.ToString());

            using( DatabaseDataContext db = new DatabaseDataContext() )
                Assert.AreEqual(
                    db.Job_GetBy(job.ID, null, null, null, true).ToList().Count, 1
                    );
        }

        [Test]
        public void Should_Get_All_Jobs()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                Assert.Greater(GetJobs().Count, 0);
        }

        #endregion
        #region Properties


        #endregion
    }
}
