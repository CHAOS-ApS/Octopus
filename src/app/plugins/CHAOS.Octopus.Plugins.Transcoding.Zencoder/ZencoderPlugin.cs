using System;
using System.Linq;
using Geckon.Octopus.Plugin.Core;
using Geckon.Serialization.Xml;
using Zen = Zencoder;

namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public class ZencoderPlugin : APlugin
    {
        #region Properties

        [Element("ZencoderApiKey")]
        public string ZencoderApiKey { get; set; }

        [Element("DestinationFilePath")]
        public string DestinationFilePath { get; set; }

        [Element("SourceFilePath")]
        public string SourceFilePath { get; set; }

        [Element("Label")]
        public string Label { get; set; }

        [Element("VideoBitrate")]
        public int? VideoBitrate { get; set; }

        [Element("AudioBitrate")]
        public int? AudioBitrate { get; set; }

        [Element("Width")]
        public int Width { get; set; }

        [Element("Height")]
        public int Height { get; set; }



        #endregion
        #region Business

        protected override void Execute()
        {
            base.Execute();
            
            var zencoder = new Zen.Zencoder(ZencoderApiKey, new Uri("https://app.zencoder.com/api/v2/"));
            var outputs = new Zen.Output[]
                              {
                                  new Zen.Output
                                      {
                                          Label        = Label,
                                          Url          = DestinationFilePath,
                                          Width        = Width,
                                          Height       = Height,
                                          VideoBitrate = VideoBitrate,
                                          AudioBitrate = AudioBitrate
                                      }, 
                              };

            var response = zencoder.CreateJob(SourceFilePath, outputs, null, "eu-dublin", false, false);
            var output   = response.Outputs.First();

            for (var outputState = zencoder.JobProgress(output.Id).State; outputState != Zen.OutputState.Finished && outputState != Zen.OutputState.Cancelled && outputState != Zen.OutputState.Failed; outputState = zencoder.JobProgress(output.Id).State)
            {
                OperationProgress = zencoder.JobProgress(output.Id).Progress / 100.0;
            }
        }

        #endregion
    }
}
