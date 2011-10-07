using Geckon.Serialization.Xml;
using System.Xml.Linq;

namespace Geckon.Octopus.Controller.Interface
{
    [Document("Geckon.Octopus.Controller.Interface.IJobData")]
    public interface IJobData
    {
        [Attribute("ID")]
        int ID { get; set; }

        [Attribute("StatusID")]
        int StatusID { get; set; }

        [Element(true)]
        XElement JobXML { get; set; }

        [Element]
        System.DateTime CreatedDate { get; set; }

        [Element]
        System.DateTime LastUpdated { get; set; }


    }
}