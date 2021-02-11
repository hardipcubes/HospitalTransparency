using HospitalTransparency.App_Start;
using HospitalTransparency.Models;
using HospitalTransparency.Models.Model;
using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class MenuController : SessionController
    {
        MenuRepository menuRepo = new MenuRepository();

        public ActionResult Menu(int menuId = 0)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                ParentList(null);
                IconList(null);
                if (menuId == 0)
                {
                    ViewBag.FormType = "";
                    ViewBag.Iconname = null;
                }
                else
                {
                    ViewBag.FormType = "Edit";
                    MenuModel menuData = menuRepo.GetMenuDataById(menuId);
                    ViewBag.Iconname = menuData.Iconname;
                    Session["Menuiconid"] = menuData.MenuIcon;
                    return PartialView("P_Main", menuData);
                }
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        #region Menu Post Data
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Menu(MenuModel menuData)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Session["Menuiconid"])))
                {
                    menuData.MenuIcon = Convert.ToInt32(Session["Menuiconid"]);
                }

                bool flag = menuRepo.SaveMenu(menuData);
                if (flag == false)
                {
                    ViewBag.ErrorMessage = "Please try again.";
                    CustomCacheManagement.MenuList = new RightsListRepository().GetMenuList();
                }
                else
                {
                    ViewBag.SuccessMessage = "Menu saved successfully.";
                }
                ViewBag.FormType = "";
            }
            return PartialView("P_List");
        }
        #endregion

        #region Delete Menu
        public ActionResult DeleteMenu(int menuId)
        {
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                bool flag = false;
                flag = menuRepo.DeleteMenu(menuId);
                if (flag == false)
                {
                    ViewBag.ErrorMessage = "Please try again. :(";
                }
                else
                {
                    ViewBag.SuccessMessage = "Menu deleted successfully. :)";

                }
                ViewBag.FormType = "";
            }
            return PartialView("P_List");
        }

        public ActionResult DeleteAll(string menuIds)
        {
            var data = menuRepo.DeleteAllMenu(menuIds.TrimEnd(','));
            if (data == true)
            {
                ViewBag.SuccessMessage = "All selected menus deleted successfully. :)";
            }
            else
            {
                ViewBag.ErrorMessage = "Please try again. :(";
            }
            return PartialView("P_List");
        }
        #endregion

        #region SetActiveDeactive
        public ActionResult SetActiveDeactive(int menuId)
        {
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                ViewBag.SuccessMessage = "Menu " + menuRepo.SetActiveDeactive(menuId) + " successfully.";
                CustomCacheManagement.MenuList = new RightsListRepository().GetMenuList();
            }
            return PartialView("P_List");
        }
        #endregion

        #region Custome Paging
        public JsonResult fnPaging()
        {
            string pageno = Request.Params["sEcho"] ?? "1";
            string MenuName = Request.Params["MenuName"];
            string ControllerName = Request.Params["ControllerName"];
            string ActionName = Request.Params["ActionName"];
            string OrderBy = Request.Params["OrderBy"];
            string searchpara = Request.Params["sSearch"];
            string sortablecol = Request.Params["iSortCol_0"] != null ? Request.Params["iSortCol_0"].ToString() : null;
            string sortorder = Request.Params["sSortDir_0"] != null ? Request.Params["sSortDir_0"].ToString() : null;
            string istart = Request.Params["iDisplayStart"];
            string iDisplayStartstr = Request.Params["iDisplayStart"];
            int iDisplayStart = Convert.ToInt16(iDisplayStartstr);
            string iDisplayLengthstr = Request.Params["iDisplayLength"];
            int iDisplayLength = Convert.ToInt16(iDisplayLengthstr);

            int page = Convert.ToInt16(pageno);
            var model = menuRepo.GetMenuList(page, MenuName, ControllerName, ActionName, OrderBy, searchpara, sortablecol, sortorder).ToList();

            var _Data = model.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            string[][] strarr = new string[_Data.Count()][];
            for (var i = 0; i < _Data.Count(); i++)
            {
                string[] subaray = new string[7];
                subaray[0] = "<input type='checkbox' id='chk_" + _Data[i].MenuId + "' class='checkbox'/> <label for='chk_" + _Data[i].MenuId + "'></label>";
                subaray[1] = _Data[i].MenusName;
                subaray[2] = _Data[i].ControllerName;
                subaray[3] = _Data[i].ActionName;
                subaray[4] = _Data[i].OrderBy.ToString();

                if (_Data[i].IsActive == true)
                {
                    subaray[5] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].MenuId + "','Y')>Yes</a>";
                }
                else
                {
                    subaray[5] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].MenuId + "','N')>No</a>";
                }

                subaray[6] = "<a class='btn btn-round ink-reaction btn-primary fa fa-edit' data-ajax-complete='scrolltop();' data-ajax='true' data-ajax-method='GET' data-ajax-mode='replace' data-ajax-update='#P_Main' href='/Menu/Menu?menuId=" + _Data[i].MenuId + "' title='Edit'></a>" + "   " + "<a href='javascript:void(0);' class='btn btn-round btn-danger ink-reaction fa fa-trash-o' title='Delete' onclick=DeleteMenu('" + _Data[i].MenuId + "')></a>";

                strarr[i] = subaray;
            }

            return Json(new { sEcho = Request.Params["sEcho"], recordsTotal = model.Count().ToString(), recordsFiltered = model.Count().ToString(), iTotalRecords = model.Count().ToString(), iTotalDisplayRecords = model.Count().ToString(), aaData = strarr }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region ParentList
        public string ParentList(string parent)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                var parentList = (from rm in db.MenuMasters
                                  where/* rm.ParentId == null &&*/ rm.IsDeleted == false && rm.IsActive == true
                                  select new { rm.MenuId, rm.MenuName }).OrderBy(m=>m.MenuName).ToList();
                ViewBag.ParentMenuList = !string.IsNullOrEmpty(parent) ? new SelectList(parentList, "MenuId", "MenuName", parent) : new SelectList(parentList, "MenuId", "MenuName");
            }
            return "Yes";
        }
        #endregion

        #region IconList
        public string IconList(string icon)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                var iconList = (from rm in db.MenuIcons
                                orderby rm.DisplayHTML
                                select new { IconClass = rm.IconClass.Replace("glyphicon glyphicon-", ""), rm.Id }).ToList();
                ViewBag.IconList = !string.IsNullOrEmpty(icon) ? new SelectList(iconList, "Id", "IconClass", icon) : new SelectList(iconList, "Id", "IconClass");
            }
            return "Yes";
        }
        #endregion

        public ActionResult MenuIconList()
        {
            var model = new MenuModel();
            using (var db = new HospitalTransparencyEntities())
            {
                var iconList = (from rm in db.MenuIcons
                                orderby rm.DisplayHTML
                                select new { IconClass = rm.IconClass, rm.Id }).ToList();
                model.MenuList = (from i in iconList
                                  select new SelectListItem()
                                  {
                                      Text = i.IconClass,
                                      Value = Convert.ToString(i.Id)
                                  }).ToList();
            }
            return PartialView("MenuIcons", model);
        }

        [HttpGet]
        public ActionResult ClickMenuIcons(string Menuiconid)
        {
            Session["Menuiconid"] = string.Empty;
            Session["Menuiconid"] = Menuiconid;
            return Json(new { data = Menuiconid }, JsonRequestBehavior.AllowGet);
        }
    }
}