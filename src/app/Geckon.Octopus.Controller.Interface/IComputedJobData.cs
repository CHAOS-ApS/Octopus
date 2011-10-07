using Geckon.Serialization.Xml;
namespace Geckon.Octopus.Controller.Interface
{
    public interface IComputedJobData : IJobData
    {
        [Element]
        double? TotalProgress { get; }

        [Element]
        double? OperationProgress { get; }
    }
}