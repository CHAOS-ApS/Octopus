using System.Linq;
using Geckon.Serialization.Xml;
using Zen = Zencoder;


namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public class CutStillPlugin : ZencoderBasePlugin
    {
        #region Properties

        [Element("Number")]
        public int? Number { get; set; }

        #endregion
        #region 
        
        protected override void Execute()
        {
            base.Execute();

            var zencoder   = GetNewZencoder();
            var thumbnails = new Zen.Thumbnails
                                {
                                    BaseUrl = DestinationFilePath.Substring(0, DestinationFilePath.LastIndexOf("/")),
                                    FileName = (Number == 1 ? "" : "{{number}}_") + DestinationFilePath.Substring(DestinationFilePath.LastIndexOf("/") + 1, DestinationFilePath.LastIndexOf(".") - (DestinationFilePath.LastIndexOf("/") + 1)),
                                    Width = Width,
                                    Height = Height,
                                    Number = Number,
                                    Label = Label ?? "default",
                                    Public = Public
                                };
            var outputs = new[]
                                {
                                    new Zen.Output().WithThumbnails(thumbnails)
                                };
            
            var response = zencoder.CreateJob(SourceFilePath, outputs, null, "eu-dublin", false, false);
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
