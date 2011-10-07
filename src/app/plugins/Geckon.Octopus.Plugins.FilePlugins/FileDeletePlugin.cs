using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.FilePlugins
{
	public class FileDeletePlugin : APluginExtendedTemporaryFile
	{
		#region Fields

		private readonly PluginPropertyFilePath _FilePath;

		#endregion

		#region Properties

		[Element("FilePath")]
		public string FilePath
		{
			get { return _FilePath.Value; }
			set
			{
				_FilePath.SetValueIfPropertiesAreEditable(value);
			}
		}

		#endregion

		#region Constructors

		public FileDeletePlugin()
		{
			_FilePath = new PluginPropertyFilePath("FilePath", this);
		}

		#endregion

		#region Business Logic

		protected override void Execute()
		{
			_FilePath.ValidatePropertyIsSet();

			RollbackLevel = ROLLBACK_LEVEL_PROPERTIES_CHECKED;
			
			RenameExistingFile(_FilePath);
		}

		protected override void Commit()
		{
			_TemporaryFilePath.ValidatePropertyIsSet();
			
			base.Commit();
		}

		protected override void Rollback()
		{
			if (RollbackLevel < RollbackLevels.FINAL_LEVEL)
				return;
			
			_FilePath.ValidatePropertyIsSet();
			_TemporaryFilePath.ValidatePropertyIsSet();

			RestoreTemporaryFile(_FilePath);
		}

		#endregion
	}
}