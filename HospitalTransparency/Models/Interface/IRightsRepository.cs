using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalTransparency.Models.Interface
{
    public interface IRightsRepository
    {
        //IEnumerable<RightsModel> GetRightsDetails(string userId);
        IEnumerable<RightsModel> GetRightsByRoleId(int roleId);
        bool AddRights(RightsModel rights);
        bool UpdateRights(RightsMaster rights);
        IEnumerable<RightsModel> MenuList();
        List<DisplayMenuModel> DisplayMenuList();
    }
}
