using Geckon.Events;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugin.Interface
{
	public interface IPluginTrackable
	{
		event EventHandlers.ObjectEventHandler<IPlugin>			ExecuteCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin>	ExecuteFailed;
		event EventHandlers.ObjectEventHandler<IPlugin>			CommitCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin>	CommitFailed;
		event EventHandlers.ObjectEventHandler<IPlugin>			RollbackCompleted;
		event EventHandlers.ObjectErrorEventHandler<IPlugin>	RollbackFailed;

		event EventHandlers.ObjectProgressEventHandler<IPlugin>	OperationProgressChanged;
		event EventHandlers.ObjectProgressEventHandler<IPlugin> TotalProgressChanged;
		
		/// <summary>
		/// This returns a double value between 0.0 and 1.0 indicating the current plugin operation progress
		/// </summary>
		[Attribute("OperationProgress")]
		double OperationProgress { get; }

		/// <summary>
		/// This returns a double value between 0.0 and 1.0 indicating the total plugin progress
		/// </summary>
		[Attribute("TotalProgress")]
		double TotalProgress { get; }
	}
}