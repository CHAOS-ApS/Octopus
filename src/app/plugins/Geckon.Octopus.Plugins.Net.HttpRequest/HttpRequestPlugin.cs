using System;
using System.Net;
using System.Text;
using System.Web;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Net.HttpRequest
{
    public class HttpRequestPlugin : APluginExtended
    {
        #region Fields

        private string _ExecuteData;
        private string _RollbackData;

        #endregion
        #region Properties

        [Element("Execute_URL")]
        public string Execute_URL
        {
            get; set;
        }

        [Element("ExecuteData")]
        public string ExecuteData
        {
            get
            {
                if( _ExecuteData == null )
                    return "";

                return _ExecuteData;
            }
            set
            {
                _ExecuteData = URLEncodeQueryString(value.Replace("&amp;", "&"));
                    
            }
        }

        public Uri Execute_Address
        {
            get
            {
                return new Uri( new Uri( Execute_URL ), ExecuteData.IndexOf( "?" ) == -1 && Execute_URL.IndexOf( "?" ) == -1 ? "?" + ExecuteData : ExecuteData );
            }
        }

        [Element("Rollback_URL")]
        public string Rollback_URL
        {
            get;
            set;
        }

        [Element("RollbackData")]
        public string RollbackData
        {
            get
            {
                if( _RollbackData == null)
                    return "";

                return _RollbackData;
            }
            set
            {
                _RollbackData = URLEncodeQueryString(value.Replace("&amp;", "&"));
            }
        }

        private string URLEncodeQueryString( string querystring )
        {
            StringBuilder outString = new StringBuilder();
            string[] pairs = querystring.Split( '&' );

            foreach( string pair in pairs )
            {
                outString.Append( string.Format( "{0}={1}&", pair.Substring( 0 , pair.IndexOf( "=" ) ), HttpUtility.UrlEncode( pair.Substring( pair.IndexOf( "=" ) + 1 ) ) ) );
            }

            return outString.ToString();
        }

        public Uri Rollback_Address
        {
            get
            {
                return new Uri( new Uri(Rollback_URL), RollbackData.IndexOf("?") == -1 && Rollback_URL.IndexOf("?") == -1 ? "?" + RollbackData : RollbackData );
            }
        }

        #endregion
        #region Business Logic

        protected override void Execute()
        {
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(Execute_Address);

            request.ContentType = "text/xml";
            request.Method      = "GET";
            using( request.GetResponse().GetResponseStream() )
            {
                
            }
        }

        protected override void Commit()
        {
            base.Commit();
        }

        protected override void Rollback()
        {
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(Rollback_Address);

            request.ContentType = "text/xml";
            request.Method      = "GET";
            using( request.GetResponse().GetResponseStream() )
            {
                
            }
        }

        #endregion
    }
}
