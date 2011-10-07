using System.IO;
using Geckon.Graphics;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Image
{
	public class ImageResizePlugin : APluginExtendedFileOperation
	{
		#region Fields

		private readonly PluginPropertyValueType<int> _Amount;
		private readonly PluginPropertyValueType<ResizeType> _Type;
		private readonly PluginPropertyValueType<int> _Quality;

		#endregion
		#region Properties

		[Element("Amount")]
		public int Amount
		{
			get { return _Amount.Value; }
			set
			{
				_Amount.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("Type")]
		public ResizeType Type
		{
			get { return _Type.Value; }
			set
			{
				_Type.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("Quality")]
		public int Quality
		{
			get { return _Quality.Value; }
			set
			{
				_Quality.SetValueIfPropertiesAreEditable(value);
			}
		}

		#endregion
		#region Constructors

		public ImageResizePlugin()
		{
			_Amount = new PluginPropertyValueType<int>("Amount", this);
			_Amount.AddIsInRangeTest(amount => amount > 0);

			_Type = new PluginPropertyValueType<ResizeType>("Type", this);

			_Quality = new PluginPropertyValueType<int>("Quality", this);
			_Quality.AddIsInRangeTest(quality => quality >= 0 && quality <= 100);
		}

		#endregion
		#region Business Logic

		protected override void Execute()
		{
			_Amount.ValidatePropertyIsSet();
			_Type.ValidatePropertyIsSet();
			_Quality.ValidatePropertyIsSet();
			
			base.Execute();

			using( var image = new Imaging( _SourceFilePath.FilePath ) )
		    {
				using (var watcher = new FileSystemWatcher(Path.GetDirectoryName(DestinationFilePath)))
				{
					watcher.Filter = Path.GetFileName(DestinationFilePath);
					watcher.Created += WatcherCreated;
					watcher.EnableRaisingEvents = true;

					image.Resize(Amount, Type, Quality, _DestinationFilePath.FilePath);
				}
		    }
		}

		private void WatcherCreated(object sender, FileSystemEventArgs e)
		{
			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;
		}

		protected override void Rollback()
		{
			base.Rollback();

			if (RollbackLevel >= ROLLBACK_LEVEL_FILE_OPERATION_BEGUN)
			{
				_DestinationFilePath.ValidateFileExist();
				File.Delete(_DestinationFilePath.FilePath);
			}

			if (RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}