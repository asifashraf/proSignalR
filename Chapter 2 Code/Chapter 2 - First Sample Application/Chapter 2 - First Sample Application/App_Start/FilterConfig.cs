using System.Web;
using System.Web.Mvc;

namespace Chapter_2___First_Sample_Application
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
