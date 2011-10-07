using System.Diagnostics;
using Geckon.Logging;

namespace Geckon.Octopus.Core.Logging
{
    public class Logging
    {
        private static Logging _Instance;

        public static Logging Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Logging();

                return _Instance;
            }
        }

        private Logging()
        {

        }

        public void Log(string message, bool isError)
        {
            lock (_Instance)
            {
                Trace.WriteLine(message);
                Geckon.Logging.Log.Write(message, isError ? LogType.Error : LogType.Information, LogTarget.EventLog, "Application", "DatabaseMail");
            }
        }
    }
}
