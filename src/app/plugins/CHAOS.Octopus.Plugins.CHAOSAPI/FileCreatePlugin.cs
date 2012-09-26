using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Geckon.Octopus.Plugin.Core;
using Geckon.Serialization.Xml;

namespace CHAOS.Octopus.Plugins.CHAOSAPI
{
    public class FileCreatePlugin : APlugin
    {
        #region Properties

        [Element]
        public string BaseURL { get; set; }
        [Element]
        public string SessionGUID { get; set; }
        [Element]
        public string ObjectGUID { get; set; }
        [Element]
        public uint? ParentFileID { get; set; }
        [Element]
        public uint FormatID { get; set; }
        [Element]
        public uint DestinationID { get; set; }
        [Element]
        public string FileName { get; set; }
        [Element]
        public string OriginalFileName { get; set; }
        [Element]
        public string FolderPath { get; set; }

        [Element]
        public uint? FileID { get; set; }

        #endregion
        #region Business Logic

        protected override void Execute()
        {
            base.Execute();

            var url = string.Format("{0}/File/Create?sessionGUID={1}&objectGUID={2}&parentFileID={3}&formatID={4}&destinationID={5}&filename={6}&originalFilename={7}&folderPath={8}",
                                    BaseURL, SessionGUID, ObjectGUID, ParentFileID.HasValue ? ParentFileID.ToString():"", FormatID, DestinationID, FileName, OriginalFileName, FolderPath);

            var response = SendRequest(url);

            FileID = uint.Parse(response.Root.Descendants("ID").First().Value);

            RollbackLevel = uint.MaxValue;
        }

        protected override void Rollback()
        {
            base.Rollback();

            if( RollbackLevel != uint.MaxValue )
                return;

            var url = string.Format("{0}/File/Delete?sessionGUID={1}&ID={2}", BaseURL, SessionGUID, FileID);

            SendRequest(url);
        }

        private XDocument SendRequest(string url)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);

            request.ContentType = "text/xml";
            request.Method      = "GET";

            using (var stream     = request.GetResponse().GetResponseStream())
            using (var textReader = new StreamReader(stream,System.Text.Encoding.Unicode))
            {
                var response = textReader.ReadToEnd();

                return XDocument.Parse(response);
            }
        }

        #endregion
    }
}
