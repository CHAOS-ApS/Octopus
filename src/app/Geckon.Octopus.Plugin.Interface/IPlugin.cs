using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugin.Interface
{
    public interface IPlugin : IStepContent
    {
        [Attribute("Version")]
        string Version { get; }

		[Attribute("UniqueIdentifier")]
        string UniqueIdentifier { get; }
        string PluginIdentifier { get; }

		bool IsRunning { get; }
		bool IsReadyForOperation { get; }

		bool IsOperationRightAvailable { get; }
    	bool GetOperationRight();
    	void ReleaseOperationRight();

		/// <summary>
		/// An Enum representing the status of the Transaction
		/// </summary>
		[Attribute("Status")]
		PluginStatus Status { get; }

		/// <summary>
		/// Preform the actual plugin logic.
		/// </summary>
		void BeginExecute();

		/// <summary>
		/// Finalizes the plugin operation by commiting the result.
		/// </summary>
		void BeginCommit();

		/// <summary>
		/// This rolls back the performed task(s).
		/// </summary>
		void BeginRollback();
    }
}