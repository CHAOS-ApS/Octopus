using System;
using System.IO;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyUri : PluginPropertyString
	{
		#region Fields

		private readonly bool _IsFile = true;

		#endregion

		#region Properties

		public string LocalPath
		{
			get { return new Uri(Value).LocalPath; }
		}

		public bool IsFile
		{
			get { return _IsFile; }
		}

		public bool DoesUriExist
		{
			get
			{
				if(!IsSet)
					return false;
				
				if(IsFile)
					return File.Exists(LocalPath);
				
				return Directory.Exists(LocalPath);
			}
		}

		public string DirectoryPath
		{
			get
			{
				return Path.GetDirectoryName(LocalPath);
			}
		}

		#endregion

		#region Constructors

		public PluginPropertyUri(string name, IPluginExtended plugin)
			: base(name, plugin)
		{

		}

		public PluginPropertyUri(string name, IPluginExtended plugin, string value)
			: base(name, plugin, value)
		{
			
		}

		public PluginPropertyUri(string name, IPluginExtended plugin, bool isFile)
			: this(name, plugin)
		{
			_IsFile = isFile;
		}

		public PluginPropertyUri(string name, IPluginExtended plugin, string value, bool isFile)
			: this(name, plugin, value)
		{
			_IsFile = isFile;
		}

		#endregion

		#region Business Logic

		protected override bool IsValid(string value)
		{
			if (!base.IsValid(value))
				return false;
			
			try
			{
				new Uri(value, UriKind.RelativeOrAbsolute); //Check if Uri is valid.
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void ValidateUriExist()
		{
			if (DoesUriExist)
				return;
			if(IsFile)
				throw new FileNotFoundException(Name + " not found", LocalPath);
				
			throw new DirectoryNotFoundException(Name +" not found: " + LocalPath);
		}

		public void ValidateUriDoesNotExist()
		{
			if (!DoesUriExist)
				return;
			if (IsFile)
				throw new FileAlreadyExistException(LocalPath);

			throw new DirectoryAlreadyExistException(LocalPath);
		}

		public void  ValidateDirectoryPathExist()
		{
			if(!Directory.Exists(DirectoryPath))
				throw new DirectoryNotFoundException("Directory for " + Name + " not found: " + DirectoryPath);
		}

		#endregion
	}
}