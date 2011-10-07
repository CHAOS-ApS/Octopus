using System;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyString : PluginPropertyNotNull<string>
	{
		#region Fields

		private readonly bool _AllowEmpty;

		#endregion

		#region Properties

		public bool AllowEmpty
		{
			get { return _AllowEmpty; }
		}

		#endregion

		#region Constructors

		public PluginPropertyString(string name, IPluginExtended plugin)
			: base(name, plugin)
		{
			
		}

		public PluginPropertyString(string name, IPluginExtended plugin, string value)
			: base(name, plugin, value)
		{
		}

		public PluginPropertyString(string name, IPluginExtended plugin, bool allowEmpty)
			: this(name, plugin)
		{
			_AllowEmpty = allowEmpty;
		}

		public PluginPropertyString(string name, IPluginExtended plugin, string value, bool allowEmpty)
			: this(name, plugin, value)
		{
			_AllowEmpty = allowEmpty;
		}

		#endregion

		#region Business Logic

		protected override bool IsValid(string value)
		{
			return base.IsValid(value) && (AllowEmpty || value != string.Empty);
		}

		#endregion
	}
}