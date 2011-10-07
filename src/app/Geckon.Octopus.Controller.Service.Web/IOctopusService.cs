using System.IO;
using System.ServiceModel;

namespace Geckon.Octopus.Controller.Service.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOctopusService" in both code and config file together.
    [ServiceContract]
    public interface IOctopusService
    {
        [OperationContract]
        Stream Job_Get(int? id);

        [OperationContract]
        Stream Job_GetUnfinished();

        [OperationContract]
        Stream Job_Insert();
    }
}
