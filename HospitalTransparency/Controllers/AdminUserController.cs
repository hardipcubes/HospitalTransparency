using HospitalTransparency.Models;
using HospitalTransparency.Models.Model;
using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalTransparency.Helper;

namespace HospitalTransparency.Controllers
{
    public class AdminUserController : SessionController
    {
        readonly HospitalTransparencyEntities db = new HospitalTransparencyEntities();
        AdminUserRepository userRepo = new AdminUserRepository();

        public ActionResult AdminUser(int? UserId)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                RoleList(null);
                if (UserId == null || UserId == 0)
                {
                    return View();
                }
                else
                {
                    ViewBag.Edit = "Edit";
                    var Data = userRepo.GetUserById(Convert.ToInt32(UserId));
                    ViewBag.UserImage = !string.IsNullOrEmpty(Data.Image) ? Data.Image : "../Content/images/notavailable.png";
                    TempData["UserImage"] = Data.Image;
                    return PartialView("P_Main", Data);
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AdminUser(AdminUserModel users)
        {
            bool status = false;
            ViewBag.SelectedRole = "User";
            if (TempData["UserImage"] != null)
            {
                if (!string.IsNullOrEmpty(TempData["UserImage"].ToString()))
                    users.Image = TempData["UserImage"].ToString();
            }
            status = userRepo.SaveUser(users);
            if (status == true)
            {
                ViewBag.SuccessMessage = "Record saved successfully.";
            }
            else
            {
                ViewBag.ErrorMessage = "Please try again.";
            }
            return PartialView("P_List");
        }

        public ActionResult Delete(int UserId)
        {
            var data = userRepo.DeleteUser(UserId);
            if (data == true)
            {
                ViewBag.SuccessMessage = "User deleted successfully. :)";
            }
            else
            {
                ViewBag.ErrorMessage = "Please try again. :(";
            }
            return PartialView("P_List");
        }

        public ActionResult DeleteAll(string userIds)
        {
            var data = userRepo.DeleteAllUser(userIds.TrimEnd(','));
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

        #region SetActiveDeactive
        public ActionResult SetActiveDeactive(int UserId)
        {
            var model = userRepo.SetActiveDeactive(UserId);
            ViewBag.SuccessMessage = "User " + model + " successfully. :)";
            return PartialView("P_List");
        }
        #endregion

        public JsonResult fnPaging()
        {
            string pageno = Request.Params["sEcho"] ?? "1";
            string Name = Request.Params["Name"];
            string RoleName = Request.Params["RoleName"];
            string Email = Request.Params["Email"];
            string searchpara = Request.Params["sSearch"];
            string sortablecol = Request.Params["iSortCol_0"] != null ? Request.Params["iSortCol_0"].ToString() : null;
            string sortorder = Request.Params["sSortDir_0"] != null ? Request.Params["sSortDir_0"].ToString() : null;
            string istart = Request.Params["iDisplayStart"];
            string iDisplayStartstr = Request.Params["iDisplayStart"];
            int iDisplayStart = Convert.ToInt16(iDisplayStartstr);
            string iDisplayLengthstr = Request.Params["iDisplayLength"];
            int iDisplayLength = Convert.ToInt16(iDisplayLengthstr);
            int page = Convert.ToInt16(pageno);

            var model = userRepo.GetUsersList(page, Name, RoleName, Email, searchpara, sortablecol, sortorder).ToList();

            var _Data = model.Skip(iDisplayStart).Take(iDisplayLength).ToList();
            var roleId = Convert.ToInt32(Session["RoleId"]);

            string[][] strarr = new string[_Data.Count()][];
            for (var i = 0; i < _Data.Count(); i++)
            {
                string[] subaray = new string[6];
                subaray[0] = "<input type='checkbox' id='chk_" + _Data[i].UserId + "' class='checkbox'/> <label for='chk_" + _Data[i].UserId + "'></label>";
                subaray[1] = _Data[i].Name;
                subaray[2] = _Data[i].RoleName;
                subaray[3] = _Data[i].Email;

                if (roleId != _Data[i].RoleId)
                {
                    if (_Data[i].IsActive == true)
                    {
                        subaray[4] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].UserId + "','Y')>Yes</a>";
                    }
                    else
                    {
                        subaray[4] = "<a href='javascript:void(0);' onclick=SetActiveDeactive('" + _Data[i].UserId + "','N')>No</a>";
                    }

                    subaray[5] = "<a class='btn btn-round ink-reaction btn-primary fa fa-edit' data-ajax-complete='scrolltop();editcharCount();' data-ajax='true' data-ajax-method='GET' data-ajax-mode='replace' data-ajax-update='#P_Main' href='/AdminUser/AdminUser?UserId=" + _Data[i].UserId + "' title='Edit'></a>" + "   " + "<a href='javascript:void(0);' class='btn btn-round btn-danger ink-reaction fa fa-trash-o' title='Delete' onclick=Ondelete('" + _Data[i].UserId + "')></a>";

                    if (_Data[i].IsActive == true)
                    {
                        subaray[5] += " " + "<a href='/Login/DirectLogin?userName=" + _Data[i].Username + "&password=" + _Data[i].Password + "' title='Login' class='btn btn-round ink-reaction btn-default fa fa-sign-in'></a>";
                    }
                }
                else
                {
                    if (_Data[i].IsActive == true)
                    {
                        subaray[4] = "Yes";
                    }
                    else
                    {
                        subaray[4] = "No";
                    }
                    subaray[5] = "<a class='btn btn-round ink-reaction btn-primary fa fa-edit' data-ajax-complete='scrolltop();editcharCount();' data-ajax='true' data-ajax-method='GET' data-ajax-mode='replace' data-ajax-update='#P_Main' href='/AdminUser/AdminUser?UserId=" + _Data[i].UserId + "' title='Edit'></a>";
                }
                strarr[i] = subaray;
            }
            return Json(new { sEcho = Request.Params["sEcho"], recordsTotal = model.Count().ToString(), recordsFiltered = model.Count().ToString(), iTotalRecords = model.Count().ToString(), iTotalDisplayRecords = model.Count().ToString(), aaData = strarr }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Checkusernamevalidation(string C_UserName, int Id)
        {
            bool flag = false;
            int count = (from m in db.AdminPanelUserMasters where m.Username == C_UserName && m.UserId != Id && m.IsDeleted == false select new { m.Username }).ToList().Count();
            if (count > 0)
            {
                flag = true;
            }
            return Json(new { flag = flag }, JsonRequestBehavior.AllowGet);
        }

        #region ParentList
        public string RoleList(int? roleId)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                var sessioRoleId = Convert.ToInt32(Session["RoleId"]);
                var roleList = (from rm in db.RoleMasters
                                where rm.IsDeleted == false && rm.IsActive == true
                                select new { rm.RoleId, rm.RoleName }).ToList();

                if (sessioRoleId != 1)
                {
                    roleList = roleList.Where(m => m.RoleId != 1).ToList();
                }
                ViewBag.RoleData = roleId != 0 ? new SelectList(roleList, "RoleId", "RoleName", roleId) : new SelectList(roleList, "RoleId", "RoleName");
            }
            return "Yes";
        }
        #endregion

        #region Upload File
        [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Upload/"), DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                file.SaveAs(path);
                var base64Path = General.ConvertImageToBase64(path);
                if (System.IO.File.Exists(path) && base64Path.Length > 10)
                {
                    System.IO.File.Delete(path);
                }
                TempData["UserImage"] = base64Path;
            }
        }
        #endregion
    }
}