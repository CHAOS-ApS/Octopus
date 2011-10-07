using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.TestPlugin;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Agent.Core.Test
{
    [TestFixture]
    public class ExecutionManagerTest
    {
        [Test]
        public void Should_Initialize_ExecutionManager()
        {
            IExecutionManager manager = new ExecutionManager( new PluginManager() );

            Assert.IsNotNull( manager.PluginManager );
        }

        [Test]
        public void Should_Add_ExecutionSlot()
        {
            IExecutionManager     manager    = new ExecutionManager( new PluginManager() );
            IAllocationDefinition definition = new AllocationDefinition( 2 );

            definition.Add( DTO.NewPluginInfoTestPlugin );

            manager.PluginManager.Install( definition );
            manager.Add( new ExecutionSlot( definition ) );

            Assert.AreEqual( 1, manager.Count );
        }

        [Test]
        public void Should_Execute_IPlugin()
        {
            IExecutionManager     manager    = new ExecutionManager( new PluginManager() );
            IAllocationDefinition definition = new AllocationDefinition( 2 );
            IExecutionSlot        slot       = new ExecutionSlot(definition);

            definition.Add( DTO.NewPluginInfoTestPlugin );

            manager.PluginManager.Install( definition );
            manager.Add( slot );

            IPlugin completedPlugin = null;
            TestPlugin testPlugin = new TestPlugin(100);

            manager.ExecuteCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                            {
                                                completedPlugin = eventArgs.EventObject;
                                            };
            manager.Execute( testPlugin );
            
            Assert.AreEqual( slot, manager[ definition ] );
            Assert.AreEqual( 1, manager[ definition ].UsedSlots );

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual( testPlugin, completedPlugin );
            Assert.AreEqual( 0, manager[ definition ].UsedSlots );
            Assert.AreEqual( PluginStatus.Executed, testPlugin.Status );
        }

        [Test]
        public void Should_Fail_Execute_IPlugin()
        {
            IExecutionManager manager = new ExecutionManager(new PluginManager());
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            manager.PluginManager.Install(definition);
            manager.Add(slot);

            IPlugin completedPlugin = null;
			TestPlugin testPlugin = new TestPlugin(100, true);

			manager.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                            {
                                                completedPlugin = eventArgs.EventObject;
                                            };
            manager.Execute(testPlugin);

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual(testPlugin, completedPlugin);
            Assert.AreEqual(PluginStatus.ExecuteFailed, testPlugin.Status);
        }

        [Test]
        public void Should_Commit_IPlugin()
        {
            IExecutionManager     manager    = new ExecutionManager( new PluginManager() );
            IAllocationDefinition definition = new AllocationDefinition( 2 );
            IExecutionSlot        slot       = new ExecutionSlot(definition);

            definition.Add( DTO.NewPluginInfoTestPlugin );

            manager.PluginManager.Install( definition );
            manager.Add( slot );

            IPlugin completedPlugin = null;
            TestPlugin testPlugin = new TestPlugin(100);

			manager.CommitCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                            {
                                                completedPlugin = eventArgs.EventObject;
                                            };
            manager.Commit( testPlugin );
            
            Assert.AreEqual( slot, manager[ definition ] );
            Assert.AreEqual( 1, manager[ definition ].UsedSlots );

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual( testPlugin, completedPlugin );
            Assert.AreEqual( 0, manager[ definition ].UsedSlots );
            Assert.AreEqual( PluginStatus.Committed, testPlugin.Status );
        }

        [Test]
        public void Should_Fail_Commit_IPlugin()
        {
            IExecutionManager manager = new ExecutionManager(new PluginManager());
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            manager.PluginManager.Install(definition);
            manager.Add(slot);

            IPlugin completedPlugin = null;
			TestPlugin testPlugin = new TestPlugin(100, true);

			manager.CommitFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                        {
                                            completedPlugin = eventArgs.EventObject;
                                        };
            manager.Commit(testPlugin);

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual(testPlugin, completedPlugin);
            Assert.AreEqual(PluginStatus.CommitFailed, testPlugin.Status);
        }

        [Test]
        public void Should_Rollback_IPlugin()
        {
            IExecutionManager     manager    = new ExecutionManager( new PluginManager() );
            IAllocationDefinition definition = new AllocationDefinition( 2 );
            IExecutionSlot        slot       = new ExecutionSlot(definition);

            definition.Add( DTO.NewPluginInfoTestPlugin );

            manager.PluginManager.Install( definition );
            manager.Add( slot );

            IPlugin completedPlugin = null;
            TestPlugin testPlugin = new TestPlugin(100);

			manager.RollbackCompleted += delegate(object sender, ObjectEventArgs<IPlugin> eventArgs)
                                            {
                                                completedPlugin = eventArgs.EventObject;
                                            };
            manager.Rollback( testPlugin );
            
            Assert.AreEqual( slot, manager[ definition ] );
            Assert.AreEqual( 1, manager[ definition ].UsedSlots );

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual( testPlugin, completedPlugin );
            Assert.AreEqual( 0, manager[ definition ].UsedSlots );
            Assert.AreEqual( PluginStatus.Rolledback, testPlugin.Status );
        }

        [Test]
        public void Should_Fail_Rollback_IPlugin()
        {
            IExecutionManager manager = new ExecutionManager(new PluginManager());
            IAllocationDefinition definition = new AllocationDefinition(2);
            IExecutionSlot slot = new ExecutionSlot(definition);

            definition.Add(DTO.NewPluginInfoTestPlugin);

            manager.PluginManager.Install(definition);
            manager.Add(slot);

            IPlugin completedPlugin = null;
			TestPlugin testPlugin = new TestPlugin(100, true);

			manager.RollbackFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                        {
                                            completedPlugin = eventArgs.EventObject;
                                        };
            manager.Rollback(testPlugin);

			Timing.WaitWhile(() => completedPlugin == null, 1000);

            Assert.AreEqual(testPlugin, completedPlugin);
            Assert.AreEqual(PluginStatus.RollbackFailed, testPlugin.Status);
        }
    }
}