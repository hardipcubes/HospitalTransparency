using HospitalTransparency.DataAccess;
using HospitalTransparency.Models.Interface;
using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static HospitalTransparency.Helper.CommonStaticMethod;

namespace HospitalTransparency.Models.Repository
{
    public class DashboardRepository
    {
        readonly DashboardDAL DAL = new DashboardDAL();
    }
}