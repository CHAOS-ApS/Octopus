using System.Collections.Generic;
using System.Reflection;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data.Interface;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;
using System;

namespace Geckon.Octopus.Agent.Core.Test
{
    [TestFixture]
    public class AllocationDefinitionTest
    {
        
        [Test]
        public void Should_Create_Slot()
        {
            IDictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
            IAllocationDefinition definition = new AllocationDefinition( 2 );

            Assert.AreEqual( 2, definition.MaxSlots );
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Should_Not_Be_Able_To_Create_With_Zero_MaxSlots()
        {
            AllocationDefinition definition = new AllocationDefinition( 0 );
        }

        [Test]
        public void Should_Add_IPluginInfo_To_Definition()
        {
            IAllocationDefinition definition = new AllocationDefinition( 2 );
            IPluginInfo           pluginInfo = DTO.NewPluginInfoTestPlugin;

            definition.Add( pluginInfo );

            Assert.AreEqual( pluginInfo.Assembly, definition[ pluginInfo.PluginIdentifier ].Assembly );
            Assert.AreEqual( pluginInfo.AssemblyIdentifier, definition[ pluginInfo.PluginIdentifier ].AssemblyIdentifier );
            Assert.AreEqual( pluginInfo.Classname, definition[ pluginInfo.PluginIdentifier ].Classname );
            Assert.AreEqual( pluginInfo.CreatedDate.ToShortDateString(), definition[ pluginInfo.PluginIdentifier ].CreatedDate.ToShortDateString() );
            Assert.AreEqual( pluginInfo.Description, definition[ pluginInfo.PluginIdentifier ].Description );
            Assert.AreEqual( pluginInfo.Filename, definition[ pluginInfo.PluginIdentifier ].Filename );
            Assert.AreEqual( pluginInfo.ID, definition[ pluginInfo.PluginIdentifier ].ID );
            Assert.AreEqual( pluginInfo.Name, definition[ pluginInfo.PluginIdentifier ].Name );
        }

        [Test]
        public void Should_Be_Able_To_Iterate_Through_Installed_IPluginsInfos()
        {
            IAllocationDefinition definition  = new AllocationDefinition(2);
            IPluginInfo           pluginInfo1 = DTO.NewPluginInfoTestPlugin;
            IPluginInfo           pluginInfo2 = DTO.NewPluginInfoTestPlugin;

            pluginInfo2.Version = "2.0.0.0";

            definition.Add(pluginInfo1);
            definition.Add(pluginInfo2);

            int count = 0;

            foreach( IPluginInfo info in definition )
            {
                Assert.IsNotNull( info );
                count++;
            }

            Assert.AreEqual( 2, count );
        }
    }
}