using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	internal static class CarbonQuery
	{
		private const int CARBON_QUERY_TIMEOUT = 10000;
		
		public static string GetCarbonJobStatusXml(string host, ushort port, string carbonJobId)
		{
			var query = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><cnpsXML CarbonAPIVer=\"1.2\" TaskType=\"JobCommand\"><JobCommand Command=\"QueryInfo\" GUID=\"" + carbonJobId + "\"/></cnpsXML>";

			return Query(host, port, query);
		}

		public static string Query(string host, ushort port, string xml)
		{
			// Connect
			TcpClient tcpSocket = null;
			NetworkStream stream = null;
			//
			try
			{
				tcpSocket = new TcpClient(host, port);
				stream = tcpSocket.GetStream();

				var xmlQuery = new XmlDocument();

				xmlQuery.LoadXml(xml);

				//Create query

				var utf8 = new UTF8Encoding();

				var queryContent = utf8.GetBytes(xmlQuery.DocumentElement.OuterXml);

				var queryHeader = utf8.GetBytes("CarbonAPIXML1 " + queryContent.Length + " ");

				var query = new byte[queryHeader.Length + queryContent.Length];

				Array.Copy(queryHeader, query, queryHeader.Length);
				Array.Copy(queryContent, 0, query, queryHeader.Length, queryContent.Length);

				// Send data
				stream.Write(query, 0, query.Length);

				// Get reply packet
				var lastDataReadTime = DateTime.Now;
				var readLength = 0;
				var reply = new Byte[1];
				var str = String.Empty;

				string apiVersion = null;
				var dataLength = 0;
				var bytesRead = 0;

				while (true) //Exited by break
				{
					if(!stream.DataAvailable)
					{
						if (lastDataReadTime.AddMilliseconds(CARBON_QUERY_TIMEOUT).CompareTo(DateTime.Now) < 0)
							throw new TimeoutException("Carbon query timed out");
						
						Thread.Sleep(100); 
						continue;
					}
					lastDataReadTime = DateTime.Now;

					readLength = stream.Read(reply, bytesRead, reply.Length - bytesRead);

					if (dataLength == 0)
					{
						str += Encoding.UTF8.GetString(reply);

						if (str.IndexOf(" ") == -1)
							continue;

						if (apiVersion == null)
						{
							apiVersion = str.Substring(0, str.Length - 1);
							str = String.Empty;
							continue;
						}

						dataLength = int.Parse(str.Substring(0, str.Length - 1));

						if (dataLength == 0)
							return String.Empty;

						reply = new byte[dataLength];

						continue;
					}

					bytesRead += readLength;

					if(bytesRead == dataLength)
						break;
				}

				return Encoding.UTF8.GetString(reply, 0, reply.Length);
			}
			finally
			{
				if (stream != null)
					stream.Close();
				if (tcpSocket != null)
					tcpSocket.Close();
			}
		}
	}
}