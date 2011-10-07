using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Geckon.Events;
using Geckon.Octopus.Agent.Core;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Controller.Core.Test
{
    [TestFixture]
    public class ControllerEngineTest
    {
        #region SetUp/TearDown

        private DatabaseDataContext _DB;

        [SetUp]
        public void SetUp()
        {
            _DB = new DatabaseDataContext();

            _DB.Test_CleanAndInsertDummyData(Regex.Replace(System.Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "bin\\plugins\\"));

            PluginLoader.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            _DB.Dispose();
            PluginLoader.Clear();
        }

        #endregion

        [Test]
        public void Should_Start_Workflow_When_File_Is_Dropped_In_Watched_Folder()
        {
            string file = _DB.Destinations.First().WriteURL + "\\File.tmp";

            if( System.IO.File.Exists( file ) )
                System.IO.File.Delete( file );

            using( IControllerEngine engine = new ControllerEngine(true))
            {
                using( System.IO.File.CreateText(file) )
                {
                    
                }

                bool isAdded = false;
                engine.JobManager.JobAddedToQueue += (obj, args) => isAdded = true;

                Timing.WaitWhile(() => !isAdded, 20000);
                Timing.WaitUntil(() => engine.JobManager.Count == 0, 20000);

                Assert.Greater(_DB.Jobs.Count(), 1);
            }

            if( System.IO.File.Exists( file ) )
                System.IO.File.Delete( file );
        }

        [Test]
        public void Should_Initialize_ControllerEngine()
        {
			using (IControllerEngine engine = new ControllerEngine(false))
			{
				Assert.IsNotNull( engine.JobManager );
			}
        }

        [Test]
        public void Should_Load_Assemblies_On_Initialize()
        {
			using( IControllerEngine engine = new ControllerEngine(false) )
			{
				Assert.Greater(PluginLoader.Count, 0);
			}
        }

        [Test]
        public void Should_Initialize_Agent_And_ExecutionSlots()
        {
            using (IControllerEngine engine = new ControllerEngine(false))
            {
                Assert.AreEqual( 1, engine.Broker.Agents.Count() );
                Assert.AreEqual( 1, ((AgentEngine) engine.Broker.Agents.ToList()[0]).ExecutionManager.Count );
                Assert.AreEqual( 4, ((AgentEngine) engine.Broker.Agents.ToList()[0]).ExecutionManager.ToList()[0].Definition.MaxSlots );
                Assert.AreEqual( 7, ((AgentEngine) engine.Broker.Agents.ToList()[0]).ExecutionManager.ToList()[0].Definition.Count() );
            }
        }

        [Test]
        public void Should_AutoSync_JobQueue()
        {
			using (IControllerEngine engine = new ControllerEngine(true))
			{
				bool isSynced = false;
				
				engine.JobManager.SyncCompleted += (sender, eventArgs) => isSynced = true;

				Timing.WaitUntil(() => isSynced, 10000, "Wait til JobManager is synced");

				Assert.Greater(engine.JobManager.Count, 0);
			}
        }

        [Test]
        public void Should_Not_AutoSync_JobQueue()
        {
           using(IControllerEngine engine = new ControllerEngine(false))
           {
			   engine.JobManager.SynchronizeOnce();

               bool isSynced = false;

               engine.JobManager.SyncCompleted += (sender, eventArgs) => isSynced = true;

               Timing.WaitUntil(() => isSynced, 20000, "Waiting for the sync failed");

			   Assert.Greater(engine.JobManager.Count, 0);
           }
        }

        [Test]
        public void Should_Send_Queued_Plugins_To_AgentBroker()
        {
			using( IControllerEngine controllerEngine = new ControllerEngine( true ) )
			{
				IAgent                agent      = new AgentEngine();
				IAllocationDefinition definition = new AllocationDefinition(3);

				definition.Add( DTO.NewPluginInfoTestPlugin );
				agent.AddDefinition( definition );

				controllerEngine.Broker.Add( agent );
				controllerEngine.JobManager.SynchronizeOnce();

				Timing.WaitUntil(() => controllerEngine.Broker.GetPluginsOnAgents().ToList().Count > 0, 10000, "Broker has plugins");

				Assert.Greater(controllerEngine.Broker.GetPluginsOnAgents().ToList().Count, 0);
			}
        }

        [Test]
        public void Should_Send_A_Job_To_Commit_When_All_Plugins_Are_Executed()
        {
			using (IControllerEngine controllerEngine = new ControllerEngine(true))
			{
				IAgent agent = new AgentEngine();
				IAllocationDefinition definition = new AllocationDefinition(3);

				definition.Add(DTO.NewPluginInfoTestPlugin);
				agent.AddDefinition(definition);
				controllerEngine.Broker.Add(agent);
				controllerEngine.JobManager.SynchronizeOnce();

				Timing.WaitUntil(delegate
				                 	{
                                        if( controllerEngine.JobManager.Count == 0 )
                                            return false;

				                 		foreach (IJob job in controllerEngine.JobManager)
				                 		{
				                 			foreach (IPlugin plugin in job.GetAllPlugins())
				                 			{
				                 				if (plugin.Status != PluginStatus.Committed)
				                 					return false;
				                 			}
				                 		}

				                 		return true;
				                 	}, 20000, "Testing if all plugins are committed");

				IList<IPlugin> plugins = new List<IPlugin>();
				IList<IPlugin> executedPlugins = new List<IPlugin>();

				foreach (IJob job in controllerEngine.JobManager)
				{
					foreach (IPlugin plugin in job.GetAllPlugins())
					{
						if (plugin.Status == PluginStatus.Committed)
							executedPlugins.Add(plugin);

						plugins.Add(plugin);
					}
				}

				Assert.AreEqual(11, plugins.Count);
				Assert.AreEqual(plugins.Count, executedPlugins.Count, "Total should be equal comitted count");
			}
        }

        [Test]
        public void Should_Rollback_Plugins_If_Error_Occurs()
        {
			using (IControllerEngine controllerEngine = new ControllerEngine(true))
			{
				IAgent                agent      = new AgentEngine();
				IAllocationDefinition definition = new AllocationDefinition(3);
				IJob                  job        = new Job(DTO.ErrorJobData);

				definition.Add( DTO.NewPluginInfoTestPlugin  );
				definition.Add( DTO.NewPluginInfoTestPlugin2 );

				agent.AddDefinition( definition );
				controllerEngine.Broker.Add( agent );

				_DB.Job_Insert( job.StatusID, job.JobXML.ToString() );
				controllerEngine.JobManager.SynchronizeOnce();

				bool isCompleted = false;
				bool isFailed    = false;
                bool isSynced    = false;

                controllerEngine.JobManager.SyncCompleted += (sender, eventArgs) => isSynced    = true;

                Timing.WaitUntil(() => isSynced, 20000, "Waiting for the sync failed");

				controllerEngine.JobManager.Last().JobRolledback   += (sender, eventArgs) => isCompleted = true;
                controllerEngine.JobManager.Last().JobCommitFailed += (sender, eventArgs) => isFailed    = true;

				Timing.WaitUntil( () => isCompleted, 20000, "Wait til last job rolledback");

				controllerEngine.JobManager.SynchronizeOnce();

				Assert.IsTrue(isCompleted);
				Assert.IsFalse(isFailed, "Rollback failed");
			}
        }

        [Test]
        public void Should_Clean_Up_Finished_Jobs()
        {
        	using (IControllerEngine controllerEngine = new ControllerEngine(true))
        	{
//				IAgent agent = new AgentEngine();
//				IAllocationDefinition definition = new AllocationDefinition(3);
//
//
//				definition.Add(DTO.NewPluginInfoTestPlugin);
//				definition.Add(DTO.NewPluginInfoTestPlugin2);
//				agent.AddDefinition(definition);
//				controllerEngine.Broker.Add(agent);

				Timing.WaitUntil(() => controllerEngine.JobManager.ToList().Count != 0, 20000, "Wait til Jobmanager has jobs");
				Timing.WaitUntil(() => controllerEngine.JobManager.ToList().Count == 0, 10*60*1000, 1000, "Wait til Jobmanager no longer has jobs");

				bool isSynced = false;

				controllerEngine.JobManager.SyncCompleted += (sender, eventArgs) => isSynced = true;

				Timing.WaitUntil(() => isSynced, 5000, "Wait Jobmanager has synced with DB once");

				int count = _DB.Job_GetBy(null, 4000, null, null, true).ToList().Count;

				Assert.AreEqual(1, count, "Job status wasn't updated in the database");
        	}
        }
    }
}