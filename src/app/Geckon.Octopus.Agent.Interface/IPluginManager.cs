using System.Collections;
using System.Collections.Generic;
using Geckon.Octopus.Data.Interface;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Agent.Interface
{
    public interface IPluginManager
    {
        /// <summary>
        /// Define an Allocation definition.
        /// </summary>
        /// <param name="definition">The IAllocationDefinition to install</param>
        void Install( IAllocationDefinition definition );

        /// <summary>
        /// UnInstalls a single plugin.
        /// </summary>
        void UnInstall();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyIdentifier"></param>
        /// <returns></returns>
        bool IsAssemblyLoaded( string assemblyIdentifier );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        bool IsPluginLoaded( string pluginIdentifier );

        /// <summary>
        /// Used to iterate through all Installed Definition
        /// </summary>
        /// <returns>IEnumerable</returns>
        IEnumerable<IAllocationDefinition> GetAllocationDefinitions();

        /// <summary>
        /// Returns an instance of the plugin if the proper definition is installed.
        /// </summary>
        /// <typeparam name="T">T must inherit from IPlugin</typeparam>
        /// <param name="pluginIdentifier">The unique identifier</param>
        /// <returns>An instance of IPlugin cast to T</returns>
        T GetPlugin<T>( string pluginIdentifier ) where T : IPlugin;

        /// <summary>
        /// Returns an instance of the plugin if the proper definition is installed.
        /// </summary>
        /// <typeparam name="T">T must inherit from IPlugin</typeparam>
        /// <param name="version">The version string</param>
        /// <param name="assembly">The assembly namespace</param>
        /// <param name="classname">The classname</param>
        /// <returns>An instance of IPlugin cast to T</returns>
        T GetPlugin<T>(string version, string assembly, string classname) where T : IPlugin;

        /// <summary>
        /// Returns an instance of the plugin if the proper definition is installed.
        /// </summary>
        /// <typeparam name="T">T must inherit from IPlugin</typeparam>
        /// <param name="fullname">The Fullname</param>
        /// <param name="version">The version string</param>
        /// <returns>An instance of IPlugin cast to T</returns>
        T GetPlugin<T>(string version, string fullname) where T : IPlugin;
    }
}