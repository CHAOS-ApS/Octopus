using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Data.Interface;
using Geckon.Octopus.Plugin.Interface;
using NUnit.Framework;

namespace Geckon.Octopus.Agent.Core.Test
{
    [TestFixture]
    public class PluginManagerTest
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
        public void Should_Load_Plugins()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition( _DB.PluginInfo_GetAll(), 2 );

            pluginManager.Install( definition );

            foreach( IPluginInfo plugin in definition )
            {
                Assert.IsTrue( pluginManager.IsAssemblyLoaded(plugin.AssemblyIdentifier ) );
                Assert.IsTrue( pluginManager.IsPluginLoaded(plugin.PluginIdentifier ) );
            }
        }

        private IAllocationDefinition ConvertToDefinition( IEnumerable<PluginInfo> plugins, uint maxSlots )
        {
            IAllocationDefinition definition = new AllocationDefinition( maxSlots );

            foreach( IPluginInfo info in plugins )
            {
                definition.Add( info );
            }

            return definition;
        }

        [Test]
        public void Should_Return_False_If_Assembly_Isnt_Loaded()
        {
            IPluginManager pluginManager = new PluginManager();

            Assert.IsFalse( pluginManager.IsAssemblyLoaded( "This.Assembly.Doesnt.Exist" ) );
        }

        [Test]
        public void Should_Return_False_If_Plugin_Isnt_Loaded()
        {
            IPluginManager pluginManager = new PluginManager();

            Assert.IsFalse( pluginManager.IsPluginLoaded( "ThisPluginDoesntExist" ) );
        }

        [Test]
        public void Should_Return_True_If_Assembly_Is_Loaded()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            IPluginInfo           myPlugin      = null;

            pluginManager.Install( definition );

            foreach( IPluginInfo plugin in _DB.PluginInfo_GetAll() )
            {
                myPlugin = plugin;
                break;
            }

            Assert.IsTrue( pluginManager.IsAssemblyLoaded( myPlugin.AssemblyIdentifier ) );
        }

        [Test]
        public void Should_Return_True_If_Plugin_Is_Loaded()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            IPluginInfo           myPlugin      = null;

            pluginManager.Install( definition );

            foreach( IPluginInfo plugin in _DB.PluginInfo_GetAll() )
            {
                myPlugin = plugin;
                break;
            }

            Assert.IsTrue( pluginManager.IsPluginLoaded( myPlugin.PluginIdentifier ) );
        }

        [Test]
        public void Should_UnInstall_All_PluginDefinitions()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            int count = 0;

            pluginManager.Install( definition );

            foreach( IAllocationDefinition def in pluginManager.GetAllocationDefinitions() )
                count++;
            
            Assert.AreEqual( 1, count );

            count = 0;

            pluginManager.UnInstall( );

            foreach( IAllocationDefinition def in pluginManager.GetAllocationDefinitions() )
                count++;
            
            Assert.AreEqual( 0, count );
        }

        [Test]
        public void Should_Get_Plugin_By_PluginIndentifier()
        {
            IPluginManager        pluginManager    = new PluginManager();
            IAllocationDefinition definition       = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            string                pluginIdentifier = "1.0.0.0, Geckon.Octopus.Plugins.TestPlugin.TestPlugin";

            pluginManager.Install( definition );

            IPlugin plugin = pluginManager.GetPlugin<IPlugin>( pluginIdentifier );

            Assert.IsNotNull( plugin );
        }

        [Test]
        public void Should_Get_Plugin_By_Version_And_Fullname()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            string                version       = "1.0.0.0";
            string                fullname      = "Geckon.Octopus.Plugins.TestPlugin.TestPlugin";

            pluginManager.Install( definition );

            IPlugin plugin = pluginManager.GetPlugin<IPlugin>( version, fullname );

            Assert.IsNotNull( plugin );
        }

        [Test]
        public void Should_Get_Plugin_By_Version_Assembly_And_Classname()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = ConvertToDefinition(_DB.PluginInfo_GetAll(), 2);
            string                version       = "1.0.0.0";
            string                assembly      = "Geckon.Octopus.Plugins.TestPlugin";
            string                classname     = "TestPlugin";

            pluginManager.Install( definition );

            IPlugin plugin = pluginManager.GetPlugin<IPlugin>(version,assembly,classname);

            Assert.IsNotNull( plugin );
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void Should_Throw_FileNotFoundException_If_Assembly_Isnt_Found_When_Insall_Definition()
        {
            IPluginManager        pluginManager = new PluginManager();
            IAllocationDefinition definition    = new AllocationDefinition( 2 );

            definition.Add( new PluginInfo() );

            pluginManager.Install( definition );
        }
    }
}