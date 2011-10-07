using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Geckon.Octopus.Controller.Web.ViewModels;
using Geckon.Octopus.Data;

namespace Geckon.Octopus.Controller.Web.Controllers
{
    public class JobController : System.Web.Mvc.Controller
    {
        //
        // GET: /Job/

        public ActionResult Index()
        {
            using( DatabaseDataContext db = new DatabaseDataContext() )
            {
                JobIndexViewModel jobIndex = new JobIndexViewModel();

                foreach( Job job in from jobsTbl in db.Jobs select jobsTbl )
                {
                    string totalProgress = XDocument.Parse( job.JobXML.ToString()).Root.Attribute("TotalProgress").Value;

                    if( totalProgress == "1" || totalProgress == "0" )
                        continue;

                    jobIndex.CurrentlyRunningJobs.Add( job );
                }

                int totalCount = db.Jobs.Count();

                foreach( var job in from jobsTbl in db.Jobs group jobsTbl by jobsTbl.Status into g select new { StatusType = g.Key, Count = g.Count() })
                {
                    jobIndex.JobStatisticsAggregates.Add(new JobStatisticsAggregate { StatusType = job.StatusType.Type, Count = job.Count, Percent = (double)job.Count / totalCount });
                }

                return View( jobIndex );
            }
        }

    }
}
