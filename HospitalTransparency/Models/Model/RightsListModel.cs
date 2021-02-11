using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Model
{
    public class RightsListModel
    {
        public int? ParentId { get; set; }
        public string ControllerName { get; set; }
        public int? MenuId { get; set; }
        public string MenuIcon { get; set; }
        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public string MenuType { get; set; }
        public IEnumerable<RightsListModel> Children { get; set; }
    }

    public class MenuList
    {
        public int MenuId { get; set; }
        public int RightsId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int OrderBy { get; set; }
        public string MenuIcon { get; set; }
        public int ParentId { get; set; }
        public int MenuLevel { get; set; }
        public int RoleId { get; set; }
        public string ParentIdList { get; set; }

        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Display { get; set; }

    }
}