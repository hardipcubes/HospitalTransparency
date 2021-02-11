using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalTransparency.Models.Interface
{
    public interface IRightsListRepository
    {
        IEnumerable<RightsListModel> GetRightsList();
    }
}
