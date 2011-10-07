using System;
using System.IO;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyDirectoryPath : PluginPropertyString
	{
		#region Fields

		#endregion
		#region Properties

		public virtual string DirectoryPath
		{
			get { return Value; }
		}

		public bool DoesDirectoryExist
		{
			get
			{
				if (!IsSet)
					return false;

				return Directory.Exists(DirectoryPath);
			}
		}

		#endregion
		#region Construction

		public PluginPropertyDirectoryPath(string name, IPluginExtended plugin)
			: base(name, plugin)
		{

		}

		public PluginPropertyDirectoryPath(string name, IPluginExtended plugin, string value)
			: base(name, plugin, value)
		{
			
		}

		#endregion
		#region Business Logic

		protected override bool IsValid(string value)
		{
			if (!base.IsValid(value))
				return false;

			try
			{
				Path.GetFullPath(value); //Check if Path is valid.
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void ValidateDirectoryExist()
		{
			if (!DoesDirectoryExist)
				throw new DirectoryNotFoundException("Directory " + Name + " not found: " + DirectoryPath);
		}

		public void ValidateDirectoryDoesNotExist()
		{
			if (DoesDirectoryExist)
				throw new DirectoryAlreadyExistException(DirectoryPath);
		}

		#endregion
	}
}