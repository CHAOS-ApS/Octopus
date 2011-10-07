using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Geckon.Logging;

namespace Geckon.Octopus.Controller.Core.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Log.Write("Service Started", LogType.Information, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new OctopusControllerService() 
			};
            ServiceBase.Run(ServicesToRun);

            Log.Write("Service Stopped", LogType.Information, LogTarget.Xml, @"C:\Users\Jesper Fyhr Knudsen\Desktop\Octopus\src\app\Geckon.Octopus.Controller.Core.WindowsService\bin\Debug\OctopusLog.xml", "Octopus");
        }
    }
}
