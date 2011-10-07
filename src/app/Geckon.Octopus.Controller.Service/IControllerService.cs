using System.Runtime.Serialization;
using System.ServiceModel;
using Geckon.Octopus.Controller.Interface;
using System.Collections.Generic;

namespace Geckon.Octopus.Controller.Service
{
    [ServiceContract]
    public interface IControllerService
    {
        [OperationContract]
        string GetSession( int clientID );

        [OperationContract]
        IJobData AddJob(string jobXml);

        [OperationContract]
        IJobData GetJobByID(int id);

        [OperationContract]
        IEnumerable<IJobData> GetJobs();

        [OperationContract]
        IEnumerable<IJobData> GetUnfinishedJobs();

        [OperationContract]
        IJobData GetJobByStatus(int status);

    }
}