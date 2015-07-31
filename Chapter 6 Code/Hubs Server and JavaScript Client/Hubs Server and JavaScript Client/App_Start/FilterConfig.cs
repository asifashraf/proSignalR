using System.Web;
using System.Web.Mvc;

namespace Hubs_Server_and_JavaScript_Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
