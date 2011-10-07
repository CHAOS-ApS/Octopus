using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Controller.Core.Test
{
    [TestFixture]
    public class JobManagerTest
    {
        #region SetUp/TearDown

        private DatabaseDataContext _DB;

        [SetUp]
        public void SetUp()
        {
            _DB = new DatabaseDataContext();

			_DB.Test_CleanAndInsertDummyData(Regex.Replace(System.Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "bin\\plugins\\"));
        }

        [TearDown]
        public void TearDown()
        {
            _DB.Test_Clean();
            _DB.Dispose();
            PluginLoader.Clear();
        }

        #endregion

        [Test]
        public void Should_Sync_With_DB()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
            {
                // TODO: Load all assemblies
                foreach (AssemblyInfo info in db.AssemblyInfo_Get(null, null, null, null, null, null, null, true))
                {
                    PluginLoader.Add(info.Version + ", " + info.Name, info.ReadURL); // Add AssemblyIdentifier !!!!!!!!!!!!!!
                }
            }

        	using (IJobManager mgr = new JobManager())
        	{
                bool isSynced = false;
                mgr.SynchronizeOnce();
                mgr.SyncCompleted += (sender, eventArgs) => isSynced = true;

                Timing.WaitUntil(() => isSynced, 10000, "Wait til JobManager is synced");

        		foreach (Data.Job job in _DB.Job_GetUnfinishedJobs())
        		{
        			if (!mgr.ContainsKey(job.ID))
        				Assert.Fail();
        		}
        	}
        }

        [Test]
        public void Should_Sync_DB_With_InMemory_Collection()
        {
            using (DatabaseDataContext db = new DatabaseDataContext())
            {
                // Load all assemblies
                foreach (AssemblyInfo info in db.AssemblyInfo_Get(null, null, null, null, null, null, null, true))
                {
                    PluginLoader.Add(info.Version + ", " + info.Name, info.ReadURL); // Add AssemblyIdentifier !!!!!!!!!!!!!!
                }
            }

        	using( IJobManager mgr = new JobManager() )
        	{
                bool isSynced = false;
                mgr.SynchronizeOnce();
                mgr.SyncCompleted += (sender, eventArgs) => isSynced = true;

                Timing.WaitUntil(() => isSynced, 10000, "Wait til JobManager is synced");

        		// Change the Jobs
        		foreach (IJob engineJob in mgr)
        		{
        			engineJob.StatusID = 1000;
        		}

                isSynced = false;
                mgr.SynchronizeOnce();  // Resync, to make sure changes in the InMemory collection is syncronised as well.
                mgr.SyncCompleted += (sender, eventArgs) => isSynced = true;

                Timing.WaitUntil(() => isSynced, 10000, "Wait til JobManager is synced");

        		_DB.Dispose();
        		_DB = new DatabaseDataContext();

        		int count = _DB.Job_GetUnfinishedJobs().ToList().Count;

        		Assert.AreEqual(1, mgr.ToList().Count);

        		// Check if all the in memory objects are in the DB, and has the correct values
        		foreach (IJob engineJob in mgr)
        		{
        			int currentCount = 0;

        			foreach (Data.Job job in _DB.Job_GetUnfinishedJobs().ToList())
        			{
        				currentCount++;

        				if( Compare( engineJob, job ) )
        					break;

        				if( currentCount == count )
        					Assert.Fail(job.ID + " didn't compare correctly");
        			}
        		}
        	}
        }

        private static bool Compare( IJob job1, Data.Job job2 )
        {
            return  job1.ID          == job2.ID        && 
                    job1.StatusID    == job2.StatusID  &&
                    job1.JobXML      == job2.JobXML    && 
                    job1.CreatedDate == job2.CreatedDate;
        }

        [Test]
        public void Should_Get_Plugins_While_Obeying_The_Step_Flow_Rules()
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                // Load all assemblies
                foreach (AssemblyInfo info in db.AssemblyInfo_Get(null, null, null, null, null, null, null, true))
                {
                    PluginLoader.Add(info.Version + ", " + info.Name, info.ReadURL); // Add AssemblyIdentifier !!!!!!!!!!!!!!
                }
            }

        	using (IJobManager mgr = new JobManager())
        	{
                bool isSynced = false;
                mgr.SynchronizeOnce();
                mgr.SyncCompleted += (sender, eventArgs) => isSynced = true;

                Timing.WaitUntil(() => isSynced, 10000, "Wait til JobManager is synced");

        		IList<IPlugin> firstRunnablePlugins = new List<IPlugin>();

        		foreach (IJob job in mgr)
        		{
        			foreach (IPlugin plugin in job.GetRunablePlugins())
        			{
        				firstRunnablePlugins.Add( plugin );
                        Assert.IsTrue(plugin.Status == PluginStatus.Initialized, "Not all firstRunnablePlugins are Queued, Status is: " + plugin.Status);    
        			}
        		}

        		foreach( IPlugin plugin in firstRunnablePlugins )
        		{
        			plugin.BeginExecute();
        		}

        		Assert.AreEqual(6, firstRunnablePlugins.Count, "first count");

        		IList<IPlugin> secondRunnablePlugins = new List<IPlugin>();

        		foreach (IJob job in mgr)
        		{
        			foreach (IPlugin plugin in job.GetRunablePlugins())
        			{
        				secondRunnablePlugins.Add(plugin);
                        Assert.IsTrue(plugin.Status == PluginStatus.Initialized, "Not all secondRunnablePlugins are Queued, Status is: " + plugin.Status);
        			}
        		}

        		foreach( IPlugin plugin in secondRunnablePlugins )
        		{
        			plugin.BeginExecute();
        		}

        		Assert.AreEqual(1, secondRunnablePlugins.Count, "second count");

        		IList<IPlugin> thirdRunnablePlugins = new List<IPlugin>();

        		foreach (IJob job in mgr)
        		{
        			foreach (IPlugin plugin in job.GetRunablePlugins())
        			{
        				thirdRunnablePlugins.Add(plugin);
                        Assert.IsTrue(plugin.Status == PluginStatus.Initialized, "Not all thirdRunnablePlugins are Queued, Status is: " + plugin.Status);
        			}
        		}

        		foreach( IPlugin plugin in thirdRunnablePlugins )
        		{
        			plugin.BeginExecute();
        		}

        		Assert.AreEqual(3, thirdRunnablePlugins.Count, "third count");

        		IList<IPlugin> fourthRunnablePlugins = new List<IPlugin>();

        		foreach (IJob job in mgr)
        		{
        			foreach (IPlugin plugin in job.GetRunablePlugins())
        			{
        				fourthRunnablePlugins.Add(plugin);
                        Assert.IsTrue(plugin.Status == PluginStatus.Initialized, "Not all fourthRunnablePlugins are Queued, Status is: " + plugin.Status);
        			}
        		}

        		foreach( IPlugin plugin in fourthRunnablePlugins )
        		{
        			plugin.BeginExecute();
        		}

        		Assert.AreEqual(1, fourthRunnablePlugins.Count, "fourth count");

        		foreach (IJob job in mgr)
        		{
        			foreach (IPlugin plugin in job.GetAllPlugins())
        			{
						if (plugin.Status == PluginStatus.Initialized)
							Assert.Fail("There should be no more runnable plugins");
        			}
        		}
        	}
        }
    }
}