using HospitalTransparency.Models;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class HomeController : SessionController
    {
        readonly HospitalTransparencyEntities db = new HospitalTransparencyEntities();
        
        public ActionResult Index()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                ViewBag.Checked = false;

                #region Check application found for logged in user
                ApplicationParamModel param = new ApplicationParamModel();
                param.UserName = Convert.ToString(Session["UserName"]);
                var modelData = CallAPI<APIResponseBase<List<ApplicationResultModel>>, ApplicationParamModel>($"/api/Application/Get", "POST", param).Result;
                if (modelData != null && modelData.StatusCode == 200 && modelData.Result != null)
                {
                    int appCount = modelData.Result.Where(m => m.ApplicationCode != "hldshare").Count();
                    Session["AppCount"] = appCount;
                }
                else
                {
                    Session["AppCount"] = null;
                }
                #endregion

                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult AppListBaseOnRights()
        {
            try
            {
                ApplicationParamModel param = new ApplicationParamModel();
                param.UserName = Convert.ToString(Session["UserName"]);
                var model = CallAPI<APIResponseBase<List<ApplicationResultModel>>, ApplicationParamModel>($"/api/Application/Get", "POST", param).Result;
                if (model != null && model.StatusCode == 200 && model.Result != null)
                    return PartialView("_AppListBaseOnRights", model.Result.Where(m => m.ApplicationCode != "hldshare").ToList());
                return PartialView("_AppListBaseOnRights", null);
            }
            catch (Exception)
            {
                return PartialView("_AppListBaseOnRights", null);
            }

        }

        public ActionResult Form()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Table()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                return View();
            }
            return RedirectToAction("Login", "Login");
        }
    }
}