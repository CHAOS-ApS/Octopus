namespace Geckon.Octopus.Controller.Core
{
    public enum JobCommands
    {
        Execute = 0,
        Rollback = 1,
        Commit = 2,
        Executing = 32,
        Rollingback = 64,
        Committing   = 128,
        Wait = 2147483647
    }
}