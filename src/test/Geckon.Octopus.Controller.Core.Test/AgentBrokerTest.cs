using Geckon.Events;
using Geckon.Octopus.Agent.Core;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.TestPlugin;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Controller.Core.Test
{
   
    [TestFixture]
    public class AgentBrokerTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public void Should_Initialize_Broker()
        {
            IBroker broker = new Broker();

            Assert.IsNotNull( broker.Agents, "Agents" );
            Assert.IsNotNull( broker.GetPluginsOnAgents(), "Plugins" );
        }

        [Test]
        public void Should_Add_LocalAgent()
        {
            IBroker broker = new Broker();
            IAgent  agent  = new AgentEngine( );
            int     count  = 0;

            broker.Add( agent );

            foreach( IAgent enumerable in broker.Agents )
                count++;

            Assert.AreEqual(1, count);
        }

        [Test]
        public void Should_Execute_Plugin()
        {
            IBroker      broker = new Broker();
            IAgentEngine agent  = new AgentEngine( );
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add( slot );

            definition.Add(DTO.NewPluginInfoTestPlugin);

            IPlugin completedPlugin = null;
            TestPlugin testPlugin = new TestPlugin(100);

            broker.ExecuteCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                        {
                                            completedPlugin = eventArgs.EventObject;
                                        };

            broker.Execute(testPlugin);

            Assert.AreEqual(slot, agent.ExecutionManager[definition]);
            Assert.AreEqual(1, agent.ExecutionManager[definition].UsedSlots, "Slots Used");

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual(testPlugin, completedPlugin);
            Assert.AreEqual(0, agent.ExecutionManager[definition].UsedSlots);
            Assert.AreEqual(PluginStatus.Executed, testPlugin.Status);


        }

        [Test, ExpectedException(typeof(NoOpenExecutionSlotsException))]
        public void Should_Throw_Exception_When_No_Slots_Are_Available()
        {
            IBroker broker = new Broker();
            IAgentEngine agent = new AgentEngine();
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            definition.Add( DTO.NewPluginInfoTestPlugin );
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add(slot);

            broker.Execute(new TestPlugin(600));
			broker.Execute(new TestPlugin(600));
			broker.Execute(new TestPlugin(600));
        }

        [Test, ExpectedException(typeof(PluginNotInstalledException))]
        public void Should_Throw_Exception_When_Plugin_Isnt_Installed_When_Execute()
        {
            IBroker broker = new Broker();
            IAgentEngine agent = new AgentEngine();
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add(slot);

            TestPlugin testPlugin = new TestPlugin();

            broker.Execute(testPlugin);
        }

        [Test, ExpectedException(typeof(PluginNotInstalledException))]
        public void Should_Throw_Exception_When_Plugin_Isnt_Installed_When_Rollback()
        {
            IBroker broker = new Broker();
            IAgentEngine agent = new AgentEngine();
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add(slot);

            TestPlugin testPlugin = new TestPlugin();

            broker.Rollback(testPlugin);
        }

        [Test]
        public void Should_Rollback_Plugin()
        {
            IBroker      broker = new Broker();
            IAgentEngine agent  = new AgentEngine( );
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            definition.Add( DTO.NewPluginInfoTestPlugin );
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add( slot );

            definition.Add(DTO.NewPluginInfoTestPlugin);

            IPlugin completedPlugin = null;
            TestPlugin testPlugin = new TestPlugin(100);

			broker.RollbackCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                        {
                                            completedPlugin = eventArgs.EventObject;
                                        };

            broker.Rollback(testPlugin);

            Assert.AreEqual(slot, agent.ExecutionManager[definition]);
            Assert.AreEqual(1, agent.ExecutionManager[definition].UsedSlots, "Slots Used");

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual(testPlugin, completedPlugin);
            Assert.AreEqual(0, agent.ExecutionManager[definition].UsedSlots);
            Assert.AreEqual(PluginStatus.Rolledback, testPlugin.Status);


        }

        [Test, ExpectedException(typeof(NoOpenExecutionSlotsException))]
        public void Should_Throw_Exception_When_No_Slots_Are_Available_For_Rollback()
        {
            IBroker broker = new Broker();
            IAgentEngine agent = new AgentEngine();
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            broker.Add(agent);
            definition.Add(DTO.NewPluginInfoTestPlugin);
            agent.PluginManager.Install(definition);
            agent.ExecutionManager.Add(slot);

            broker.Rollback(new TestPlugin(600));
			broker.Rollback(new TestPlugin(600));
			broker.Rollback(new TestPlugin(600));
        }
    }
}