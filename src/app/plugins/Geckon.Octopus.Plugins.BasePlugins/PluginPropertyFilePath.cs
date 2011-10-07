using System;
using System.IO;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PluginPropertyFilePath : PluginPropertyDirectoryPath
	{
		#region Fields

		#endregion
		#region Properties

		public string FilePath
		{
			get { return Value; }
		}

		public override string DirectoryPath
		{
			get { return Path.GetDirectoryName(FilePath); }
		}

		public bool DoesFileExist
		{
			get
			{
				if(!IsSet)
					return false;

				return File.Exists(FilePath);
			}
		}

		public string FileName
		{
			get
			{
				return Path.GetFileName(FilePath);
			}
		}

		public string FileNameWithoutExtension
		{
			get
			{
				return Path.GetFileNameWithoutExtension(FilePath);
			}
		}

		public string FileExtension
		{
			get
			{
				return Path.GetExtension(FilePath);
			}
		}

		#endregion
		#region Constructors

		public PluginPropertyFilePath(string name, IPluginExtended plugin)
			: base(name, plugin)
		{

		}

		public PluginPropertyFilePath(string name, IPluginExtended plugin, string value)
			: base(name, plugin, value)
		{
			
		}

		#endregion
		#region Business Logic

		protected override bool IsValid(string value)
		{
			if (!base.IsValid(value))
				return false;

			return Path.GetFileName(value) != String.Empty;
		}

		public void ValidateFileExist()
		{
			if (!DoesFileExist)
				throw new FileNotFoundException(Name + " not found", FilePath);
		}

		public void ValidateFileDoesNotExist()
		{
			if (DoesFileExist)
				throw new FileAlreadyExistException(FilePath);
		}

		#endregion
	}
}