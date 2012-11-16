using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Transcoding.Image.Test.S3
{
    [TestFixture]
    public class ImageResizePluginTest
    {
        [Test]
        public void Should_Extract_Using_Regex()
        {
            //  

            var regexMatch = Regex.Match("https://s3-eu-west-1.amazonaws.com/chaosdata/2012/10/29/af19cdcb-92cc-fc41-aa9b-5be349f6595b.png", @"^http[s]*://(.+)\.amazonaws\.com/(.+?)/(.+)$");

            var region     = regexMatch.Groups[1].Value;
            var bucketName = regexMatch.Groups[2].Value;
            var key        = regexMatch.Groups[3].Value;

            Assert.AreEqual("s3-eu-west-1", region);
            Assert.AreEqual("chaosdata", bucketName);
            Assert.AreEqual("2012/10/29/af19cdcb-92cc-fc41-aa9b-5be349f6595b.png", key);
        }
    }
}
