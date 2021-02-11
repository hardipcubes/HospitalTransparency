using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using static HospitalTransparency.Helper.CommonStaticMethod;
using HospitalTransparency.DataAccess;
using HospitalTransparency.App_Start;
using System.Reflection;

namespace HospitalTransparency.Models.Repository
{
    public class RightsListRepository : IRightsListRepository, IDisposable
    {
        MenuDAL DAL = new MenuDAL();
        #region Get Rights List
        public IEnumerable<RightsListModel> GetRightsList()
        {

            List<RightsListModel> pageDetails = new List<RightsListModel>();
            using (var db = new HospitalTransparencyEntities())
            {
                DataTable dt = new DataTable();
                int roleId = Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());
                List<MenuList> myQuery = CustomCacheManagement.MenuList.Where(m => m.RoleId == roleId).OrderBy(m => m.ParentId).ThenBy(m => m.OrderBy).ToList();
                dt = ToDataTable<MenuList>(myQuery);
                pageDetails = GetTopLevelRows(dt)
                .Select(row => CreateItem(dt, row))
                .ToList();
                

                var controller = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values["controller"]).ToLower();
                //if (controller.ToLower() != "widgetrights")
                //{
                //    controller = controller.Replace("Home", "Dashboard").Replace("AdminUser", "AdminPanelUser").Replace("Rights", "RightsMaster").ToLower();
                //}
                var actionName = myQuery.Where(m => m.ControllerName == controller).ToList().FirstOrDefault();
                var action = "";
                if (actionName != null)
                {
                    action = actionName.ActionName.ToLower();
                }

                var roleQuery = myQuery.Where( m=> m.ActionName == action && m.ControllerName == controller).FirstOrDefault();

                if (roleQuery != null)
                {
                    HttpContext.Current.Session["RightAdd"] = roleQuery.Add;
                    HttpContext.Current.Session["RightEdit"] = roleQuery.Edit;
                    HttpContext.Current.Session["RightDelete"] = roleQuery.Delete;
                    HttpContext.Current.Session["RightDisplay"] = roleQuery.Display;
                    HttpContext.Current.Session["MenuId"] = roleQuery.MenuId;
                    HttpContext.Current.Session["ParentMenuId"] = roleQuery.ParentIdList;
                }

                return pageDetails;
            }


        }
        #endregion

        #region Menu Bind

        public List<MenuList> GetMenuList()
        {

            List<MenuList> pageDetails = new List<MenuList>();
            DataTable dt = new DataTable();
            dt = DAL.GetMenuList();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in (dt.Rows))
                {
                    MenuList mdl = new MenuList();
                    {
                        mdl.MenuId = SafeValue<int>(row["MenuId"]);
                        mdl.RightsId = SafeValue<int>(row["RightsId"]);
                        mdl.MenuName = SafeValue<string>(row["MenuName"]);
                        mdl.ControllerName = SafeValue<string>(row["ControllerName"]).ToLower();
                        mdl.ActionName = SafeValue<string>(row["ActionName"]).ToLower();
                        mdl.OrderBy = SafeValue<int>(row["OrderBy"]);
                        mdl.MenuIcon = SafeValue<string>(row["IconClass"]);
                        mdl.ParentId = SafeValue<int>(row["ParentId"]);
                        mdl.MenuLevel = SafeValue<int>(row["MenuLevel"]);
                        mdl.RoleId = SafeValue<int>(row["RoleId"]);
                        mdl.ParentIdList = SafeValue<string>(row["ParentIdList"]);
                        mdl.Add = SafeValue<bool>(row["Add"]);
                        mdl.Edit = SafeValue<bool>(row["Edit"]);
                        mdl.Delete = SafeValue<bool>(row["Delete"]);
                        mdl.Display = SafeValue<bool>(row["Display"]);
                        pageDetails.Add(mdl);
                    };
                }
            }

            return pageDetails;
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
        private RightsListModel CreateItem(DataTable dataTable, DataRow row)
        {
            var id = SafeValue<int>(row.Field<Int32>("MenuId"));
            var children = GetChildren(dataTable, id)
              .Select(r => CreateItem(dataTable, r))
              .ToList();
            return new RightsListModel
            {
                MenuId = id,
                MenuIcon = SafeValue<string>(row["MenuIcon"]),
                ParentId = SafeValue<int>(row["ParentId"]),
                ActionName = !string.IsNullOrEmpty(SafeValue<string>(row["ActionName"])) ? SafeValue<string>(row["ActionName"]).Replace("Dashboard", "Index").Replace("AdminPanelUser", "AdminUser").Replace("RightsMaster", "Rights") : null,
                ControllerName = !string.IsNullOrEmpty(SafeValue<string>(row["ControllerName"])) ? SafeValue<string>(row["ControllerName"]).Replace("Dashboard", "Home").Replace("AdminPanelUser", "AdminUser").Replace("RightsMaster", "Rights") : null,
                MenuName = SafeValue<string>(row["MenuName"]),
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
        #endregion

        #region Private Function
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
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