using System.Web.Mvc;

namespace Geckon.Octopus.Controller.Web.Controllers
{
    public class HomeController : System.Web.Mvc.Controller
    {
        //
        // GET: /Home/
        public string Index()
        {
            return string.Format( "Octopus version {0} ", GetType().Assembly.GetName().Version );
        }

    }
}
