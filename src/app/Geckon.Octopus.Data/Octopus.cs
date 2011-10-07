using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Geckon.Octopus.Controller.Interface;

namespace Geckon.Octopus.Data
{
    public partial class OctopusDataContext
    {
        [Function(Name = "dbo.PluginInfo_GetBy")]
        public ISingleResult<PluginInfo> PluginInfo_GetByClassname([Parameter(Name = "Classname", DbType = "VarChar(512)")] string classname)
        {
            return PluginInfo_GetBy(null, null, null, null, null, classname, null, null, null);
        }

        [Function(Name = "dbo.PluginInfo_GetBy")]
        public ISingleResult<PluginInfo> PluginInfo_GetByAssemblyname([Parameter(Name = "Assemblyname", DbType = "VarChar(512)")] string assemblyname)
        {
            return PluginInfo_GetBy(null, null, null, null, assemblyname, null, null, null, null);
        }

        public IEnumerable<PluginInfo> PluginInfo_GetAll()
        {
            return PluginInfo_GetBy(null, null, null, null, null, null, null, null, null);
        }

        public int Job_Update(IJobData jobData)
        {
            return Job_Update(jobData.ID, jobData.ClientID, jobData.StatusID, jobData.JobXML, jobData.CreatedDate);
        }
    }
}