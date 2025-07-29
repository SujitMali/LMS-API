using System.Web;
using System.Web.Mvc;

namespace Lib_Mang_Sys_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
