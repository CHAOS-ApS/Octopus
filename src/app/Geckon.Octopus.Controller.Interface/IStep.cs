using System.Collections.Generic;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Controller.Interface
{
    public interface IStep : IPluginContainer, IList<IStepContent>
    {
		
    }
}