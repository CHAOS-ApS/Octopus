namespace Geckon.Octopus.Controller.Interface
{
	public enum JobCommand
	{
		Execute   = 0,
		Rollback  = 1,
		Commit    = 2,
		Finalize  = 3,
		None      = 4
	}
}