using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Repository
{
    public class AdminUserRepository : IAdminUserRepository, IDisposable
    {
        HospitalTransparencyEntities db = new HospitalTransparencyEntities();

        #region Get Admin Users List
        public IEnumerable<AdminUserModel> GetUsersList(int? page, string Name, string RoleName, string Email, string searchfilter, string SortCol, string SortOrder)
        {
             
                IList<AdminUserModel> adminUsersList = new List<AdminUserModel>();
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    var id = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    var roleId = Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());

                var query = (from m in db.AdminPanelUserMasters
                             join r in db.RoleMasters on m.RoleId equals r.RoleId
                             where m.IsDeleted == false //&& (m.RoleId > roleId || m.UserId == id)
                                && r.IsActive == true && r.IsDeleted == false
                             select new { m.UserId, m.Name, r.RoleName, m.Password, m.Email, m.IsActive, m.CreatedOn, m.RoleId, m.Username }).OrderByDescending(x => x.CreatedOn).ToList();

                if (!String.IsNullOrEmpty(Name))
                    {
                        query = query.Where(e => e.Name == Name).ToList();
                    }
                    if (!String.IsNullOrEmpty(RoleName))
                    {
                        query = query.Where(e => e.RoleName == RoleName).ToList();
                    }
                    if (!String.IsNullOrEmpty(Email))
                    {
                        query = query.Where(e => e.Email == Email).ToList();
                    }

                    #region Searching
                    if (!string.IsNullOrEmpty(searchfilter))
                        query = query.Where(e => (!String.IsNullOrEmpty(e.Name) && e.Name.ToUpper().Contains(searchfilter.ToUpper())) || (!String.IsNullOrEmpty(e.RoleName) && e.RoleName.ToUpper().Contains(searchfilter.ToUpper())) || (!String.IsNullOrEmpty(e.Email) && e.Email.ToUpper().Contains(searchfilter.ToUpper()))).ToList();
                    #endregion

                    #region Sorting Acs/Desc

                    /*-------Name-----------*/
                    if (SortOrder == "asc")
                    {
                        if (SortCol == "0")
                        {
                            query = query.OrderBy(e => e.Name).ToList();
                        }
                    }
                    else
                    {
                        if (SortCol == "0")
                        {
                            query = query.OrderByDescending(e => e.Name).ToList();
                        }
                    }

                    /*-------UserName-----------*/
                    if (SortOrder == "asc")
                    {
                        if (SortCol == "1")
                        {
                            query = query.OrderBy(e => e.RoleName).ToList();
                        }
                    }
                    else
                    {
                        if (SortCol == "1")
                        {
                            query = query.OrderByDescending(e => e.RoleName).ToList();
                        }
                    }


                    /*-------Email-----------*/
                    if (SortOrder == "asc")
                    {
                        if (SortCol == "2")
                        {
                            query = query.OrderBy(e => e.Email).ToList();
                        }
                    }
                    else
                    {
                        if (SortCol == "2")
                        {
                            query = query.OrderByDescending(e => e.Email).ToList();
                        }
                    }

                    /*-------Active-----------*/
                    if (SortOrder == "asc")
                    {
                        if (SortCol == "3")
                        {
                            query = query.OrderBy(e => e.IsActive).ToList();
                        }
                    }
                    else
                    {
                        if (SortCol == "3")
                        {
                            query = query.OrderByDescending(e => e.IsActive).ToList();
                        }
                    }

                    #endregion

                    foreach (var userData in query)
                    {
                        AdminUserModel auserModel = new AdminUserModel();
                        {
                            auserModel.UserId = userData.UserId;
                            auserModel.Username = userData.Username;
                            auserModel.Name = userData.Name;
                            auserModel.RoleName = userData.RoleName;
                            auserModel.Password = userData.Password;
                            auserModel.Email = userData.Email;
                            auserModel.IsActive = userData.IsActive;
                            auserModel.RoleId = userData.RoleId;
                            adminUsersList.Add(auserModel);
                        };
                    }
                }
                return adminUsersList;
             
        }
        #endregion

        #region Add/Edit Users
        public bool SaveUser(AdminUserModel users)
        {
            
                var aUserdata = db.AdminPanelUserMasters.Find(users.UserId);
                var aUserTable = new AdminPanelUserMaster();
                if (aUserdata == null)
                {
                    aUserTable.Email = users.Email;
                    aUserTable.Name = users.Name;
                    aUserTable.Password = users.Password;
                    aUserTable.Username = users.Username;
                    aUserTable.RoleId = users.RoleId;
                    aUserTable.ImagePath = users.Image;
                    aUserTable.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    aUserTable.CreatedOn = DateTime.Now;
                    aUserTable.IsDeleted = false;
                    aUserTable.IsActive = true;
                    aUserTable.ModifiedDate = DateTime.Now;
                    aUserTable.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    db.AdminPanelUserMasters.Add(aUserTable);
                }
                else
                {
                    db.Entry(aUserdata).State = EntityState.Modified;
                    aUserdata.ModifiedDate = DateTime.Now;
                    aUserdata.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    aUserdata.Email = users.Email.Trim();
                    aUserdata.Name = users.Name.Trim();
                    aUserdata.Password = users.Password;
                    aUserdata.Username = users.Username;
                    aUserdata.RoleId = users.RoleId;
                    aUserdata.ImagePath = users.Image;
                }
                db.SaveChanges();
                return true;
             
        }
        #endregion

        #region Activate/Deactive User
        public string SetActiveDeactive(int userId)
        {
            
                var data = db.AdminPanelUserMasters.Find(userId);
                string isactiveMsg = (data.IsActive ? "deactivated" : "activated");
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
                return isactiveMsg;
            

        }
        #endregion

        #region Delete User
        public bool DeleteUser(int userId)
        {
            
                var db = new HospitalTransparencyEntities();
                var userData = db.AdminPanelUserMasters.Find(userId);
                if (userData != null)
                {
                    userData.ModifiedDate = DateTime.Now;
                    userData.IsDeleted = true;
                    userData.IsActive = false;
                    userData.DeletedDate = DateTime.Now;
                    userData.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    db.Entry(userData).CurrentValues.SetValues(userData);
                    db.SaveChanges();
                }

                return true;
             
        }
        #endregion

        #region Delete All Users
        public bool DeleteAllUser(string userIds)
        {
            
                var db = new HospitalTransparencyEntities();
                foreach (var item in userIds.Split(','))
                {
                    var userId = Convert.ToInt64(item);
                    var userData = db.AdminPanelUserMasters.Find(userId);
                    if (userData != null)
                    {
                        userData.ModifiedDate = DateTime.Now;
                        userData.IsDeleted = true;
                        userData.IsActive = false;
                        userData.DeletedDate = DateTime.Now;
                        userData.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                        db.Entry(userData).CurrentValues.SetValues(userData);
                    }
                }
                db.SaveChanges();
                return true;
             
        }
        #endregion

        #region GetUserById
        public AdminUserModel GetUserById(int userId)
        {
            
                var db = new HospitalTransparencyEntities();

                IList<AdminUserModel> details = new List<AdminUserModel>();
                var data = (from m in db.AdminPanelUserMasters
                            where m.UserId == userId
                            select new
                            {
                                m.Email,
                                m.UserId,
                                m.IsActive,
                                m.Name,
                                m.Password,
                                m.Username,
                                m.CreatedOn,
                                m.RoleId,
                                m.ImagePath
                            }).FirstOrDefault();

                var userData = new AdminUserModel();
                {
                    userData.UserId = data.UserId;
                    userData.Email = data.Email;
                    userData.IsActive = data.IsActive;
                    userData.Name = data.Name;
                    userData.Password = data.Password;
                    userData.Username = data.Username;
                    userData.RoleId = data.RoleId;
                    userData.Image = data.ImagePath;
                }
                return userData;
             
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
                    db.Dispose();
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