using System;
using System.Collections.Generic;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using System.Xml.Linq;
using System.Data.Linq;

namespace Geckon.Octopus.Controller.API
{
    public interface IOctopusService
    {
        IComputedJobData Job_Create(string jobXml);
        IComputedJobData Job_Create(int jobTemplateID, string xmldata);
        IList<IComputedJobData> Job_Get();
        IList<IComputedJobData> Job_Get(int? id, int? statusID, DateTime? createdDate, DateTime? lastModifiedDate);
        IComputedJobData Job_GetByID(int id);
        IComputedJobData Job_GetByStatus(int status);
        IList<IComputedJobData> Job_GetUnfinished();
        JobTemplate JobTemplate_Create(string name, Uri templateUri);
        IList<JobTemplate> JobTemplate_Get(int? id, string name);
    }
}