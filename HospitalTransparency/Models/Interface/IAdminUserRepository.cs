using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalTransparency.Models.Interface
{
    public interface IAdminUserRepository
    {
        IEnumerable<AdminUserModel> GetUsersList(int? page, string Name, string RoleName, string Email, string searchfilter, string SortCol, string SortOrder);
        bool SaveUser(AdminUserModel users);
        string SetActiveDeactive(int userId);
        bool DeleteUser(int userId);
        bool DeleteAllUser(string userIds);
        AdminUserModel GetUserById(int userId);
    }
}
