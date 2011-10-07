using System.Collections.Generic;
using Geckon.Octopus.Data;

namespace Geckon.Octopus.Controller.Web.ViewModels
{
    public class JobIndexViewModel
    {
        public IList<Job>                    CurrentlyRunningJobs    = new List<Job>();
        public IList<JobStatisticsAggregate> JobStatisticsAggregates = new List<JobStatisticsAggregate>();
    }

    public class JobStatisticsAggregate
    {
        public string StatusType { get; set; }
        public int    Count      { get; set; }
        public double Percent    { get; set; }
    }
}