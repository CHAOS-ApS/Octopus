using System;
using System.Data.Linq;
using System.Xml;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using System.Collections.Generic;
using System.Linq;

namespace Geckon.Octopus.Controller.Service
{
    // NOTE: If you change the class name "ControllerService" here, you must also update the reference to "ControllerService" in App.config.
    public class ControllerService : IControllerService
    {
        #region Methods

        public string GetSession( int clientID )
        {
            throw new NotImplementedException();
        }

        public IJobData AddJob(string jobXml)
        {
            // TODO: Validate jobXml

            // TODO: Add to database if valid
            XmlDocument xml = new XmlDocument();

            xml.LoadXml( jobXml );

            using( DatabaseDataContext db = new DatabaseDataContext() )
                return (IJobData)db.Job_Insert( 0, 
                                                jobXml 
                                              );
        }

        public IEnumerable<IJobData> GetJobs()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return db.Job_GetBy( null, null, null, null, true);
        }

        public IEnumerable<IJobData> GetUnfinishedJobs()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return db.Job_GetUnfinishedJobs();
        }

        public IJobData GetJobByID(int id)
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (IJobData)db.Job_GetBy( id, null, null, null, null );
        }

        public IJobData GetJobByStatus(int status)
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (IJobData)db.Job_GetBy( null, 
                                               status, 
                                               null, null, null
                                              );
        }

        #endregion
    }
}