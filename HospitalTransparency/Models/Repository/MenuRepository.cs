using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Repository
{
    public class MenuRepository : IMenuRepository, IDisposable
    {
        HospitalTransparencyEntities db = new HospitalTransparencyEntities();

        #region Get MenuList
        public IEnumerable<MenuModel> GetMenuList(int? page, string menuName, string controllerName, string actionName, string orderBy, string searchfilter, string sortCol, string sortOrder)
        {
            try
            {
                IList<MenuModel> menuList = new List<MenuModel>();
                var query = (from m in db.MenuMasters
                             where m.IsDeleted == false
                             select new { m.MenuId, m.MenuName, m.ControllerName, m.ActionName, m.MenuIcon, m.ParentId, m.OrderBy, m.IsActive, m.IsDeleted, m.CreatedOn }).OrderByDescending(x => x.CreatedOn).ToList();

                #region Searching
                if (!string.IsNullOrEmpty(searchfilter))
                    query = query.Where(e => (!String.IsNullOrEmpty(e.MenuName) && e.MenuName.ToUpper().Contains(searchfilter.ToUpper())) || (!String.IsNullOrEmpty(e.ActionName) && e.ActionName.ToUpper().Contains(searchfilter.ToUpper())) || (!String.IsNullOrEmpty(e.OrderBy.ToString()) && e.OrderBy.ToString().ToUpper().Contains(searchfilter.ToUpper())) || (!String.IsNullOrEmpty(e.ControllerName) && e.ControllerName.ToUpper().Contains(searchfilter.ToUpper()))).ToList();
                #endregion

                #region Sorting Acs/Desc
                if (sortOrder == "asc")
                {
                    if (sortCol == "0")
                    {
                        query = query.OrderBy(e => e.MenuName).ToList();
                    }
                }
                else
                {
                    if (sortCol == "0")
                    {
                        query = query.OrderByDescending(e => e.MenuName).ToList();
                    }
                }

                if (sortOrder == "asc")
                {
                    if (sortCol == "1")
                    {
                        query = query.OrderBy(e => e.ControllerName).ToList();
                    }
                }
                else
                {
                    if (sortCol == "1")
                    {
                        query = query.OrderByDescending(e => e.ControllerName).ToList();
                    }
                }

                if (sortOrder == "asc")
                {
                    if (sortCol == "2")
                    {
                        query = query.OrderBy(e => e.ActionName).ToList();
                    }
                }
                else
                {
                    if (sortCol == "2")
                    {
                        query = query.OrderByDescending(e => e.ActionName).ToList();
                    }
                }

                if (sortOrder == "asc")
                {
                    if (sortCol == "3")
                    {
                        query = query.OrderBy(e => e.OrderBy).ToList();
                    }
                }
                else
                {
                    if (sortCol == "3")
                    {
                        query = query.OrderByDescending(e => e.OrderBy).ToList();
                    }
                }

                /*-------  -----------*/

                if (sortOrder == "asc")
                {
                    if (sortCol == "4")
                    {
                        query = query.OrderBy(e => e.IsActive).ToList();
                    }
                }
                else
                {
                    if (sortCol == "4")
                    {
                        query = query.OrderByDescending(e => e.IsActive).ToList();
                    }
                }


                #endregion

                foreach (var pageData in query)
                {
                    var pageModel = new MenuModel();
                    {
                        pageModel.MenuId = pageData.MenuId;
                        pageModel.MenusName = pageData.MenuName;
                        pageModel.ControllerName = pageData.ControllerName;
                        pageModel.ActionName = pageData.ActionName;
                        pageModel.ParentId = pageData.ParentId;
                        pageModel.OrderBy = pageData.OrderBy;
                        pageModel.MenuIcon = pageData.MenuIcon;
                        pageModel.IsActive = pageData.IsActive;
                        pageModel.CreatedOn = pageData.CreatedOn;
                        pageModel.IsDeleted = pageData.IsDeleted;
                        menuList.Add(pageModel);
                    }
                }
                return menuList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Add/Edit Menu
        public bool SaveMenu(MenuModel menus)
        {
            try
            {
                var menuTbl = new MenuMaster();
                if (menus.MenuId == 0)
                {
                    menuTbl.MenuName = menus.MenusName;
                    if (menus.ControllerName != null)
                    {
                        menuTbl.ControllerName = menus.ControllerName.Trim();
                    }
                    if (menus.ActionName != null)
                    {
                        menuTbl.ActionName = menus.ActionName.Trim();
                    }
                    menuTbl.ParentId = menus.ParentId;
                    menuTbl.OrderBy = menus.OrderBy;
                    menuTbl.MenuIcon = menus.MenuIcon;
                    menuTbl.IsActive = true;
                    menuTbl.IsDeleted = false;
                    menuTbl.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    menuTbl.CreatedOn = DateTime.Now;
                    menuTbl.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    menuTbl.ModifiedOn = DateTime.Now;
                    db.MenuMasters.Add(menuTbl);
                }
                else
                {
                    var menu = db.MenuMasters.Find(menus.MenuId);
                    db.Entry(menu).State = EntityState.Modified;
                    menu.ModifiedOn = DateTime.Now;
                    menu.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    menu.MenuName = menus.MenusName;
                    if (menus.ControllerName != null)
                    {
                        menu.ControllerName = menus.ControllerName.Trim();
                    }
                    if (menus.ActionName != null)
                    {
                        menu.ActionName = menus.ActionName.Trim();
                    }
                    menu.ParentId = menus.ParentId;
                    menu.OrderBy = menus.OrderBy;
                    menu.MenuIcon = menus.MenuIcon;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Activate/Deactive Menu
        public string SetActiveDeactive(int menuId)
        {
            try
            {
                var data = db.MenuMasters.Find(menuId);
                string isActiveMsg = (data.IsActive ? "deactivated" : "activated");
                if (data.IsActive)
                {
                    data.IsActive = false;
                    db.Entry(data).CurrentValues.SetValues(data);
                }
                else
                {
                    data.IsActive = true;
                    db.Entry(data).CurrentValues.SetValues(data);

                }
                db.SaveChanges();
                return isActiveMsg;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region Delete Menu
        public bool DeleteMenu(int menuId)
        {
            try
            {
                var menuData = db.MenuMasters.Find(menuId);
                if (menuData != null)
                {
                    menuData.ModifiedOn = DateTime.Now;
                    menuData.IsDeleted = true;
                    db.Entry(menuData).CurrentValues.SetValues(menuData);
                    db.SaveChanges();
                }
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Delete All Menus
        public bool DeleteAllMenu(string menuIds)
        {
            
                var db = new HospitalTransparencyEntities();
                foreach (var item in menuIds.Split(','))
                {
                    var menuId = Convert.ToInt64(item);
                    var menuData = db.MenuMasters.Find(menuId);
                    if (menuData != null)
                    {
                        menuData.ModifiedOn = DateTime.Now;
                        menuData.IsDeleted = true;
                        menuData.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                        db.Entry(menuData).CurrentValues.SetValues(menuData);
                    }
                }
                db.SaveChanges();
                return true;
             
        }
        #endregion

        #region Get MenuData By Id
        public MenuModel GetMenuDataById(int menuId)
        {

            var menuData = new MenuModel();
            var data = db.MenuMasters.FirstOrDefault(x => x.MenuId == menuId);
            if (data != null)
            {
                menuData.MenuId = data.MenuId;
                menuData.MenusName = data.MenuName;
                menuData.ControllerName = data.ControllerName;
                menuData.ActionName = data.ActionName;
                menuData.ParentId = data.ParentId;
                menuData.OrderBy = data.OrderBy;
                menuData.MenuIcon = data.MenuIcon;
                menuData.CreatedOn = data.CreatedOn;
                menuData.ModifiedOn = data.ModifiedOn;
                menuData.CreatedBy = data.CreatedBy;
                menuData.ModifiedBy = data.ModifiedBy;
                menuData.IsDeleted = data.IsDeleted;
                menuData.IsActive = data.IsActive;

                if (data.MenuIcon != null)
                {
                    var menuIcon = db.MenuIcons.FirstOrDefault(m => m.Id == data.MenuIcon);
                    if (menuIcon != null)
                    {
                        menuData.Iconname = menuIcon.IconClass;
                    }
                }
            }
            return menuData;

        }
        #endregion

        #region Dispose
        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    using (var db = new HospitalTransparencyEntities())
                    {
                        db.Dispose();
                    }
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}