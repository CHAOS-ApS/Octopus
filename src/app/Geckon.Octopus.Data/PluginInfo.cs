using System;
using Geckon.Octopus.Data.Interface;

namespace Geckon.Octopus.Data
{
    public partial class PluginInfo : IPluginInfo
    {
        public string AssemblyIdentifier
        {
            get { return CreateAssemblyIdentifier( Version, Assembly ); }
        }

        public string PluginIdentifier
        {
            get { return CreatePluginIdentifier( Version, Assembly, Classname ); }
        }

        public static string CreateFullname(string assembly, string classname)
        {
            return assembly + "." + classname;
        }

        public static string CreateAssemblyIdentifier( string version, string assembly  )
        {
            return version + ", " + assembly;
        }

        public static string CreatePluginIdentifier(string version, string assembly, string classname)
        {
            return CreateAssemblyIdentifier(version, assembly) + "." + classname;
        }

        public static string CreatePluginIdentifier(string version, string fullname )
        {
            return CreateAssemblyIdentifier(version, fullname);
        }
    }
}
