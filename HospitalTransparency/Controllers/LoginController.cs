using HospitalTransparency.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalTransparency.Helper;
using HospitalTransparency.Models.Model;

namespace HospitalTransparency.Controllers
{
    public class LoginController : SessionController
    {
        readonly HospitalTransparencyEntities db = new HospitalTransparencyEntities();

        #region Login
        public ActionResult Login()
        {
            var userId = CheckForCookieUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var status = CheckValidUserId(Convert.ToInt32(userId));
                if (status)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, string hdnrememberMe)
        {
            int userId = CheckValidUser(userName, password);
            if (userId != 0)
            {
                RemoveRememberMeCookie();
                if (hdnrememberMe == "true")
                {
                    if (string.IsNullOrEmpty(CheckForCookieUserId()))
                    {
                        CreateRememberMeCookie(userId);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter valid Username and Password.";
                return View("Login");
            }
        }

        public int CheckValidUser(string userName, string password)
        {
            int userId = 0;
            var UserList = (from u in db.AdminPanelUserMasters
                            join r in db.RoleMasters on u.RoleId equals r.RoleId
                            where u.Username == userName && u.Password == password && u.IsActive == true && u.IsDeleted == false
                            select new { u.CreatedBy, u.Name, u.UserId, u.Username, u.RoleId, r.RoleName, u.ImagePath }).FirstOrDefault();

            if (UserList != null)
            {
                Session["RoleId"] = UserList.RoleId;
                Session["UserId"] = UserList.UserId;
                Session["UserName"] = !string.IsNullOrEmpty(UserList.Username) ? UserList.Username : "";
                Session["Name"] = !string.IsNullOrEmpty(UserList.Name) ? UserList.Name : "User";
                Session["Role"] = !string.IsNullOrEmpty(UserList.RoleName) ? UserList.RoleName : "";
                Session["Image"] = !string.IsNullOrEmpty(UserList.ImagePath) ? UserList.ImagePath : "..\\Content\\images\\User.png";
                userId = UserList.UserId;
                GetApplicationRights();
            }
            return userId;
        }
        #endregion

        #region Lock
        public ActionResult Lock(int? userId)
        {
            if (userId != null)
            {
                var UserList = (from u in db.AdminPanelUserMasters
                                where u.UserId == userId && u.IsActive == true && u.IsDeleted == false
                                select new { u.CreatedBy, u.Name, u.UserId, u.Username, u.RoleId, u.ImagePath }).FirstOrDefault();
                if (UserList != null)
                {
                    ViewBag.Name = !string.IsNullOrEmpty(UserList.Name) ? UserList.Name : "User";
                    ViewBag.UserName = !string.IsNullOrEmpty(UserList.Username) ? UserList.Username : "";
                    ViewBag.ImagePath = !string.IsNullOrEmpty(UserList.ImagePath) ? UserList.ImagePath : "..\\Content\\images\\User.png";
                }
                else
                {
                    ViewBag.Name = "";
                    ViewBag.UserName = "";
                    ViewBag.ImagePath = "";
                }
                ViewBag.UserId = userId;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult Lock(string userName, string password, int uId)
        {
            int userId = CheckValidUser(userName, password);
            if (userId != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter valid password.";

                var UserList = (from u in db.AdminPanelUserMasters
                                where u.UserId == uId && u.IsActive == true && u.IsDeleted == false
                                select new { u.CreatedBy, u.Name, u.UserId, u.Username, u.RoleId, u.ImagePath }).FirstOrDefault();
                if (UserList != null)
                {
                    ViewBag.Name = !string.IsNullOrEmpty(UserList.Name) ? UserList.Name : "User";
                    ViewBag.UserName = !string.IsNullOrEmpty(UserList.Username) ? UserList.Username : "";
                    ViewBag.ImagePath = !string.IsNullOrEmpty(UserList.ImagePath) ? UserList.ImagePath : "..\\Content\\images\\User.png";
                }
                else
                {
                    ViewBag.Name = "";
                    ViewBag.UserName = "";
                    ViewBag.ImagePath = "";
                }
                ViewBag.UserId = uId;
                return View("Lock");
                //return RedirectToAction("Lock?userId=" + uId, "Login");
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            RemoveRememberMeCookie();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
        #endregion

        #region Direct Login
        public ActionResult DirectLogin(string userName, string password)
        {
            int userId = CheckValidUser(userName, password);
            if (userId != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Please enter valid Username and Password.";
                return View("Login");
            }
        }
        #endregion

        #region Remember Me
        public bool CheckValidUserId(int userId)
        {
            var status = false;
            var UserList = (from u in db.AdminPanelUserMasters
                            join r in db.RoleMasters on u.RoleId equals r.RoleId
                            where u.UserId == userId && u.IsActive == true && u.IsDeleted == false
                            select new { u.CreatedBy, u.Name, u.UserId, u.Username, u.RoleId, r.RoleName, u.ImagePath }).FirstOrDefault();

            if (UserList != null)
            {
                Session["RoleId"] = UserList.RoleId;
                Session["UserId"] = UserList.UserId;
                Session["UserName"] = !string.IsNullOrEmpty(UserList.Username) ? UserList.Username : "";
                Session["Name"] = !string.IsNullOrEmpty(UserList.Name) ? UserList.Name : "";
                Session["Role"] = !string.IsNullOrEmpty(UserList.RoleName) ? UserList.RoleName : "";
                Session["Image"] = !string.IsNullOrEmpty(UserList.ImagePath) ? UserList.ImagePath : "..\\Content\\images\\User.png";
                status = true;
                GetApplicationRights();
            }
            return status;
        }

        private string CheckForCookieUserId()
        {
            string returnValue = string.Empty;
            HttpCookie rememberMeUserNameCookie = Request.Cookies.Get(rememberMeCookie);
            if (null != rememberMeUserNameCookie)
            {
                returnValue = rememberMeUserNameCookie.Value;
            }
            return returnValue;
        }

        private void CreateRememberMeCookie(int userId)
        {
            HttpCookie myCookie = new HttpCookie(rememberMeCookie, userId.ToString());
            myCookie.Expires = DateTime.MaxValue;
            Response.SetCookie(myCookie);
        }

        private void RemoveRememberMeCookie()
        {
            HttpCookie rememberMeUserNameCookie = Request.Cookies[rememberMeCookie];
            if (null != rememberMeUserNameCookie)
            {
                Response.Cookies.Remove(rememberMeCookie);
                rememberMeUserNameCookie.Expires = DateTime.Now.AddYears(-1);
                rememberMeUserNameCookie.Value = null;
                Response.SetCookie(rememberMeUserNameCookie);
            }
        }

        private const string rememberMeCookie = "rUserId";
        #endregion

        #region Forgot Password
        public JsonResult forgotPassword(string emailId)
        {
            var status = "";
            
                var userData = (from u in db.AdminPanelUserMasters
                                where u.Email == emailId && u.IsActive == true && u.IsDeleted == false
                                select new { u.UserId, u.Username }).FirstOrDefault();

                if (userData != null)
                {
                    string msgBody = string.Empty;
                    var settingsReader = new AppSettingsReader();
                    var url = (string)settingsReader.GetValue("Url", typeof(String));

                    var incUserId = General.RandomString(50) + '_' + Cryptography.Encrypt(userData.UserId.ToString(), true);
                    var resetLink = url + "Login/Reset?key=" + incUserId;

                    var logoPath = Server.MapPath("~/Content/images/logo.png");
                    var base64Path = General.ConvertImageToBase64(logoPath);

                    msgBody = System.IO.File.ReadAllText(HttpContext.Request.PhysicalApplicationPath + "\\Template\\ResetPassword.html");
                    msgBody = msgBody.Replace("@@UserName", !string.IsNullOrEmpty(userData.Username) ? userData.Username : "User");
                    msgBody = msgBody.Replace("@@ResetLink", resetLink);
                    msgBody = msgBody.Replace("@@Logo", !string.IsNullOrEmpty(base64Path) ? base64Path : url + "Content/images/logo.png");
                    Task.Factory.StartNew(() => SendEmail.Email(emailId, "Forgot Password", msgBody));
                    status = "Your password reset link has been sent successfully.";
                }
                else
                {
                    status = "Please enter valid email-Id.";
                }
             

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reset Password
        public ActionResult Reset(string key)
        {
             
                var userId = Convert.ToInt32(Cryptography.Decrypt(key.Split('_')[1], true));
                if (userId != null)
                {
                    var UserList = (from u in db.AdminPanelUserMasters
                                    where u.UserId == userId && u.IsActive == true && u.IsDeleted == false
                                    select new { u.CreatedBy, u.Name, u.UserId, u.Username, u.RoleId, u.ImagePath }).FirstOrDefault();
                    if (UserList != null)
                    {
                        ViewBag.Name = !string.IsNullOrEmpty(UserList.Name) ? UserList.Name : "User";
                        ViewBag.UserName = !string.IsNullOrEmpty(UserList.Username) ? UserList.Username : "";
                        ViewBag.ImagePath = !string.IsNullOrEmpty(UserList.ImagePath) ? UserList.ImagePath : "..\\Content\\images\\User.png";
                    }
                    else
                    {
                        ViewBag.Name = "";
                        ViewBag.UserName = "";
                        ViewBag.ImagePath = "";
                    }
                    ViewBag.UserId = userId;
                    return View();
                }
             
            return RedirectToAction("Login", "Login");
        }

        public JsonResult ResetPassword(int userId, string password)
        {
            var status = "0";
            
                var userData = db.AdminPanelUserMasters.Find(userId);
                if (userData != null)
                {
                    userData.ModifiedDate = DateTime.Now;
                    userData.Password = password;
                    userData.ModifiedBy = Convert.ToInt32(Session["UserId"]);
                    db.Entry(userData).CurrentValues.SetValues(userData);
                    db.SaveChanges();
                    status = "1";
                }
             
            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private void GetApplicationRights()
        {
            #region Check application found for logged in user
            ApplicationParamModel param = new ApplicationParamModel();
            param.UserName = Convert.ToString(Session["UserName"]);
            var modelData = CallAPI<APIResponseBase<List<ApplicationResultModel>>, ApplicationParamModel>($"/api/Application/Get", "POST", param).Result;

            Session["ApplicationList"] = null; //Session value null before set
            if (modelData != null && Session["ApplicationList"] == null && modelData.StatusCode == 200 && modelData.Result != null)
            {
                Session.Add("ApplicationList", modelData.Result.Where(m => m.ApplicationCode != "hldht").ToList());
            }
            else
            {
                Session.Add("ApplicationList", null);
            }
            #endregion
        }
    }
}