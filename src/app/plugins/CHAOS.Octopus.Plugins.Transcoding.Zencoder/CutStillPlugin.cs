using System.Linq;
using Geckon.Serialization.Xml;
using Zen = Zencoder;


namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public class CutStillPlugin : ZencoderBasePlugin
    {
        #region Properties

        [Element("Number")]
        public uint Number { get; set; }

        #endregion
        #region 
        
        protected override void Execute()
        {
            base.Execute();

            var zencoder = GetNewZencoder();
            var outputs  = new[]
                              {
                                  new Zen.Output
                                      {
                                          Url          = DestinationFilePath,
                                          Width        = Width,
                                          Height       = Height,
                                          Public       = Public,
                                          Number       = Number
                                      }, 
                              };

            var response = zencoder.CreateJob(SourceFilePath, outputs, null, "eu-dublin", false, false);
            var output = response.Outputs.First();

            for (var outputState = zencoder.JobProgress(output.Id).State; outputState != Zen.OutputState.Finished && outputState != Zen.OutputState.Cancelled && outputState != Zen.OutputState.Failed; outputState = zencoder.JobProgress(output.Id).State)
            {
                OperationProgress = zencoder.JobProgress(output.Id).Progress / 100.0;
            }
        }

        #endregion
    }
}
