using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalTransparency.Models.Interface
{
    public interface IMenuRepository
    {
        IEnumerable<MenuModel> GetMenuList(int? page, string menuName, string controllerName, string actionName, string orderBy, string searchfilter, string sortCol, string sortOrder);
        bool SaveMenu(MenuModel menus);
        string SetActiveDeactive(int menuId);
        bool DeleteMenu(int menuId);
        bool DeleteAllMenu(string menuIds);
        MenuModel GetMenuDataById(int menuId);
    }
}
