using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalTransparency.Models.Interface
{
    public interface IRoleRepository
    {
        IEnumerable<RoleModel> GetRoleList(int? page, string Name, string Description, string searchfilter, string SortCol, string SortOrder);
        RoleModel GetRoleById(int roleId);
        bool SaveRole(RoleModel role);
        bool DeleteRole(int roleId);
        bool DeleteAllRole(string roleIds);
        string SetActiveDeactive(int roleId);
    }
}
