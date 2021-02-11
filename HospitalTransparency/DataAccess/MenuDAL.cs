using HospitalTransparency.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HospitalTransparency.DataAccess
{
    public class MenuDAL
    {
        //public DataTable GetMenuList(int RoleId)
        //{
        //    SqlParameter[] sqlParam = new SqlParameter[]
        //    {
        //            new SqlParameter() {ParameterName = "@RoleId",Value= RoleId }
        //     };
        //    return new DatabaseHelper().DataTable("GetRightsDetail_v1", sqlParam);
        //}

        public DataTable GetMenuList()
        {
            return new DatabaseHelper().DataTable("GetRightsDetail");
        }
    }
}