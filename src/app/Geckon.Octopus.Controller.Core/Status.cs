namespace Geckon.Octopus.Controller.Core
{
    enum Status
    {
        New = 0,
        Pending = 250,
        JobLoaded = 500,
        JobQueued = 750,
        TestNew = 999,
        TestDone = 1000,
        ExecuteComplete = 2000,
        ExecuteFailed = 3000,
        CommitComplete = 4000,
        CommitFailed = 5000,
        RollbackComplete = 6000,
        RollbackFailed = 7000
    }
}
