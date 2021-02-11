using HospitalTransparency.Models.Model;
using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class RoleController : SessionController
    {
        RoleRepository roleRepo = new RoleRepository();

        public ActionResult Role(int? RoleId)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                if (string.IsNullOrEmpty(RoleId.ToString()))
                {
                    ViewBag.FormType = "";
                }
                else
                {
                    ViewBag.FormType = "Edit";
                    RoleModel RoleData = roleRepo.GetRoleById(Convert.ToInt32(RoleId));
                    return PartialView("P_Main", RoleData);
                }
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        #region Role Post Data
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Role(RoleModel RoleData)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                bool flag = roleRepo.SaveRole(RoleData);
                if (flag == false)
                {
                    ViewBag.ErrorMessage = "Please try again";
                }
                else
                {
                    ViewBag.SuccessMessage = "Record saved successfully.";
                }
                ViewBag.FormType = "";
            }
            return PartialView("P_List");
        }
        #endregion

        #region Delete Role
        public ActionResult DeleteRole(int? RoleId)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                bool flag = false;
                flag = roleRepo.DeleteRole(Convert.ToInt32(RoleId));
                if (flag == false)
                {
                    ViewBag.ErrorMessage = "Please try again";
                }
                else
                {
                    ViewBag.SuccessMessage = "Record deleted successfully.";

                }
                ViewBag.FormType = "";
            }
            return PartialView("P_List");
        }

        public ActionResult DeleteAll(string roleIds)
        {
            var data = roleRepo.DeleteAllRole(roleIds.TrimEnd(','));
            if (data == true)
            {
                ViewBag.SuccessMessage = "All selected users deleted successfully. :)";
            }
            else
            {
                ViewBag.ErrorMessage = "Please try again. :(";
            }
            return PartialView("P_List");
        }
        #endregion

        #region SetActiveDeactive
        public ActionResult SetActiveDeactive(int? RoleId)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                ViewBag.SuccessMessage = roleRepo.SetActiveDeactive(Convert.ToInt32(RoleId));
            }
            return PartialView("P_List");
        }
        #endregion

        #region Custome Paging
        public JsonResult fnPaging()
        {
            string pageno = Request.Params["sEcho"].ToString() ?? "1";
            string Name = Request.Params["RoleName"].ToString();
            string Description = Request.Params["RoleDesc"].ToString();
            string searchpara = Request.Params["sSearch"].ToString();
            string sortablecol = Request.Params["iSortCol_0"] != null ? Request.Params["iSortCol_0"].ToString() : null;
            string sortorder = Request.Params["sSortDir_0"] != null ? Request.Params["sSortDir_0"].ToString() : null;
            string istart = Request.Params["iDisplayStart"].ToString();
            string iDisplayStartstr = Request.Params["iDisplayStart"].ToString();
            int iDisplayStart = Convert.ToInt16(iDisplayStartstr);
            string iDisplayLengthstr = Request.Params["iDisplayLength"].ToString();
            int iDisplayLength = Convert.ToInt16(iDisplayLengthstr);

            int page = Convert.ToInt16(pageno);
            var model = roleRepo.GetRoleList(page, Name, Description, searchpara, sortablecol, sortorder).ToList();

            var _Data = model.Skip(iDisplayStart).Take(iDisplayLength).ToList();

            string[][] strarr = new string[_Data.Count()][];
            for (var i = 0; i < _Data.Count(); i++)
            {
                string[] subaray = new string[5];
                subaray[0] = "<input type='checkbox' id='chk_" + _Data[i].RoleId + "' class='checkbox'/> <label for='chk_" + _Data[i].RoleId + "'></label>";
                subaray[1] = _Data[i].RoleName;
                subaray[2] = _Data[i].RoleDesc;

                if (_Data[i].IsActive == true)
                {
                    subaray[3] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].RoleId + "','Y')>Yes</a>";
                }
                else
                {
                    subaray[3] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].RoleId + "','N')>No</a>";
                }

                subaray[4] = "<a class='btn btn-round ink-reaction btn-primary fa fa-edit' data-ajax-complete='scrolltop();' data-ajax='true' data-ajax-method='GET' data-ajax-mode='replace' data-ajax-update='#P_Main' href='/Role/Role?RoleId=" + _Data[i].RoleId + "' title='Edit'></a>" + "   " + "<a href='javascript:void(0);' class='btn btn-round btn-danger ink-reaction fa fa-trash-o' title='Delete' onclick=DeleteRole('" + _Data[i].RoleId + "')></a>";

                strarr[i] = subaray;
            }

            return Json(new { sEcho = Request.Params["sEcho"], recordsTotal = model.Count().ToString(), recordsFiltered = model.Count().ToString(), iTotalRecords = model.Count().ToString(), iTotalDisplayRecords = model.Count().ToString(), aaData = strarr }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}