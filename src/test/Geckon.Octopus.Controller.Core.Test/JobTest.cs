using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.TestPlugin;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;

namespace Geckon.Octopus.Controller.Core.Test
{
    [TestFixture]
    public class JobTest
    {
        #region SetUp/TearDown

        private DatabaseDataContext _DB;

        [SetUp]
        public void SetUp()
        {
            _DB = new DatabaseDataContext();

			_DB.Test_CleanAndInsertDummyData(Regex.Replace(Environment.CurrentDirectory, "(src)(\\\\)(test)(\\\\)[\\w.-]+(\\\\)(bin)(\\\\)(Debug|Release)$", "bin\\plugins\\"));
            foreach (AssemblyInfo info in _DB.AssemblyInfo_Get(null, null, null, null, null, null, null, true))
            {
                PluginLoader.Add(info.Version + ", " + info.Name, info.ReadURL); // Add AssemblyIdentifier !!!!!!!!!!!!!!
            }
        }

        [TearDown]
        public void TearDown()
        {
            _DB.Test_Clean();
            _DB.Dispose();
            PluginLoader.Clear();
        }

        #endregion


        [Test]
        public void Should_Initialize_Job_With_JobData()
        {
            IJobData jobdata = DTO.JobData;
            IJob     job     = new Job(jobdata );

            Assert.AreEqual(jobdata.ID, job.ID);
            Assert.AreEqual(jobdata.StatusID, job.StatusID);
            Assert.AreEqual(jobdata.CreatedDate, job.CreatedDate);

            Assert.AreEqual(2, job.Count);

			Assert.AreEqual(5, job[0].Count);
        }

        [Test]
        public void Should_Initialize_Job_With_JobData2()
        {
            IJobData jobdata = DTO.JobData;

            jobdata.JobXML = XElement.Parse( "<?xml version=\"1.0\" encoding=\"utf-8\"?><Job ClientID=\"0\" xmlns=\"http://www.w3.org/1999/xhtml\"><Step><Plugin Class=\"Geckon.Octopus.Plugins.FilePlugins.FileCopyPlugin\" Status=\"Initialized\" Version=\"1.0.0.0\"><SourceFilePath>\\\\91.197.249.65\\c$\\portal_files\\source\\2010\\10\\13\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4</SourceFilePath><DestinationFilePath>\\\\91.197.249.65\\c$\\portal_files\\source\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4</DestinationFilePath><ShouldOwerwriteExistingFile>True</ShouldOwerwriteExistingFile></Plugin></Step><Step><Plugin Class=\"Geckon.Octopus.Plugins.FilePlugins.FileDeletePlugin\" Status=\"Initialized\" Version=\"1.0.0.0\"><SourceFilePath>\\\\91.197.249.65\\c$\\portal_files\\source\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4</SourceFilePath></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.Net.HTTPRequest.HTTPRequestPlugin\" Status=\"Initialized\" Version=\"1.0.0.0\"><Execute_URL>http://web.server00.geckon.com/Portal/API/PortalService.svc/File_CreateAndAssociateWithObject</Execute_URL><ExecuteData>sessionID=ea970a1d-a974-4002-9a7a-08b7a299bb3a&amp;filename=\\\\91.197.249.65\\c$\\portal_files\\preview\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4&amp;objectID=1269891&amp;formatID=5</ExecuteData><Rollback_URL>http://web.server00.geckon.com/Portal/API/PortalService.svc/File_CreateAndAssociateWithObject</Rollback_URL><RollbackData>sessionID=ea970a1d-a974-4002-9a7a-08b7a299bb3a&amp;filename=\\\\91.197.249.65\\c$\\portal_files\\preview\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4&amp;objectID=1269891&amp;formatID=5</RollbackData></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.Transcoding.FFmpeg.TranscodeTwoPassh264Plugin\" Status=\"Initialized\" Version=\"1.0.0.0\"><FFmpegFilePath>\\\\91.197.249.65\\c$\\FFmpeg\\ffmpeg.exe</FFmpegFilePath><AudioBitrate>168000</AudioBitrate><VideoWidth>640</VideoWidth><VideoHeight>480</VideoHeight><VideoBitrate>900000</VideoBitrate><SourceFilePath>\\\\91.197.249.65\\c$\\portal_files\\source\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4</SourceFilePath><DestinationFilePath>\\\\91.197.249.65\\c$\\portal_files\\preview\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4</DestinationFilePath><ShouldOwerwriteExistingFile>True</ShouldOwerwriteExistingFile></Plugin></Step><Step><Plugin Class=\"Geckon.Octopus.Plugins.Net.HTTPRequest.HTTPRequestPlugin\" Status=\"Initialized\" Version=\"1.0.0.0\"><Execute_URL>http://web.server00.geckon.com/Portal/API/PortalService.svc/File_CreateAndAssociateWithObject</Execute_URL><ExecuteData>sessionID=ea970a1d-a974-4002-9a7a-08b7a299bb3a&amp;filename=\\\\91.197.249.65\\c$\\portal_files\\preview\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4&amp;objectID=1269891&amp;formatID=7440</ExecuteData><Rollback_URL>http://web.server00.geckon.com/Portal/API/PortalService.svc/File_CreateAndAssociateWithObject</Rollback_URL><RollbackData>sessionID=ea970a1d-a974-4002-9a7a-08b7a299bb3a&amp;filename=\\\\91.197.249.65\\c$\\portal_files\\preview\\AS-1269891_smk_digital_H264.mov_640x360_240kb.mp4&amp;objectID=1269891&amp;formatID=7440</RollbackData></Plugin></Step></Job>");
            IJob job = new Job(jobdata);

            Assert.AreEqual(jobdata.ID, job.ID);
            Assert.AreEqual(jobdata.StatusID, job.StatusID);
            Assert.AreEqual(jobdata.CreatedDate, job.CreatedDate);

            Assert.AreEqual(2, job.Count);

            Assert.AreEqual(5, job[0].Count);
        }

        [Test]
        public void Should_Set_JobCommand_To_Finialize_When_All_Plugins_Are_Committed()
        {
            bool isCommited = false;

            IJob job = new Job( DTO.JobData );
            job.JobCommitted += delegate
                                   {
                                       isCommited = true;
                                   };

            foreach( IPlugin plugin in job.GetAllPlugins(  ) )
            {
                plugin.BeginCommit();
            }

			Timing.WaitUntil(() => isCommited, 2000);

            Assert.AreEqual( JobCommand.Finalize, job.CurrentCommand );
            Assert.IsTrue(isCommited);
        }

        [Test]
        public void Should_Set_JobCommand_To_Finialize_When_All_Plugins_Are_Rolledback()
        {
            bool isRolledback = false;

            IJob job = new Job( DTO.JobData );
            job.JobRolledback += delegate
                                   {
                                       isRolledback = true;
                                   };

            foreach( IPlugin plugin in job.GetAllPlugins( ) )
            {
                plugin.BeginRollback();
            }

			Timing.WaitUntil(() => isRolledback, 2000);

            Assert.AreEqual( JobCommand.Finalize, job.CurrentCommand );
            Assert.IsTrue(isRolledback);
        }

        [Test]
        public void Should_Set_JobCommand_To_Commit_When_All_Plugins_Are_Executed()
        {
            bool isExecuted = false;

            IJob job = new Job( DTO.JobData );
            job.JobExecuted += delegate
                                   {
                                       isExecuted = true;
                                   };

            foreach( IPlugin plugin in job.GetAllPlugins( ) )
            {
                plugin.BeginExecute();
            }

			Timing.WaitUntil(() => isExecuted, 2000);

            Assert.AreEqual( JobCommand.Commit, job.CurrentCommand );
            Assert.IsTrue(isExecuted);
        }

		[Test]
		public void Should_Return_Runable_Plugins_In_Reverse_Order_When_Rolling_Back()
		{
			IJob job = new Job(DTO.JobData);
			var errorPlugin = new TestPlugin(true,false,false);
			var hasFailed = false;

			job[job.Count - 1].Clear();
			job[job.Count - 1].Add(errorPlugin); //Error plugin added to the end

			job.JobExecuteFailed += (sender, eventArgs) => hasFailed = true;

            foreach (IPlugin plugin in job.GetAllPlugins())
			{
				plugin.BeginExecute();
			}

			Timing.WaitUntil(() => hasFailed, 2000);

            IList<IPlugin> plugins = job.GetRunablePlugins().ToList();

			Assert.AreEqual(JobCommand.Rollback, job.CurrentCommand);
            Assert.AreEqual(plugins.First(), errorPlugin);
            Assert.AreEqual(1, plugins.Count);

            foreach (var list in plugins)
		    {
		        list.BeginRollback();
		    }

            IList<IPlugin> plugins2 = job.GetRunablePlugins().ToList();

            Assert.AreEqual(7, plugins2.Count);

		    foreach (IPlugin list in plugins2)
		    {
                list.BeginRollback();
		    }

            IList<IPlugin> plugins3 = job.GetRunablePlugins().ToList();

            Assert.AreEqual(1, plugins3.Count);

            foreach (IPlugin list in plugins3)
            {
                list.BeginRollback();
            }

            IList<IPlugin> plugins4 = job.GetRunablePlugins().ToList();

            Assert.AreEqual(2, plugins4.Count);

			Assert.IsTrue(hasFailed);
		}

        [Test]
        public void Should_Generate_JobXml()
        {
            IJob job = new Job( DTO.JobData );

			job[1].Add(new TestPlugin());

        	var result = new XmlDocument();
			result.LoadXml(job.JobXML.ToString());

			Assert.AreEqual(
                "<Job ID=\"1\" Command=\"Execute\" OperationProgress=\"0\" TotalProgress=\"0\"><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Flow OperationProgress=\"0\" TotalProgress=\"0\"><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin></Step><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin></Step><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin></Step></Flow><Flow OperationProgress=\"0\" TotalProgress=\"0\"><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin></Step></Flow></Step><Step OperationProgress=\"0\" TotalProgress=\"0\"><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>50</ExecuteDuration><CommitDuration>20</CommitDuration><RollbackDuration>30</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin><Plugin Class=\"Geckon.Octopus.Plugins.TestPlugin.TestPlugin\" Status=\"Initialized\" OperationProgress=\"0\" TotalProgress=\"0\" Version=\"1.0.0.0\" RollbackLevel=\"0\"><ExecuteDuration>0</ExecuteDuration><CommitDuration>0</CommitDuration><RollbackDuration>0</RollbackDuration><ShouldExecuteFail>false</ShouldExecuteFail><ShouldCommitFail>false</ShouldCommitFail><ShouldRollbackFail>false</ShouldRollbackFail></Plugin></Step></Job>",
                result.OuterXml
                );
        }
    }
}