using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Geckon.Octopus.Controller.Interface;

namespace Geckon.Octopus.Data
{
    partial class DatabaseDataContext
    {
        public DatabaseDataContext() : base( ConfigurationManager.ConnectionStrings["Octopus"].ConnectionString )
        {
            
        }

        [Function(Name = "dbo.PluginInfo_GetBy")]
        public ISingleResult<PluginInfo> PluginInfo_GetByClassname([Parameter(Name = "Classname", DbType = "VarChar(512)")] string classname)
        {
            return PluginInfo_GetBy(null, null, null, null, null, null, classname, null, null, null);
        }

        [Function(Name = "dbo.PluginInfo_GetBy")]
        public ISingleResult<PluginInfo> PluginInfo_GetByAssemblyname([Parameter(Name = "Assemblyname", DbType = "VarChar(512)")] string assemblyname)
        {
            return PluginInfo_GetBy(null, null, null, null, null, assemblyname, null, null, null, null);
        }

        public IEnumerable<PluginInfo> PluginInfo_GetAll()
        {
            return PluginInfo_GetBy(null, null, null, null, null, null, null, null, null, null);
        }

        public ISingleResult<Job> Job_Update(IJobData jobData)
        {
            return Job_Update(jobData.ID,  jobData.StatusID, jobData.JobXML, jobData.CreatedDate);
        }
    }
}
