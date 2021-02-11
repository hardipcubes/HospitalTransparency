using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Model
{
    public class RightsModel
    {
        public string RightsId { get; set; }
        public int? RoleId { get; set; }
        public int? MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuType { get; set; }

        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? Display { get; set; }
        public bool? disAdd { get; set; }
        public bool? disEdit { get; set; }
        public bool? disDelete { get; set; }
        public bool? disDisplay { get; set; }

        public string UserId { get; set; }
        public string ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? ParentId { get; set; }
        public string PageRightsFor { get; set; }
        public string MenuDescription { get; set; }
    }

    public class DisplayMenuModel
    {
        public int SrNo { get; set; }
        public string MenuType { get; set; }
        public int RoleId { get; set; }
        public int ParentId { get; set; }
        public int MenuLevel { get; set; }
        public string ParentIdList { get; set; }
        public string ControllerName { get; set; }
        public int? MenuId { get; set; }
        public string MenuIcon { get; set; }
        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public int OrderBy { get; set; }
        public IEnumerable<DisplayMenuModel> Children { get; set; }
    }

    public enum RoleId
    {
        SuperAdmin = 1,
        Default = 2
    }
}