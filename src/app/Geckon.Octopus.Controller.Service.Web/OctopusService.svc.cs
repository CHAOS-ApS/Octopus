using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Serialization;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Controller.Service.Web
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OctopusService : Geckon.Web.WCF.WCFBase, IOctopusService
    {
        [Description("http://wiki.geckon.com/Octopus.Job_Get.ashx")]
        [WebGet(UriTemplate = "Job_Get?id={id}")]
        [OperationBehavior(AutoDisposeParameters = true)]
        public Stream Job_Get(int? id)
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                return ToStream( XmlSerialize.ToXML( db.Job_GetBy( id, null, null, null, null, true ).Select( job => new JobDTO( job ) ).ToList(), ReturnType.Default, true ) );
            }
        }

        [Description("http://wiki.geckon.com/Octopus.Job_GetUnfinished.ashx")]
        [WebGet(UriTemplate = "Job_GetUnfinished")]
        [OperationBehavior(AutoDisposeParameters = true)]
        public Stream Job_GetUnfinished()
        {
            throw new NotImplementedException();
        }

        [Description("http://wiki.geckon.com/Octopus.Job_Insert.ashx")]
        [WebGet(UriTemplate = "Job_Insert")]
        [OperationBehavior(AutoDisposeParameters = true)]
        public Stream Job_Insert()
        {
            throw new NotImplementedException();
        }
    }

    public class JobDTO
    {
        [AttributeAttribute("ID")]
        int ID { get; set; }

        [AttributeAttribute("ClientID")]
        int ClientID { get; set; }

        [AttributeAttribute("StatusID")]
        int StatusID { get; set; }

        [Element(true)]
        string JobXML { get; set; }

        [Element]
        System.DateTime CreatedDate { get; set; }

        [Element]
        System.DateTime LastUpdated { get; set; }

        public JobDTO(Job job)
        {
            ID = job.ID;
            ClientID = job.ClientID;
            StatusID = job.StatusID;
            JobXML = job.JobXML;
            CreatedDate = job.CreatedDate;
            LastUpdated = job.LastUpdated;
        }
    }
}
