using System;
using System.Linq;
using Geckon.Events;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Core.Exceptions;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.TestPlugin;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Agent.Core.Test
{
    [TestFixture]
    public class ExecutionSlotTest
    {
        [Test]
        public void Should_Initialize_With_IAllocationDefinition()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            IExecutionSlot        slot       = new ExecutionSlot( definition );

            Assert.AreEqual( definition, slot.Definition );
        }

        [Test, ExpectedException(typeof(NullReferenceException))]
        public void Should_Throw_NullReference_With_Null_Definition()
        {
            new ExecutionSlot( null );
        }

        [Test]
        public void Should_Add_IPlugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add( DTO.NewPluginInfoTestPlugin );
            
            IExecutionSlot slot = new ExecutionSlot( definition );
            slot.Execute( new TestPlugin( 50 ) );
            Assert.AreEqual(1, slot.UsedSlots);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Should_Throw_NullReference_If_Adding_A_Null_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add( DTO.NewPluginInfoTestPlugin );
            
            IExecutionSlot slot = new ExecutionSlot( definition );
            
			slot.Execute( null );
        }

        [Test, ExpectedException(typeof(PluginNotInstalledException))]
        public void Should_Not_Add_IPlugin_If_It_Violates_Definition()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add( DTO.NewPluginInfoTestPlugin );
            
            IExecutionSlot slot = new ExecutionSlot( definition );
            slot.Execute( new TestPlugin2() );
        }

        [Test]
        public void Should_Be_Able_To_Iterate_Through_Added_Plugins()
        {
            IAllocationDefinition definition = new AllocationDefinition(6);
            definition.Add(DTO.NewPluginInfoTestPlugin);

            IExecutionSlot slot = new ExecutionSlot(definition);
            slot.Execute(new TestPlugin(600));
			slot.Execute(new TestPlugin(600));
			slot.Execute(new TestPlugin(600));
			slot.Execute(new TestPlugin(600));

            Assert.AreEqual( 4, slot.ToList().Count );
        }

        [Test,ExpectedException(typeof(NoOpenExecutionSlotsException))]
        public void Should_Throw_Exception_If_All_Slots_Are_Used()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

            IExecutionSlot slot = new ExecutionSlot(definition);
            slot.Execute(new TestPlugin(600));
            slot.Execute(new TestPlugin(600));
        }

        [Test]
        public void Should_Execute_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

            TestPlugin plugin = new TestPlugin(100);

            IExecutionSlot slot = new ExecutionSlot(definition);
            slot.Execute( plugin );

            Assert.AreEqual( 1, slot.UsedSlots );

			plugin.EndCurrentOperation();

			Timing.WaitUntil(() => plugin.Status == PluginStatus.Executed, 1000);

            Assert.AreEqual( 1.0, plugin.OperationProgress );
            Assert.AreEqual( 0, slot.UsedSlots );
            Assert.AreEqual( PluginStatus.Executed, plugin.Status );
        }

        [Test]
        public void Should_Rollback_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin2);

            TestPlugin2 plugin = new TestPlugin2(50);

            IExecutionSlot slot = new ExecutionSlot(definition);
            slot.Rollback( plugin );

			Timing.WaitUntil(() => plugin.Status == PluginStatus.Rolledback, 1000);

            Assert.AreEqual(PluginStatus.Rolledback, plugin.Status);
        }

        [Test]
        public void Should_Commit_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

            TestPlugin plugin = new TestPlugin(50);

            IExecutionSlot slot = new ExecutionSlot(definition);
            slot.Commit( plugin );

			Timing.WaitUntil(() => plugin.Status == PluginStatus.Committed, 1000);

            Assert.AreEqual( PluginStatus.Committed, plugin.Status );
        }

        [Test]
        public void Should_Fail_Commit_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

			TestPlugin plugin = new TestPlugin(50, true);
            IPlugin failedPlugin = null;
            Exception pluginException = null;

            IExecutionSlot slot = new ExecutionSlot(definition);
			slot.CommitFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                 {
                                     failedPlugin    = eventArgs.EventObject;
                                     pluginException = eventArgs.Exception;
                                 };

        	slot.Commit(plugin);

			Timing.WaitWhile(() => failedPlugin == null, 3000);

            Assert.AreEqual( plugin, failedPlugin );
            Assert.IsNotNull( pluginException );
            Assert.AreEqual( PluginStatus.CommitFailed, plugin.Status );
        }

        [Test]
        public void Should_Fail_Rollback_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

			TestPlugin plugin = new TestPlugin(50, true);
            IPlugin failedPlugin = null;
            Exception pluginException = null;

            IExecutionSlot slot = new ExecutionSlot(definition);
			slot.RollbackFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                    {
                                        failedPlugin    = eventArgs.EventObject;
                                        pluginException = eventArgs.Exception;
                                    };

			slot.Rollback(plugin);

			Timing.WaitWhile(() => failedPlugin == null, 1000);

            Assert.AreEqual(plugin, failedPlugin);
            Assert.IsTrue(pluginException != null);
            Assert.AreEqual(PluginStatus.RollbackFailed, plugin.Status);
        }

        [Test]
        public void Should_Fail_Execute_Plugin()
        {
            IAllocationDefinition definition = new AllocationDefinition(1);
            definition.Add(DTO.NewPluginInfoTestPlugin);

            TestPlugin plugin = new TestPlugin(50, true);
            IPlugin failedPlugin = null;
            Exception pluginException = null;

            IExecutionSlot slot = new ExecutionSlot(definition);
			slot.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> eventArgs)
                                    {
                                        failedPlugin    = eventArgs.EventObject;
                                        pluginException = eventArgs.Exception;
                                    };

			slot.Execute(plugin);

			Timing.WaitWhile(() => failedPlugin == null, 1000);

            Assert.AreEqual(plugin, failedPlugin);
            Assert.IsTrue(pluginException != null);
            Assert.AreEqual(PluginStatus.ExecuteFailed, plugin.Status);
        }
    }
}