using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Geckon.Octopus.Controller.Core;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugins.Transcoding.FFmpeg;
using Job = Geckon.Octopus.Controller.Core.Job;

namespace Geckon.Octopus.UI.Console
{
    class Program
    {
        static DirectoryInfo source;
        static DirectoryInfo destination;

        private static string Filter;

        static void Main( string[] args )
        {
            if( args.Length != 3 )
            {
                System.Console.WriteLine( "[sourcePath] [searchFilter] [destinationPath]" );
                return;
            }
            
            source      = new DirectoryInfo( args[0] );
            Filter      = args[1];
            destination = new DirectoryInfo( args[2] );

            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                db.Test_InsertDemoData("\\Lib");
           
                foreach( FileInfo file in GetFiles( source ) )
                {
                    IJob  job   = new Job( );
                    IStep step1 = new Step();

					CutVideoFramePlugin still = new CutVideoFramePlugin();
                    TranscodeTwoPassh264Plugin transcode = new TranscodeTwoPassh264Plugin();

                    still.SourceFilePath      = file.FullName;
                    still.DestinationFilePath = Path.Combine( destination.FullName, Path.Combine(file.Name, ".png") );

                    transcode.SourceFilePath      = file.FullName;
                    transcode.DestinationFilePath = Path.Combine(destination.FullName, file.Name);
                    transcode.VideoBitrate        = ( 1024 - 128 ) * 1024;
                    transcode.AudioBitrate        = 128 * 1024;

                    step1.Add(still);
                    step1.Add(transcode);

                    job.Add(step1);

                    System.Console.WriteLine( file.FullName + ", added");

                    db.Job_Insert(job.StatusID, job.JobXML.ToString());
                }
            }
        }

        private static IEnumerable<FileInfo> GetFiles( DirectoryInfo directory )
        {
            foreach( DirectoryInfo subDir in directory.GetDirectories())
                foreach( FileInfo file in GetFiles( subDir ) )
                    yield return file;

            foreach( FileInfo file in directory.GetFiles( Filter ) )
                yield return file;
        }
    }
}
