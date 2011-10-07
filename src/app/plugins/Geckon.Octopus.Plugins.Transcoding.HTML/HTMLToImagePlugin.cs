namespace Geckon.Octopus.Plugins.Transcoding.HTML
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using BasePlugins;
	using Plugin.Core;
	using Serialization.Xml;
    using Geckon.Transcoding.HTML;

	public class HTMLToImagePlugin : APluginExtendedTemporaryFile
	{
		#region Fields

		private readonly PluginProperty<int>			_Delay;
		private readonly PluginPropertyFilePath			_DestinationPath;
		private readonly PluginProperty<int>			_Height;
		private readonly PluginPropertyString			_HTMLPath;
		private readonly PluginPropertyValueType<bool>	_ShouldOwerwriteExistingFile;
		private readonly PluginProperty<bool>			_ShouldWaitForJavascript;
		private readonly PluginProperty<int>			_Width;

		#endregion
		#region Properties

		[Element("HTMLPath")]
		public string HTMLPath
		{
			get { return _HTMLPath.Value; }
			set { _HTMLPath.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("DestinationPath")]
		public string DestinationPath
		{
			get { return _DestinationPath.Value; }
			set { _DestinationPath.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("ShouldOwerwriteExistingFile")]
		public bool ShouldOwerwriteExistingFile
		{
			get { return _ShouldOwerwriteExistingFile.Value; }
			set { _ShouldOwerwriteExistingFile.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("Width")]
		public int Width
		{
			get { return _Width.Value; }
			set { _Width.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("Height")]
		public int Height
		{
			get { return _Height.Value; }
			set { _Height.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("Delay")]
		public int Delay
		{
			get { return _Delay.Value; }
			set { _Delay.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("_ShouldWaitForJavascript")]
		public bool ShouldWaitForJavascript
		{
			get { return _ShouldWaitForJavascript.Value; }
			set { _ShouldWaitForJavascript.SetValueIfPropertiesAreEditable(value); }
		}

		#endregion
		public HTMLToImagePlugin()
		{
			_HTMLPath = new PluginPropertyString("HTMLPath", this, false);
			_DestinationPath = new PluginPropertyFilePath("DestinationPath", this);
			_ShouldOwerwriteExistingFile = new PluginPropertyValueType<bool>("ShouldOwerwriteExistingFile", this, false);
			_Width = new PluginProperty<int>("Width", this);
			_Height = new PluginProperty<int>("Height", this);
			_Delay = new PluginProperty<int>("Delay", this, 0);
			_ShouldWaitForJavascript = new PluginProperty<bool>("ShouldWaitForJavascript", this, false);
		}
		#region Business Logic

		protected override void Execute()
		{
			_HTMLPath.ValidatePropertyIsSet();
			_DestinationPath.ValidatePropertyIsSet();
			_Width.ValidatePropertyIsSet();
			_Height.ValidatePropertyIsSet();

			if (Width <= 0 || Height <= 0)
				throw new InvalidOperationException("Width and Height must be greater than zero");

			if (Delay != 0 && ShouldWaitForJavascript)
				throw new InvalidOperationException("Delay can not be set when ShouldWaitForJavascript is set to true");

			RollbackLevel = ROLLBACK_LEVEL_PROPERTIES_CHECKED;

			if (ShouldOwerwriteExistingFile)
				RenameExistingFileIfExist(_DestinationPath);
			else
				_DestinationPath.ValidateFileDoesNotExist();

			RollbackLevel = ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED;

			Bitmap bitmap = null;

			bitmap = ShouldWaitForJavascript
			         	? new HTMLToBitmapTranscoder().GetBitmapOnJavascript(new Uri(HTMLPath), new Size(Width, Height))
			         	: new HTMLToBitmapTranscoder().GetBitmap(new Uri(HTMLPath), new Size(Width, Height), Delay);

			bitmap.Save(DestinationPath, ImageFormat.Png);
		}

		protected override void Commit()
		{
			base.Commit();
		}

		protected override void Rollback()
		{
			if (RollbackLevel >= RollbackLevels.FINAL_LEVEL && _DestinationPath.DoesFileExist)
				File.Delete(DestinationPath);

			RestoreTemporaryFileIfExist(_DestinationPath);
		}

		#endregion
	}
}