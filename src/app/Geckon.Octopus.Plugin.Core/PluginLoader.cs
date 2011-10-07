using System;
using System.Collections.Generic;
using System.Reflection;
using Geckon.Logging;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Plugin.Core
{
    public static class PluginLoader
    {
        #region Fields

        private static readonly IDictionary<string, Assembly> _LoadedAssemblies = new Dictionary<string, Assembly>();

        #endregion
        #region Properties

        private static IDictionary<string, Assembly> LoadedAssemblies
        {
            get { return _LoadedAssemblies; }
        }

        public static int Count
        {
            get { return LoadedAssemblies.Count;  }
        }

        #endregion
        #region Construction



        #endregion
        #region Business Logic

        public static void Add( string assemblyIdentifier, string assemblyUrl )
        {
            LoadedAssemblies.Add( assemblyIdentifier, Assembly.LoadFrom( assemblyUrl ) );
        }

        public static void Clear()
        {
            LoadedAssemblies.Clear();
        }

        public static bool IsAssemblyLoaded( string assemblyIdentifier )
        {
            return LoadedAssemblies.ContainsKey( assemblyIdentifier );
        }

        public static T GetPlugin<T>( string assemblyIdentifier, string assembly, string classname ) where T : IPlugin
        {
            return (T) LoadedAssemblies[ assemblyIdentifier ].CreateInstance( assembly + "." + classname );
        }

        public static T GetPlugin<T>( string version, string fullname ) where T : IPlugin
        {
            //Log.Write("PluginLoader GetPlugin - Started", LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");

            string assembly = fullname.Substring( 0, fullname.LastIndexOf(".") );
            string key      = version + ", " + assembly;

            if( !LoadedAssemblies.ContainsKey( key ) )
                throw new KeyNotFoundException( key + " wasn't present in the dictionary" );

            foreach (KeyValuePair<string, Assembly> pair in LoadedAssemblies)
            {
                //Log.Write("PluginLoader GetPlugin - pair.Key = " + pair.Key, LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");
                //Log.Write("PluginLoader GetPlugin - key      = " + key, LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");
                //Log.Write("PluginLoader GetPlugin - pair.Value.FullName      = " + pair.Value.FullName, LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");

                foreach (Type type in pair.Value.GetTypes())
                {
                    //Log.Write("PluginLoader GetPlugin - Type.FullName = " + type.FullName, LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");
                }
            }
            
            //Log.Write("PluginLoader GetPlugin - Ended", LogType.Error, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");

            return (T)LoadedAssemblies[ key ].CreateInstance( fullname );
        }

        #endregion
    }
}