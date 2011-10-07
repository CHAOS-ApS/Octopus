using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Xsl;
using System.Data.Linq;
using System.Net;
using System.Linq;

namespace Geckon.Octopus.Controller.API
{
    public class OctopusService : IOctopusService
    {
        #region Methods

        public IComputedJobData Job_Create(string jobXml)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
                return (IComputedJobData)db.Job_Insert(null, jobXml).Single();
        }

        public IComputedJobData Job_Create(int jobTemplateID, string xmldata)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
            {
                JobTemplate jobTemplate = db.JobTemplate_Get(jobTemplateID, null).Single();

                string templateUri = jobTemplate.JobTemplateUri;

                string templateXml = GetTemplateXml(templateUri);

                string jobXml = TransformXslt(
                    templateXml,
                    xmldata
                    );

                return (IComputedJobData)db.Job_Insert(null, jobXml).Single();
            }
        }

        private string GetTemplateXml(string templateUri)
        {
            if (Uri.IsWellFormedUriString(templateUri, UriKind.Absolute))
                return TryDownloadStyleSheet(templateUri);

            if (IsXml(templateUri))
                return templateUri;

            throw new Exception("The template URI \"" + templateUri + "\" is neither an absolut URI nor valid XML.");
        }

        private static bool IsXml(string xml)
        {
            try
            { XDocument.Parse(xml); }
            catch (XmlException)
            { return false; }

            return true;
        }

        protected string TryDownloadStyleSheet(string templateUri)
        {
            try
            { return new WebClient().DownloadString(templateUri); }
            catch (Exception exception)
            { throw new Exception("Failed to download xslt style sheet from " + templateUri, exception); }
        }

        public IList<IComputedJobData> Job_Get(int? id, int? statusID, DateTime? createdDate, DateTime? lastModifiedDate)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
                return (
                    from job in db.Job_GetBy(id, statusID, null, createdDate, true).ToList()
                    select (IComputedJobData)job
                    ).ToList();
        }

        public JobTemplate JobTemplate_Create(string name, Uri templateUri)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
                return db.JobTemplate_Create(name, templateUri.ToString()).Single();
        }

        public JobTemplate JobTemplate_Create(string name, string templateXml)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
                return db.JobTemplate_Create(name, templateXml).Single();
        }

        public IList<JobTemplate> JobTemplate_Get(int? id, string name)
        {
            using (DatabaseDataContext db = new DatabaseDataContext(OctopusConnectionString))
                return db.JobTemplate_Get(id, name).ToList();
        }


        public IList<IComputedJobData> Job_Get()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (
                    from job in db.Job_GetBy(null, null, null, null, null)
                    select (IComputedJobData)job
                    ).ToList();
        }

        public IList<IComputedJobData> Job_GetUnfinished()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (
                    from job in db.Job_GetUnfinishedJobs()
                    select (IComputedJobData)job
                    ).ToList();
        }

        public IComputedJobData Job_GetByID(int id)
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (IComputedJobData)db.Job_GetBy(id, null, null, null, null);
        }

        public IComputedJobData Job_GetByStatus(int status)
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                return (IComputedJobData)db.Job_GetBy(null,
                                               status,
                                               null, null, null
                                              );
        }

        public void Test_CleanAndInsertTestData(string appPath)
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
                db.Test_CleanAndInsertDummyData(appPath);
        }

        #endregion
        #region Helper methods

        private static string TransformXslt(string xslt, string xml)
        {
            var xr = XmlReader.Create(
                new StringReader(xml)
                );
            
            var transform = new XslCompiledTransform();

            transform.Load(
                CreateXmlReader(xslt)
                );

            var sb = new StringBuilder();

            transform.Transform(
                CreateXmlReader(xml),
                XmlWriter.Create(sb)
                );

            //Remove UTF16 declaration created by xslt tranform, and rejected by sql server.
            return XDocument.Parse(sb.ToString()).ToString();
        }

        private static XmlReader CreateXmlReader(string xml)
        {
            return XmlReader.Create(
                new StringReader(xml)
                );
        }

        #endregion
        #region Properties

        private static string OctopusConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Octopus"].ConnectionString; }
        }

        #endregion
    }
}
