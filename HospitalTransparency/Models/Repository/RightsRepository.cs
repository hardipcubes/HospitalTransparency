using HospitalTransparency.DataAccess;
using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static HospitalTransparency.Helper.CommonStaticMethod;

namespace HospitalTransparency.Models.Repository
{
    public class RightsRepository : IRightsRepository, IDisposable
    {
        HospitalTransparencyEntities db = new HospitalTransparencyEntities();
        RightsDAL DAL = new RightsDAL();
        #region GetRightsDetails
        public IEnumerable<RightsModel> GetRightsDetails(string userId)
        {
            
                IList<RightsModel> rightsDetails = new List<RightsModel>();
                using (var db = new HospitalTransparencyEntities())
                {
                    var rightsData = db.Sp_MenuRightsList().ToList();
                    foreach (var item in rightsData)
                    {
                        var pageModel = new RightsModel();
                        {
                            //pageModel.MenuId = item.MenuId;
                            //pageModel.MenuName = item.MenuName;
                            //pageModel.MenuType = item.MenuType;
                            //rightsDetails.Add(pageModel);
                        }
                    }
                }
                return rightsDetails;
             
        }
        #endregion

        #region GetRightsByRoleId
        public IEnumerable<RightsModel> GetRightsByRoleId(int roleId)
        {
            using (var db = new HospitalTransparencyEntities())
            {
                IList<RightsModel> details = new List<RightsModel>();
                var data = (from mm in db.MenuMasters
                            join rm in db.RightsMasters on mm.MenuId equals rm.MenuId into rights
                            from right in rights.DefaultIfEmpty()
                            where right.RoleId == roleId && mm.IsDeleted == false && mm.IsActive == true
                            select new
                            {
                                right.Display,
                                right.MenuId,
                                right.Add,
                                right.Delete,
                                right.Edit,
                                right.RightsId,
                                right.RoleId,
                                mm.MenuName
                            }).ToList();

                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        var rights = new RightsModel();
                        {
                            rights.MenuId = item.MenuId;
                            rights.MenuName = item.MenuName;
                            rights.Add = item.Add;
                            rights.Edit = item.Edit;
                            rights.Delete = item.Delete;
                            rights.Display = item.Display;
                            rights.RoleId = item.RoleId;
                            details.Add(rights);
                        }
                    }
                }
                return details;
            }
        }
        #endregion

        #region Add Rights
        public bool AddRights(RightsModel rights)
        {
             
                using (var db = new HospitalTransparencyEntities())
                {
                    var pageData = new RightsMaster();
                    {
                        pageData.RoleId = rights.RoleId;
                        pageData.MenuId = rights.MenuId;
                        pageData.Add = Convert.ToBoolean(rights.Add);
                        pageData.Edit = Convert.ToBoolean(rights.Edit);
                        pageData.Delete = Convert.ToBoolean(rights.Delete);
                        pageData.Display = Convert.ToBoolean(rights.Display);
                        pageData.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                        pageData.CreatedDate = DateTime.Now;
                        pageData.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                        pageData.ModifiedDate = DateTime.Now;
                        db.RightsMasters.Add(pageData);
                        db.SaveChanges();
                    }
                }
                return true;
             
        }
        #endregion

        #region Update Rights
        public bool UpdateRights(RightsMaster rights)
        {
            
                using (var db = new HospitalTransparencyEntities())
                {
                    db.RightsMasters.Add(rights);
                    db.Entry(rights).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
             
        }
        #endregion

        #region MenuList

        public List<DisplayMenuModel> DisplayMenuList()
        {
            List<DisplayMenuModel> lstmodel = new List<DisplayMenuModel>();
            DataTable dt = new DataTable();
            dt = DAL.GetRightsMenuList();
            lstmodel = GetTopLevelRows(dt)
               .Select(row => CreateItem(dt, row))
               .ToList();

            return lstmodel;
        }

        /// <summary>
        /// Gets the children menu.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        private IEnumerable<DataRow> GetChildren(DataTable dataTable, Int32 parentId)
        {
            return dataTable
              .Rows
              .Cast<DataRow>()
              .Where(row => row.Field<Int32>("ParentId") == parentId);
        }

        /// <summary>
        /// Creates the children item.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        private DisplayMenuModel CreateItem(DataTable dataTable, DataRow row)
        {
            var id = SafeValue<int>(row.Field<Int32>("MenuId"));
            var children = GetChildren(dataTable, id)
              .Select(r => CreateItem(dataTable, r))
              .ToList();
            return new DisplayMenuModel
            {
                MenuId = id,
                SrNo = 1,
                MenuName = SafeValue<string>(row["MenuName"]),
                MenuLevel = SafeValue<int>(row["MenuLevel"]),
                MenuIcon = SafeValue<string>(row["MenuIcon"]),
                ParentId = SafeValue<int>(row["ParentId"]),
                ParentIdList = SafeValue<string>(row["ParentIdList"]),
                OrderBy = SafeValue<int>(row["Orderby"]),
                Children = children
            };
        }

        /// <summary>
        /// Gets the top level rows.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        private IEnumerable<DataRow> GetTopLevelRows(DataTable dataTable)
        {
            return dataTable
              .Rows
              .Cast<DataRow>()
              .Where(row => row.Field<Int32>("ParentId") == 0);
        }

        public IEnumerable<RightsModel> MenuList()
        {
            using (var db = new HospitalTransparencyEntities())
            {
                
                    var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                    var getRoleId = Convert.ToInt32(HttpContext.Current.Session["RoleId"]);
                    var actionName = new MenuMaster();

                    actionName = db.MenuMasters.Where(m => m.ControllerName == controller && m.IsActive == true).ToList().FirstOrDefault();

                    #region Active Menu
                    if (actionName != null)
                    {
                        var menuId = actionName.MenuId;
                        var parentId = actionName.ParentId;
                        HttpContext.Current.Session["ActivemenuId"] = menuId;
                        HttpContext.Current.Session["ActiveparentId"] = parentId;
                    }
                    #endregion

                    var roleQuery = (from r in db.RightsMasters
                                     join m in db.MenuMasters on r.MenuId equals m.MenuId
                                     where m.ActionName == actionName.ActionName && m.ControllerName == controller
                                           && r.RoleId == getRoleId
                                     select new { r.Add, r.Edit, r.Display, r.Delete, r.MenuId, m.ParentId }).FirstOrDefault();

                    if (roleQuery != null)
                    {
                        HttpContext.Current.Session["RightAdd"] = roleQuery.Add;
                        HttpContext.Current.Session["RightEdit"] = roleQuery.Edit;
                        HttpContext.Current.Session["RightDelete"] = roleQuery.Delete;
                        HttpContext.Current.Session["RightDisplay"] = roleQuery.Display;
                        HttpContext.Current.Session["MenuId"] = roleQuery.MenuId;
                        HttpContext.Current.Session["ParentMenuId"] = roleQuery.ParentId;
                    }
                    IList<RightsModel> rightsData = new List<RightsModel>();

                    var nquery = (from rm in db.RightsMasters
                                  join mm in db.MenuMasters on rm.MenuId equals mm.MenuId
                                  join mi in db.MenuIcons on mm.MenuIcon equals mi.Id
                                  where rm.RoleId == getRoleId && mm.IsActive == true && (rm.Add == true || rm.Edit == true || rm.Delete == true || rm.Display == true)
                                  orderby mm.OrderBy
                                  select new
                                  {
                                      rm.Add,
                                      rm.RoleId,
                                      rm.Delete,
                                      rm.Edit,
                                      rm.Display,
                                      mm.ControllerName,
                                      mm.MenuId,
                                      mm.ActionName,
                                      mm.MenuName,
                                      mm.ParentId,
                                      mi.IconClass
                                  }).ToList();



                    foreach (var courseData in nquery)
                    {
                        rightsData.Add(new RightsModel
                        {
                            Add = courseData.Add,
                            Edit = courseData.Edit,
                            Delete = courseData.Delete,
                            Display = courseData.Display,
                            ControllerName = courseData.ControllerName,
                            ActionName = courseData.ActionName,
                            MenuName = courseData.MenuName,
                            ParentId = courseData.ParentId,
                            MenuId = courseData.MenuId,
                            MenuIcon = courseData.IconClass
                        });
                    }
                    return rightsData;
                
            }
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