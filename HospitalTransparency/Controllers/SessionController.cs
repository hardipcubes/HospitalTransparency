using HospitalTransparency.Models;
using HospitalTransparency.Models.Repository;
using HospitalTransparency.DataAccess;
using HospitalTransparency.Models.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class SessionController : Controller
    {
        HospitalTransparencyEntities db = new HospitalTransparencyEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (Session["UserId"] != null)
            {
                if (Session["RoleId"] != null)
                {

                    ViewBag.UserId = Session["UserId"].ToString();
                    ViewBag.Role = Session["Role"].ToString();
                    ViewBag.Name = Session["Name"].ToString();

                    using (var dbnwe = new RightsListRepository())
                    {
                        Session["MenuRightsList"] = dbnwe.GetRightsList();

                        ViewBag.MenuRightsList = Session["MenuRightsList"];
                        ViewBag.RightAdd = Session["RightAdd"] != null ? Session["RightAdd"].ToString() : null;
                        ViewBag.RightEdit = Session["RightEdit"] != null ? Session["RightEdit"].ToString() : null;
                        ViewBag.RightDelete = Session["RightDelete"] != null ? Session["RightDelete"].ToString() : null;
                        ViewBag.RightDisplay = Session["RightDisplay"] != null ? Session["RightDisplay"].ToString() : null;

                        ViewBag.MenuId = Session["MenuId"] != null ? Session["MenuId"].ToString() : null;
                        ViewBag.ParentMenuId = Session["ParentMenuId"] != null ? Session["ParentMenuId"].ToString() : null;
                    }
                }
            }
            else
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                if (actionName.ToLower() != "login" && controllerName.ToLower() != "login")
                    filterContext.Result = RedirectToAction("Login", "Login");
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            ErrorDAL DAL = new ErrorDAL();
            filterContext.ExceptionHandled = true;

            var ex = filterContext.Exception; // TPDO : Log Exception

            var requestUrl = Request.Url.ToString();

            DAL.InsertErrorLog(requestUrl, ex.Message, ex.StackTrace);

            #region Use full Code


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = ex.Message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                if (ex is HttpException)
                {
                    if (((HttpException)ex).GetHttpCode() == 404)
                    {
                        //Redirect to 404 page:
                        filterContext.Result = RedirectToAction("Error404", "Error");
                    }
                }
                else
                {
                    string excMessageSafe = Regex.Replace(ex.ToString(),
                    "(\\<+[a-z!/\\?])|(&\\#)", new MatchEvaluator((m) =>
                    {
                        return m.Value.Replace("<", string.Empty).Replace("&#", string.Empty);
                    }), RegexOptions.IgnoreCase);
                    // Redirect on error:
                    //var ErrorObject = System.Web.Configuration.WebConfigurationManager.AppSettings["show_detailed_errors"] == "true" ? new { ErrorTitle = ex.GetType().Name, ErrorMessage = excMessageSafe.Substring(0, 1400) } : null;

                    filterContext.Result = RedirectToAction("Error500", "Error");
                }
            }

            #endregion
        }

        public APIResponseBase<T> CallAPI<T, I>(string url, string Method = "POST", I param = default(I), bool returnActualResponse = false)
        {
            try
            {
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("token", "b2af0a6df1df43ddb68bc3dffc3f510c");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (Method == "POST")
                    response = Client.PostAsJsonAsync(getAPIPath(url), param).Result;
                else
                    response = Client.GetAsync(getAPIPath(url)).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK || returnActualResponse)
                {
                    var result = response.Content.ReadAsAsync<T>().Result;
                    return new APIResponseBase<T>()
                    {
                        IsSuccess = true,
                        Message = string.Empty,
                        Result = result,
                        StatusCode = (int)response.StatusCode
                    };
                }
                else
                {
                    return new APIResponseBase<T>()
                    {
                        IsSuccess = false,
                        Message = getErrorMessage(url, response),
                        Result = default(T),
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new APIResponseBase<T>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = default(T),
                    StatusCode = 500
                };
            }
        }

        private string getAPIPath(string partialURL)
        {
            string RootAPI = System.Web.Configuration.WebConfigurationManager.AppSettings["api"];
            return RootAPI + partialURL;
        }


        private string getErrorMessage(string url, HttpResponseMessage response)
        {
            return $"Request Url : {url} Status code : {response.StatusCode} Error : {response.ReasonPhrase}";
        }
    }
}