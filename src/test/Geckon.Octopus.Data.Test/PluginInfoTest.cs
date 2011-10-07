using System.Collections.Generic;
using System.Text.RegularExpressions;
using Geckon.Octopus.Data.Interface;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Data.Test
{
    [TestFixture]
    public class PluginInfoTest
    {
        #region Setup and Teardown

        private DatabaseDataContext _DB;

        [SetUp]
        public void SetUp()
        {
            _DB = new DatabaseDataContext();

            _DB.Test_CleanAndInsertDummyData( Regex.Replace(System.Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "bin\\plugins\\") );
        }

        [TearDown]
        public void TearDown()
        {
            _DB.Test_Clean();
            _DB.Dispose();
        }

        #endregion

        [Test]
        public void Should_Get_By_Classname()
        {
            IEnumerable<PluginInfo> plugins = _DB.PluginInfo_GetByClassname( "TestPlugin" );

            int count = 0;
            foreach( PluginInfo plugin in plugins )
                count++;

            Assert.AreEqual( 1, count );
        }

        [Test]
        public void Should_Get_By_Assembly()
        {
            IEnumerable<PluginInfo> plugins = _DB.PluginInfo_GetByAssemblyname("Geckon.Octopus.Plugins.TestPlugin");

            int count = 0;
            foreach( PluginInfo plugin in plugins )
                count++;

            Assert.AreEqual( 3, count );
        }

        [Test]
        public void Should_Get_By_Classname_Assembly_And_Version()
        {
            IEnumerable<PluginInfo> plugins = _DB.PluginInfo_GetBy(null,null,null,"1.0.0.0", null, "Geckon.Octopus.Plugins.TestPlugin", null, null, null, null);

            int count = 0;
            foreach( PluginInfo plugin in plugins )
                count++;

            Assert.AreEqual( 3, count );
        }

        [Test]
        public void Should_Get_All()
        {
            IEnumerable<PluginInfo> plugins = _DB.PluginInfo_GetAll();

            int count = 0;
            foreach( PluginInfo plugin in plugins )
                count++;

            Assert.AreEqual( 7, count );
        }

        [Test]
        public void Should_Generate_AssemblyIdentifier(  )
        {
            IPluginInfo plugin = DTO.NewPluginInfoTestPlugin;

            Assert.AreEqual( plugin.Version + ", " + plugin.Assembly, plugin.AssemblyIdentifier );
        }

        [Test]
        public void Should_Generate_PluginIdentifier()
        {
            IPluginInfo plugin = DTO.NewPluginInfoTestPlugin;

            Assert.AreEqual(plugin.Version + ", " + plugin.Assembly + "." + plugin.Classname, plugin.PluginIdentifier);
        }

        [Test]
        public void Should_Create_Fullname()
        {
            string classname = "Classname";
            string  assembly = "Ass.Em.Bly";
            
            Assert.AreEqual( "Ass.Em.Bly.Classname", PluginInfo.CreateFullname( assembly, classname ) );
        }

        [Test]
        public void Should_Create_PluginIdentifer()
        {
            string version   = "1.0.0.0";
            string classname = "Classname";
            string assembly  = "Ass.Em.Bly";

            Assert.AreEqual("1.0.0.0, Ass.Em.Bly.Classname", PluginInfo.CreatePluginIdentifier(version, assembly, classname) );
        }

        [Test]
        public void Should_Create_AssemblyIdentifer()
        {
            string version   = "1.0.0.0";
            string assembly  = "Ass.Em.Bly";

            Assert.AreEqual("1.0.0.0, Ass.Em.Bly", PluginInfo.CreateAssemblyIdentifier(version, assembly ));
        }
    }
}
