using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public abstract class APluginExtended : APlugin, IPluginExtended
	{
		#region Fields

		protected const uint ROLLBACK_LEVEL_PROPERTIES_CHECKED = 100;

		#endregion
		#region Properties

		public bool ArePropertiesEditable
		{
			get { return Status == PluginStatus.Initializing || Status == PluginStatus.Initialized; }
		}

		#endregion
	}
}