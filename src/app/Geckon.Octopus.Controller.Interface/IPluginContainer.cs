using System.Collections.Generic;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IPluginContainer : IPluginTrackable
    {
		event EventHandlers.ObjectEventHandler<IPlugin>			PluginAdded;
		event EventHandlers.ObjectEventHandler<IPlugin>			PluginRemoved;

		bool HasRunningPlugins { get; }

        IEnumerable<IPlugin> GetRunablePlugins(JobCommand command);
        IEnumerable<IPlugin> GetAllPlugins();
    }
}