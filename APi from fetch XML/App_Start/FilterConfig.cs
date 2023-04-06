using System.Web;
using System.Web.Mvc;

namespace APi_from_fetch_XML
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
