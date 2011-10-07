using System.Linq;
using System.Text.RegularExpressions;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.TestPlugin;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Agent.Core.Test
{
    [TestFixture]
    public class AgentEngineTest
    {
        #region Setup and Teardown

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
        }

        #endregion

        [Test]
        public void Should_Initialize_IAgent_To_Default()
        {
            IAgentEngine engine = new AgentEngine();

            Assert.IsNotNull( engine.ExecutionManager );
            Assert.IsNotNull( engine.PluginManager );
        }

        [Test]
        public void Should_Initialize_Agent_From_A_ID()
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                int          agentID = db.Agent_GetBy( null, null ).First().ID;
                IAgentEngine agent   = new AgentEngine(agentID);

                Assert.IsNotNull(agent.ExecutionManager);
                Assert.IsNotNull(agent.PluginManager);
                Assert.Greater(agent.ExecutionManager.Count, 0);
                Assert.Greater(agent.PluginManager.GetAllocationDefinitions().Count(), 0);
            }
        }

        [Test]
        public void Should_Initialize_IAgent_With_Custom_Execution_And_Plugin_Manager()
        {
            IPluginManager    pluginManager    = new PluginManager();
            IExecutionManager executionManager = new ExecutionManager( pluginManager );
            IAgentEngine      agentEngine      = new AgentEngine( executionManager );

            Assert.IsNotNull( agentEngine.ExecutionManager );
            Assert.IsNotNull( agentEngine.PluginManager );
            Assert.AreEqual( executionManager, agentEngine.ExecutionManager );
            Assert.AreEqual( pluginManager, agentEngine.PluginManager );
        }

        [Test]
        public void Should_Execute_Plugin()
        {
            IAgentEngine          engine     = new AgentEngine();
            IPlugin               plugin     = new TestPlugin();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            engine.AddDefinition(definition);

            IPlugin outPlugin = null;
            engine.ExecutionManager.ExecuteCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                                            {
                                                                outPlugin = eventArgs.EventObject;
                                                            };

        	engine.Execute(plugin);

			Timing.WaitWhile(() => outPlugin == null, 1000);

            Assert.AreEqual( plugin, outPlugin );
            Assert.AreEqual( PluginStatus.Executed, outPlugin.Status );
        }

        [Test]
        public void Should_Rollback_Plugin()
        {
            IAgentEngine engine = new AgentEngine();
            IPlugin plugin = new TestPlugin();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            engine.AddDefinition(definition);

            IPlugin outPlugin = null;
			engine.ExecutionManager.RollbackCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                                            {
                                                                outPlugin = eventArgs.EventObject;
                                                            };
            engine.Rollback(plugin);

			Timing.WaitWhile(() => outPlugin == null, 1000);

            Assert.AreEqual(plugin, outPlugin);
            Assert.AreEqual(PluginStatus.Rolledback, outPlugin.Status);
        }

        [Test]
        public void Should_Commit_Plugin()
        {
            IAgentEngine engine = new AgentEngine();
            IPlugin plugin = new TestPlugin();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            engine.AddDefinition(definition);

            IPlugin outPlugin = null;
			engine.ExecutionManager.CommitCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                                            {
                                                                outPlugin = eventArgs.EventObject;
                                                            };
            engine.Commit(plugin);

			Timing.WaitWhile(() => outPlugin == null, 1000);

            Assert.AreEqual(plugin, outPlugin);
            Assert.AreEqual(PluginStatus.Committed, outPlugin.Status);
        }

        /*[Test] There is no buffer anymore.
        public void Should_Add_Plugin_To_Buffer()
        {
            IAgentEngine          engine     = new AgentEngine();
            IPlugin               plugin     = new TestPlugin();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add( DTO.NewPluginInfoTestPlugin );

            engine.AddDefinition( definition );
            engine.AddPlugin( plugin );

            Assert.AreEqual(1,engine.CountPlugins);
            Assert.IsTrue(engine.PluginManager.IsAssemblyLoaded(definition[plugin.PluginIdentifier].AssemblyIdentifier) );
            Assert.IsTrue(engine.PluginManager.IsPluginLoaded(definition[plugin.PluginIdentifier].PluginIdentifier));
        }*/

       /* [Test,ExpectedException(typeof(PluginNotInstalledException))] There is no buffer anymore.
        public void Should_Not_Add_Plugin_With_No_Definition_To_Buffer()
        {
            IAgentEngine          engine     = new AgentEngine();
            IPlugin               plugin     = new TestPlugin2();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            engine.AddDefinition(definition);
            engine.AddPlugin(plugin);
        }*/

		/*[Test] There is no buffer anymore.
		public void Should_Add_Two_Plugins_To_Buffer()
		{
			IAgentEngine engine = new AgentEngine();
			IPlugin plugin = new TestPlugin();
			IPlugin plugin2 = new TestPlugin();
			IAllocationDefinition definition = new AllocationDefinition(2);

			definition.Add(DTO.NewPluginInfoTestPlugin);

			engine.AddDefinition(definition);
			engine.AddPlugin(plugin);
			engine.AddPlugin(plugin2);

			Assert.AreEqual(2, engine.CountPlugins);
			Assert.IsTrue(engine.PluginManager.IsAssemblyLoaded(definition[plugin.PluginIdentifier].AssemblyIdentifier));
			Assert.IsTrue(engine.PluginManager.IsPluginLoaded(definition[plugin.PluginIdentifier].PluginIdentifier));
		}*/

		/*[Test, ExpectedException(typeof(System.ArgumentException))] There is no buffer anymore.
		public void Should_Get_ArgumentException_When_Trying_To_Add_A_Plugin_With_The_Same_ID_Twice()
		{
			IAgentEngine engine = new AgentEngine();
			IPlugin plugin = new TestPlugin();
			IAllocationDefinition definition = new AllocationDefinition(2);

			definition.Add(DTO.NewPluginInfoTestPlugin);

			engine.AddDefinition(definition);
			engine.AddPlugin(plugin);
			engine.AddPlugin(plugin);
		}*/

        [Test]
        public void Should_Return_True_If_Plugin_Is_Installed()
        {
            IAgentEngine          engine     = new AgentEngine();
            IAllocationDefinition definition = new AllocationDefinition(2);

            definition.Add( DTO.NewPluginInfoTestPlugin );

            engine.AddDefinition( definition );

            Assert.IsTrue( engine.IsPluginInstalled( DTO.NewPluginInfoTestPlugin.PluginIdentifier ) );
        }

        [Test]
        public void Should_Return_False_If_Plugin_Isnt_Installed()
        {
            IAgentEngine engine = new AgentEngine();

            Assert.IsFalse( engine.IsPluginInstalled( DTO.NewPluginInfoTestPlugin.PluginIdentifier ) );
        }

		/*[Test] There is no buffer anymore.
		public void Should_Return_True_If_Plugin_Is_Added()
		{
			IAgentEngine          engine     = new AgentEngine();
			IAllocationDefinition definition = new AllocationDefinition(2);
			IPlugin plugin = new TestPlugin();

			definition.Add( DTO.NewPluginInfoTestPlugin );
			engine.AddDefinition(definition);

			engine.AddPlugin(plugin);

			Assert.IsTrue(engine.ContainsPlugin(plugin.UniqueIdentifier));
		}*/

        [Test]
        public void Should_Return_False_If_Plugin_Isnt_Added()
        {
            IAgentEngine engine = new AgentEngine();
            IPlugin plugin = new TestPlugin();

            Assert.IsFalse(engine.ContainsPlugin(plugin.UniqueIdentifier));
        }

		/*[Test] There is no buffer anymore.
		public void Should_Return_True_If_Plugin_HasAvailableSlots()
		{
			IAgentEngine engine = new AgentEngine();
			IAllocationDefinition definition = new AllocationDefinition(2);
			IPlugin plugin = new TestPlugin();

			definition.Add(DTO.NewPluginInfoTestPlugin);
			engine.AddDefinition(definition);

			engine.AddPlugin(plugin);

			Assert.IsTrue(engine.HasAvailableSlots( plugin.PluginIdentifier));
		}*/

        [Test]
        public void Should_Return_False_If_Plugin_Doesnt_Have_AvailableSlots()
        {
            IAgentEngine engine = new AgentEngine();
            IPlugin      plugin = new TestPlugin();

            Assert.IsFalse(engine.HasAvailableSlots(plugin.PluginIdentifier));
        }
    }
}