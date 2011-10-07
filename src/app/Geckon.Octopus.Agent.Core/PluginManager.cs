using System;
using System.Collections.Generic;
using System.IO;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Data.Interface;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Core
{
    public class PluginManager : IPluginManager
    {
        #region Fields

        private readonly IList<IAllocationDefinition>  _AllocationDefinitions;
 
        #endregion
        #region Properties

        private IList<IAllocationDefinition> AllocationDefinitions
        {
            get { return _AllocationDefinitions; }
        }

        #endregion
        #region Construction

        public PluginManager()
        {

            _AllocationDefinitions = new List<IAllocationDefinition>();
        }

        #endregion
        #region Business Logic

        public void Install( IAllocationDefinition allocationDefinition )
        {
            foreach( IPluginInfo pluginInfo in allocationDefinition )
            {
                if( !File.Exists( pluginInfo.ReadURL ) )
                    throw new FileNotFoundException( string.Format( "The assembly '{0}' doesn't exist", pluginInfo.ReadURL ) );
            }

            foreach( IPluginInfo pluginInfo in allocationDefinition )
            {
                if( !PluginLoader.IsAssemblyLoaded( pluginInfo.AssemblyIdentifier ) )
                    PluginLoader.Add(pluginInfo.AssemblyIdentifier, pluginInfo.ReadURL);
            }

            AllocationDefinitions.Add( allocationDefinition );
        }

        public void UnInstall()
        {
            AllocationDefinitions.Clear();
            PluginLoader.Clear();
        }

        public bool IsPluginLoaded( string pluginIdentifier )
        {
            foreach( IAllocationDefinition definition in AllocationDefinitions )
            {
                if( definition.Contains( pluginIdentifier ) )
                    return true;
            }

            return false;
        }


        public bool IsAssemblyLoaded(string assemblyIdentifier)
        {
            return PluginLoader.IsAssemblyLoaded(assemblyIdentifier);
        }

        public IEnumerable<IAllocationDefinition> GetAllocationDefinitions()
        {
            foreach( IAllocationDefinition definition in AllocationDefinitions )
            {
                yield return definition;
            }
        }

        public T GetPlugin<T>( string pluginIdentifier ) where T : IPlugin
        {
            foreach( IAllocationDefinition allocationDefinition in AllocationDefinitions )
            {
                IPluginInfo pluginInfo = allocationDefinition[ pluginIdentifier ];

                if( pluginInfo != null )
                    return PluginLoader.GetPlugin<T>( pluginInfo.AssemblyIdentifier, pluginInfo.Assembly, pluginInfo.Classname );

            }

            throw new ArgumentOutOfRangeException( "The pluginIdentifier was not found" );
        }

        public T GetPlugin<T>( string version, string assembly, string classname ) where T : IPlugin
        {
            return GetPlugin<T>( PluginInfo.CreatePluginIdentifier( version, assembly, classname ) );
        }

        public T GetPlugin<T>( string version, string fullname ) where T : IPlugin
        {
            return GetPlugin<T>( version + ", " + fullname );
        }

        #endregion

    }
}