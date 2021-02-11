using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Models.Model
{
    public class MenuModel
    {
        public int MenuId { get; set; }
        public string MenusName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? ParentId { get; set; }
        public string MenuType { get; set; }
        public int? OrderBy { get; set; }
        public int? MenuIcon { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public List<SelectListItem> MenuList { get; set; }
        public string Iconname { get; set; }
    }
}