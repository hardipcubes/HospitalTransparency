using HospitalTransparency.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HospitalTransparency.DataAccess
{
    public class RightsDAL
    {
        public DataTable GetRightsMenuList()
        {
            return new DatabaseHelper().DataTable("Sp_MenuRightsList");
        }
    }
}