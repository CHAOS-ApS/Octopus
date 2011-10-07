using System;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IControllerEngine : IDisposable
    {
        IJobManager JobManager { get; }
        IBroker Broker { get; }

		bool ShouldRunPlugins { get; set; }
    }
}