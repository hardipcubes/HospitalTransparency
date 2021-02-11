using HospitalTransparency.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalTransparency.App_Start
{
    public static class CustomCacheManagement
    {
        public static List<MenuList> MenuList
        {
            get { return HttpRuntime.Cache["MenuList"] as List<MenuList>; }
            set { HttpRuntime.Cache["MenuList"] = value; }
        }

    }
}