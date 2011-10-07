using Geckon.Octopus.Controller.Interface;
using System.Xml.Linq;
using System.Xml.XPath;
using System;
using Geckon.Reflection;

namespace Geckon.Octopus.Data
{
    public partial class Job : IComputedJobData
    {
        #region Helper methods

        private static TValue GetRootAttributeValue<TValue>(XElement element, string attributeName)
        {
            if (element == null)
                return default(TValue);

            XAttribute attribute = element.Attribute(attributeName);

            if (attribute == null || String.IsNullOrEmpty(attribute.Value.Replace(",", ".")))
                return default(TValue);

            return ConversionExpert.ChangeType<TValue>(attribute.Value.Replace(",","."));
        }
        
        #endregion
        #region Computed properties

        public double? OperationProgress
        {
            get
            {
                return GetRootAttributeValue<double?>(this.JobXML, "OperationProgress");
            }
        }

        public double? TotalProgress
        {
            get
            {
                return GetRootAttributeValue<double?>(this.JobXML, "TotalProgress");
            }
        }

        #endregion
    }
}
