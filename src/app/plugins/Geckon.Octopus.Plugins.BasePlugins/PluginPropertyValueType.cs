namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyValueType<T> : PluginProperty<T> where T: struct
	{
		#region Fields

		protected bool _IsSet;

		#endregion

		#region Properties

		public override bool IsSet
		{
			get
			{
				return _IsSet;
			}
		}

		public override T Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				base.Value = value;

				_IsSet = true;
			}
		}

		#endregion

		#region Constructors

		public PluginPropertyValueType(string name, IPluginExtended plugin) : base(name, plugin)
		{
		}

		public PluginPropertyValueType(string name, IPluginExtended plugin, T value) : base(name, plugin, value)
		{
			_IsSet = true;
		}

		#endregion

		#region Business Logic

		#endregion
	}
}