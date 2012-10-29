using System.Linq;
using System.Text.RegularExpressions;
using Geckon.Serialization.Xml;
using Zen = Zencoder;

namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public class ZencoderPlugin : ZencoderBasePlugin
    {
        #region Properties

        [Element("VideoBitrate")]
        public int? VideoBitrate { get; set; }

        [Element("AudioBitrate")]
        public int? AudioBitrate { get; set; }

        #endregion
        #region Business

        protected override void Execute()
        {
            base.Execute();

            var sourceSettings      = GetS3Settings(SourceFilePath);
            var destinationSettings = GetS3Settings(DestinationFilePath);

            var zencoder = GetNewZencoder();
            var outputs  = new[]
                              {
                                  new Zen.Output
                                      {
                                          Label        = Label,
                                          Url          = ToZencoderUrl(destinationSettings),
                                          Width        = Width,
                                          Height       = Height,
                                          VideoBitrate = VideoBitrate,
                                          AudioBitrate = AudioBitrate,
                                          Public       = Public
                                      }, 
                              };

            var response = zencoder.CreateJob(ToZencoderUrl(sourceSettings), outputs, null, "eu-dublin", false, false);
            var output   = response.Outputs.First();

            for (var outputState = zencoder.JobProgress(output.Id).State; outputState != Zen.OutputState.Finished && outputState != Zen.OutputState.Cancelled && outputState != Zen.OutputState.Failed; outputState = zencoder.JobProgress(output.Id).State)
            {
                OperationProgress = zencoder.JobProgress(output.Id).Progress / 100.0;
            }
        }

        // TODO: Implement Rollback

        #endregion
    }
}
