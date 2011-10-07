using System;
using System.Xml;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Data.Interface;
using System.Xml.Linq;

namespace Geckon.Octopus.TestUtilities
{
    public static class DTO
    {
		public static IPluginInfo NewPluginInfoTestPlugin 
        { 
            get
            {
                IPluginInfo pluginInfo = new PluginInfo();

                pluginInfo.Name = "Test Plugin 1";
                pluginInfo.Description = "description";
                pluginInfo.Assembly = "Geckon.Octopus.Plugins.TestPlugin";
                pluginInfo.Classname = "TestPlugin";
                pluginInfo.Filename = "Geckon.Octopus.Plugins.TestPlugin.dll";
                pluginInfo.ID = 1;
                pluginInfo.ReadURL = "Geckon.Octopus.Plugins.TestPlugin.dll";
                pluginInfo.WriteURL = "Geckon.Octopus.Plugins.TestPlugin.dll";
                pluginInfo.Version = "1.0.0.0";
                pluginInfo.CreatedDate = new DateTime(2000,01,01);

                return pluginInfo;
            } 
        }

		public static IPluginInfo NewPluginInfoTestPlugin2
		{
			get
			{
				IPluginInfo pluginInfo = new PluginInfo();

				pluginInfo.Name = "Test Plugin 2";
				pluginInfo.Description = "description";
				pluginInfo.Assembly = "Geckon.Octopus.Plugins.TestPlugin";
				pluginInfo.Classname = "TestPlugin2";
				pluginInfo.Filename = "Geckon.Octopus.Plugins.TestPlugin.dll";
				pluginInfo.ID = 2;
				pluginInfo.ReadURL = "Geckon.Octopus.Plugins.TestPlugin.dll";
				pluginInfo.WriteURL = "Geckon.Octopus.Plugins.TestPlugin.dll";
				pluginInfo.Version = "1.0.0.0";
				pluginInfo.CreatedDate = new DateTime(2000, 01, 01);

				return pluginInfo;
			}
		}

		public static IPluginInfo GetPluginInfo(string name, string classname, string assembly, int id)
		{
			IPluginInfo pluginInfo = new PluginInfo();

			pluginInfo.Name = name;
			pluginInfo.Description = "description";
			pluginInfo.Assembly = "Geckon.Octopus.Plugins." + assembly;
			pluginInfo.Classname = classname;
			pluginInfo.Filename = "Geckon.Octopus.Plugins." + assembly + ".dll";
			pluginInfo.ID = id;
			pluginInfo.ReadURL = "Geckon.Octopus.Plugins." + assembly + ".dll";
			pluginInfo.WriteURL = "Geckon.Octopus.Plugins." + assembly + ".dll";
			pluginInfo.Version = "1.0.0.0";
			pluginInfo.CreatedDate = new DateTime(2000, 01, 01);

			return pluginInfo;
		}

        public static IJobData JobData
        {
            get
            {
                IJobData jobData = new Job();

                jobData.ID = 1;
                jobData.CreatedDate = DateTime.Now;
                jobData.StatusID = 1000;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("JobXmlSample.xml");

                jobData.JobXML = XElement.Parse(xDoc.OuterXml);

                return jobData;
            }
        }

        public static IJobData ErrorJobData
        {
            get
            {
                IJobData jobData = new Job();

                jobData.ID = 1;
                jobData.CreatedDate = DateTime.Now;
                jobData.StatusID = 1000;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("ErrorJobXmlSample.xml");

                jobData.JobXML = XElement.Parse(xDoc.OuterXml);

                return jobData;
            }
        }
    }
}
