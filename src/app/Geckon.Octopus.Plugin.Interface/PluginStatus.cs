namespace Geckon.Octopus.Plugin.Interface
{
    public enum PluginStatus
    {
        Initializing			= 1,
        Initialized				= 2,
        Executing				= 101,
        Executed				= 102,
		ExecuteFailed			= 103,
        Committing				= 201,
        Committed				= 202,
		CommitFailed			= 203,
        Rollingback				= 301,
        Rolledback				= 302,
		RollbackFailed			= 303
    }
}
