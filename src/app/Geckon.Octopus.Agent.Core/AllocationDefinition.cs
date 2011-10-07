using System.Collections;
using System.Collections.Generic;
using Geckon.Octopus.Agent.Interface;
using Geckon.Octopus.Data.Interface;
using System;

namespace Geckon.Octopus.Agent.Core
{
    public class AllocationDefinition : IAllocationDefinition
    {
        #region Fields

        private uint _MaxSlots;
        private IDictionary<string, IPluginInfo> _InstalledPlugins;

        #endregion
        #region Properties

        public uint MaxSlots
        {
            get { return _MaxSlots; }
            set { _MaxSlots = value; }
        }

        public IPluginInfo this[string pluginIdentifier]
        {
            get { return InstalledPlugins[pluginIdentifier]; }
        }

        public bool Contains(string pluginIdentifier)
        {
            return InstalledPlugins.ContainsKey( pluginIdentifier );
        }

        protected IDictionary<string, IPluginInfo> InstalledPlugins
        {
            get { return _InstalledPlugins; }
            set { _InstalledPlugins = value; }
        }

        #endregion
        #region Constructor

        public AllocationDefinition(uint maxSlots)
        {
            if (maxSlots == 0)
                throw new ArgumentException("MaxSlots cannot be zero");

            _MaxSlots = maxSlots;
            _InstalledPlugins = new Dictionary<string, IPluginInfo>();
        }

        #endregion
        #region Business Logic

        public void Add(IPluginInfo pluginInfo)
        {
            if (InstalledPlugins.ContainsKey(pluginInfo.PluginIdentifier))
                return;

            InstalledPlugins.Add(pluginInfo.PluginIdentifier, pluginInfo);
        }

        public IEnumerator<IPluginInfo> GetEnumerator()
        {
            foreach (KeyValuePair<string, IPluginInfo> plugin in InstalledPlugins)
                yield return plugin.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
