using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Repository
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        HospitalTransparencyEntities db = new HospitalTransparencyEntities();

        #region Get Role List
        public IEnumerable<RoleModel> GetRoleList(int? page, string Name, string Description, string searchfilter, string SortCol, string SortOrder)
        {
           
                IList<RoleModel> RoleList = new List<RoleModel>();
                var roleId = Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());

                var query = (from m in db.RoleMasters
                             where m.IsDeleted == false && m.RoleId > roleId
                             select new { m.RoleName, m.RoleDesc, m.RoleId, m.IsActive, m.IsDeleted, m.CreatedOn }).OrderByDescending(x => x.CreatedOn).ToList();

                #region Searching
                if (!string.IsNullOrEmpty(searchfilter))
                    query = query.Where(e => (!String.IsNullOrEmpty(e.RoleName) && e.RoleName.ToUpper().Contains(searchfilter.ToUpper()))
                        || (!String.IsNullOrEmpty(e.RoleDesc) && e.RoleDesc.ToUpper().Contains(searchfilter.ToUpper()))).ToList();
                #endregion

                #region Sorting Acs/Desc
                if (SortOrder == "asc")
                {
                    if (SortCol == "0")
                    {
                        query = query.OrderBy(e => e.RoleName).ToList();
                    }
                }
                else
                {
                    if (SortCol == "0")
                    {
                        query = query.OrderByDescending(e => e.RoleName).ToList();
                    }
                }

                if (SortOrder == "asc")
                {
                    if (SortCol == "1")
                    {
                        query = query.OrderBy(e => e.RoleDesc).ToList();
                    }
                }
                else
                {
                    if (SortCol == "1")
                    {
                        query = query.OrderByDescending(e => e.RoleDesc).ToList();
                    }
                }

                /*-------  -----------*/

                if (SortOrder == "asc")
                {
                    if (SortCol == "2")
                    {
                        query = query.OrderBy(e => e.IsActive).ToList();
                    }
                }
                else
                {
                    if (SortCol == "2")
                    {
                        query = query.OrderByDescending(e => e.IsActive).ToList();
                    }
                }


                #endregion

                foreach (var PageData in query)
                {
                    RoleModel PageModel = new RoleModel();
                    {
                        PageModel.RoleId = PageData.RoleId;
                        PageModel.RoleName = PageData.RoleName;
                        PageModel.RoleDesc = PageData.RoleDesc;
                        PageModel.IsActive = PageData.IsActive;
                        PageModel.CreatedOn = PageData.CreatedOn;
                        PageModel.IsDeleted = PageData.IsDeleted;
                        RoleList.Add(PageModel);
                    };
                }
                return RoleList;
             
        }
        #endregion

        #region Get Role Details By RoleId
        public RoleModel GetRoleById(int roleId)
        {
            
                var data = db.RoleMasters.Where(x => x.RoleId == roleId).FirstOrDefault();
                RoleModel roleData = new RoleModel();
                roleData.RoleId = data.RoleId;
                roleData.RoleName = data.RoleName;
                roleData.RoleDesc = data.RoleDesc;
                roleData.CreatedOn = data.CreatedOn;
                roleData.ModifiedOn = data.ModifiedOn;
                roleData.CreatedBy = data.CreatedBy;
                roleData.ModifiedBy = data.ModifiedBy;
                roleData.IsDeleted = data.IsDeleted;
                roleData.IsActive = data.IsActive;
                roleData.DeletedOn = data.DeletedOn;
                return roleData;
             
        }
        #endregion

        #region Add/Edit Role
        public bool SaveRole(RoleModel role)
        {
             
                var roleDetail = db.RoleMasters.Find(role.RoleId);
                var roleTbl = new RoleMaster();
                if (roleDetail == null)
                {
                    roleTbl.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    roleTbl.CreatedOn = DateTime.Now;
                    roleTbl.RoleName = role.RoleName;
                    roleTbl.RoleDesc = role.RoleDesc.Trim();
                    roleTbl.IsDeleted = false;
                    roleTbl.IsActive = true;
                    roleTbl.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    db.RoleMasters.Add(roleTbl);
                }
                else
                {
                    db.Entry(roleDetail).State = EntityState.Modified;
                    roleDetail.ModifiedOn = DateTime.Now;
                    roleDetail.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    roleDetail.RoleName = role.RoleName.Trim();
                    roleDetail.RoleDesc = role.RoleDesc.Trim();
                }
                db.SaveChanges();
                return true;
             
        }
        #endregion

        #region Activate/Deactive Role
        public string SetActiveDeactive(int roleId)
        {
            
                var data = db.RoleMasters.Find(roleId);
                string isActiveMsg = (data.IsActive ? "deactivated" : "activated");
                if (data.IsActive == true)
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
        #endregion

        #region Delete role
        public bool DeleteRole(int roleId)
        {
             
                var roleData = db.RoleMasters.Find(roleId);
                if (roleData != null)
                {
                    roleData.ModifiedOn = DateTime.Now;
                    roleData.IsDeleted = true;
                    roleData.DeletedOn = DateTime.Now;
                    db.Entry(roleData).CurrentValues.SetValues(roleData);
                }

                var rightsData = db.RightsMasters.Where(m => m.RoleId == roleId).ToList();
                if (rightsData.Any())
                {
                    foreach (var item in rightsData)
                    {
                        var rightsDetail = db.RightsMasters.Find(item.RightsId);
                        if (rightsDetail != null)
                        {
                            db.Entry(rightsDetail).State = EntityState.Deleted;
                        }
                    }
                }

                db.SaveChanges();
                return true;
             
        }

        public bool DeleteAllRole(string roleIds)
        {
             
                var db = new HospitalTransparencyEntities();
                foreach (var item in roleIds.Split(','))
                {
                    var roleId = Convert.ToInt64(item);
                    var roleData = db.RoleMasters.Find(roleId);
                    if (roleData != null)
                    {
                        roleData.ModifiedOn = DateTime.Now;
                        roleData.IsDeleted = true;
                        roleData.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                        db.Entry(roleData).CurrentValues.SetValues(roleData);
                    }

                    var rightsData = db.RightsMasters.Where(m => m.RoleId == roleId).ToList();
                    if (rightsData.Any())
                    {
                        foreach (var rightsItem in rightsData)
                        {
                            var rightsDetail = db.RightsMasters.Find(rightsItem.RightsId);
                            if (rightsDetail != null)
                            {
                                db.Entry(rightsDetail).State = EntityState.Deleted;
                            }
                        }
                    }
                }
                db.SaveChanges();
                return true;
            
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