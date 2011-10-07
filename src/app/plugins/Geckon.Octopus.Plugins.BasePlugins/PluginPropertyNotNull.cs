using System;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyNotNull<T> : PluginProperty<T> where T : class
	{
		#region Properties

		public override bool IsSet
		{
			get
			{
				return Value != null;
			}
		}

		#endregion

		#region Constructors

		public PluginPropertyNotNull(string name, IPluginExtended plugin)
			: base(name, plugin)
		{
			
		}

		public PluginPropertyNotNull(string name, IPluginExtended plugin, T value)
			: base(name, plugin, value)
		{

		}

		#endregion

		#region Business Logic

		protected override void ValidateValue(T value)
		{
			if (value == null)
				throw new ArgumentNullException(Name);

			base.ValidateValue(value);
		}

		#endregion
	}
}