using System.Collections.Generic;
using Geckon.Octopus.Data.Interface;

namespace Geckon.Octopus.Agent.Interface
{
    public interface IAllocationDefinition : IEnumerable<IPluginInfo>
    {
        /// <summary>
        /// The Max number of slots
        /// </summary>
        uint MaxSlots { get; set; }

        /// <summary>
        /// Adds an IPluginInfo to the AllocationSlot
        /// </summary>
        /// <param name="pluginInfo"></param>
        void Add(IPluginInfo pluginInfo);

        /// <summary>
        /// Get a IPluginInfo based on it's pluginIdentifier
        /// </summary>
        /// <param name="pluginIdentifier">pluginIdentifier</param>
        /// <returns>IPluginInfo</returns>
        IPluginInfo this[string pluginIdentifier] { get; }

        /// <summary>
        /// Return true if a plugin with the same identifier exists, otherwise false
        /// </summary>
        /// <param name="pluginIdentifier">IPluginInfo.PluginIdentifier</param>
        /// <returns></returns>
        bool Contains( string pluginIdentifier );
    }
}