using HospitalTransparency.App_Start;
using HospitalTransparency.Models;
using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class RightsController : SessionController
    {
        IRightsRepository rights = new RightsRepository();
        public ActionResult Rights()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserId"])))
            {
                using (var db = new RightsRepository())
                {
                    Session["RightEdit"] = "";
                    //var model = db.GetRightsDetails(null);
                    List<DisplayMenuModel> model = rights.DisplayMenuList();
                    RoleList(null);
                    return View(model);
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddRights(int roleId, string[] menuData)
        {
            
                using (var db = new HospitalTransparencyEntities())
                {
                    #region Remove rights first from table
                    db.Database.ExecuteSqlCommand("Delete from RightsMaster where RoleId = '" + roleId + "'");
                    #endregion

                    #region If rights are not for Super Admin then Update Rights for all users for that particular role

                    foreach (var item in menuData)
                    {
                        var menus = item.Split(',');
                        var menuId = menus[0];
                        bool? add = Convert.ToBoolean(menus[1]);
                        bool? edit = Convert.ToBoolean(menus[2]);
                        bool? delete = Convert.ToBoolean(menus[3]);
                        bool? display = Convert.ToBoolean(menus[4]);

                        var right = new RightsModel();
                        right.MenuId = int.Parse(menuId);
                        right.RoleId = roleId;
                        right.Add = add;
                        right.Edit = edit;
                        right.Display = display;
                        right.Delete = delete;
                        right.ClientId = null;
                        right.UserId = null;
                        right.CreatedBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                        right.CreatedDate = DateTime.Now;
                        using (var context = new RightsRepository())
                        {
                            context.AddRights(right);
                        }
                    }

                CustomCacheManagement.MenuList = new RightsListRepository().GetMenuList();
                #endregion

                return Json(true, JsonRequestBehavior.AllowGet);
                }
             
        }

        public ActionResult MenuListFilter(int roleId)
        {
            using (var db = new RightsRepository())
            {
                var getComData = db.GetRightsByRoleId(roleId);
                return Json(getComData, JsonRequestBehavior.AllowGet);
            }
        }

        #region Role List
        public string RoleList(string role)
        {
            int roleid = Convert.ToInt32(Session["RoleId"].ToString());
            using (var db = new HospitalTransparencyEntities())
            {
                if (roleid == 1)
                {
                    var roleList = (from rm in db.RoleMasters
                                    where rm.IsDeleted == false && rm.IsActive == true
                                    select new { rm.RoleId, rm.RoleName, rm.DisplayOrder }).Distinct().OrderBy(r => r.RoleName).ToList();
                    ViewBag.roleList = !string.IsNullOrEmpty(role) ? new SelectList(roleList, "RoleId", "RoleName", role) : new SelectList(roleList, "RoleId", "RoleName");
                }
                else
                {
                    var roleList = (from rm in db.RoleMasters
                                    where rm.IsDeleted == false && rm.IsActive == true && rm.RoleId == roleid
                                    select new { rm.RoleId, rm.RoleName, rm.DisplayOrder }).Distinct().OrderBy(r => r.RoleName).ToList();
                    ViewBag.roleList = !string.IsNullOrEmpty(role) ? new SelectList(roleList, "RoleId", "RoleName", roleid) : new SelectList(roleList, "RoleId", "RoleName");
                }
            }
            return "Yes";
        }
        #endregion

        public JsonResult ManageRootMenu(int RoleId, int MenuId)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                var getComData = (from c in db.MenuMasters where c.MenuId == MenuId && c.ParentId != null select c).ToList();
                if (getComData.Any())
                {
                    int? parentId = 0;
                    foreach (var item in getComData)
                    {
                        parentId = item.ParentId;
                    }
                    getComData = (from c in db.MenuMasters where c.IsActive == true && c.IsDeleted == false && c.ParentId == parentId select c).ToList();
                }
                return Json(getComData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetParentMenuByChild(int childMenuId, int roleId)
        {
            int? parentid = 0;
            using (var db1 = new HospitalTransparencyEntities())
            {
                var Data = db1.MenuMasters.Where(m => m.MenuId == childMenuId).ToList();

                if (Data.Count > 0)
                {
                    parentid = Data.First().ParentId;
                    var menu = db1.Sp_GetParentMenuByChildId(parentid, roleId).ToList();
                    return Json(new { parentid = parentid, menu = menu }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { parentid = parentid, }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetParentMenuByChild_MenuMaster(int parentId)
        {
            string parentid = string.Empty;
            using (var db1 = new HospitalTransparencyEntities())
            {
                var menu = db1.MenuMasters.Where(m => m.ParentId == parentId && m.IsActive == true).ToList();
                return Json(new { parentid = parentid, menu = menu }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult AllMenuList()
        //{
        //    using (var db = new HospitalTransparencyEntities())
        //    {
        //        var getComData = db.Sp_MenuRightsList().ToList();
        //        return Json(getComData, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}