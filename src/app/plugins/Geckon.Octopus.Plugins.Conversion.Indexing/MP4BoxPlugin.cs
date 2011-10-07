using System.Diagnostics;
using System.Text.RegularExpressions;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Conversion.Indexing
{
    public class MP4BoxPlugin : APluginExtendedFileOperation
    {
        #region Fields

        private PluginPropertyFilePath _MP4BoxPath;

        #endregion
        #region Properties

        [Element("MP4BoxPath")]
        public string MP4BoxPath
        {
            get { return _MP4BoxPath.Value; }
            set
            {
                _MP4BoxPath.SetValueIfPropertiesAreEditable(value);
            }
        }

        #endregion
        #region Construction

        public MP4BoxPlugin()
        {
            _MP4BoxPath = new PluginPropertyFilePath( "MP4BoxPath", this );
        }

        #endregion
        #region Business Logic

        protected override void Execute()
        {
            if( _MP4BoxPath.IsSet )
                _MP4BoxPath.ValidateFileExist();

            using( Process p = new Process() )
            {
                p.StartInfo.UseShellExecute        = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName               = MP4BoxPath;
                p.StartInfo.Arguments              = string.Format("-add {0} {1} {2}", SourceFilePath, ShouldOwerwriteExistingFile ? "-new" : "", DestinationFilePath);
                p.StartInfo.CreateNoWindow         = true;

                p.OutputDataReceived += p_OutputDataReceived;
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
            }
        }

        void p_OutputDataReceived( object sender, DataReceivedEventArgs e )
        {
            OperationProgress +=  1/300.0;
        }

        #endregion
    }
}
