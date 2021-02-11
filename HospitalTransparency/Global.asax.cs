using HospitalTransparency.DataAccess;
using HospitalTransparency.App_Start;
using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HospitalTransparency
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CustomCacheManagement.MenuList = new RightsListRepository().GetMenuList();
        }

        void Application_Error(object sender, EventArgs e)
        {
            ErrorDAL DAL = new ErrorDAL();
            Exception ex = Server.GetLastError().GetBaseException();
            var requestUrl = Request.Url.ToString();
            DAL.InsertErrorLog(requestUrl, ex.Message, ex.StackTrace);

            if (ex is HttpException)
            {
                if (((HttpException)ex).GetHttpCode() == 404)
                {
                    Response.Redirect("/Error/Error404", true);
                }
            }
            else
            {
                string excMessageSafe = Regex.Replace(ex.ToString(),
                "(\\<+[a-z!/\\?])|(&\\#)", new MatchEvaluator((m) =>
                {
                    return m.Value.Replace("<", string.Empty).Replace("&#", string.Empty);
                }), RegexOptions.IgnoreCase);
                //string errorDetailsQueryString = System.Web.Configuration.WebConfigurationManager.AppSettings["show_detailed_errors"] == "true" ? "?ErrorTitle=" + HttpUtility.UrlPathEncode(exc.GetType().Name) + "ErrorMessage" + HttpUtility.UrlPathEncode(excMessageSafe.Substring(0, 1200)) : "";
                Response.Redirect("/Error/Error500", true);
            }

        }
    }
}
