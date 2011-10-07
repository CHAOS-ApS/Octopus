namespace Geckon.Octopus.Agent.Interface
{
    public interface IAgentEngine : IAgent
    {
        /// <summary>
        /// The IPluginManager used by this IAgentEngine
        /// </summary>
        IPluginManager PluginManager{ get; }

        /// <summary>
        /// The IExecutionManager used by this IAgentEngine
        /// </summary>
        IExecutionManager ExecutionManager { get; }

        /// <summary>
        /// Retruns the number of IPlugin currently in the buffer
        /// </summary>
        int CountPlugins { get; }
    }
}