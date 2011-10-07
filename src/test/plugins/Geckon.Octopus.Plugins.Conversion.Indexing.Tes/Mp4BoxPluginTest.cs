using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Conversion.Indexing.Test
{
    [TestFixture]
    public class Mp4BoxPluginTest
    {
        [Test]
        public void ShouldSetMP4BoxPath()
        {
            MP4BoxPlugin plugin = new MP4BoxPlugin();

            plugin.MP4BoxPath = @"..\..\..\..\..\..\lib\MP4Box.exe";
            Assert.AreEqual(@"..\..\..\..\..\..\lib\MP4Box.exe", plugin.MP4BoxPath);
        }

        [Test]
        public void ShouldExecute()
        {
            MP4BoxPlugin plugin = new MP4BoxPlugin();

            plugin.MP4BoxPath                  = @"..\..\..\..\..\..\lib\MP4Box.exe";
            plugin.SourceFilePath              = "file.mp4";
            plugin.DestinationFilePath         = "indexed.mp4";
            plugin.ShouldOwerwriteExistingFile = true;

            bool isDone = false;
            plugin.ExecuteCompleted += delegate
                                           {
                                               isDone = true;
                                           };

            plugin.ExecuteFailed += delegate { Assert.Fail("Execute Failed"); };

            plugin.BeginExecute();

            Timing.WaitUntil( () => ( plugin.Status == PluginStatus.Executed ), 50 );

            Assert.AreEqual(@"..\..\..\..\..\..\lib\MP4Box.exe", plugin.MP4BoxPath);
        }
    }
}
