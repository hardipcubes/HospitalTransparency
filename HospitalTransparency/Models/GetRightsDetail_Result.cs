//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HospitalTransparency.Models
{
    using System;
    
    public partial class GetRightsDetail_Result
    {
        public Nullable<int> RightsId { get; set; }
        public Nullable<bool> Add { get; set; }
        public Nullable<bool> Edit { get; set; }
        public Nullable<bool> Delete { get; set; }
        public Nullable<bool> Display { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public string IconClass { get; set; }
        public int ParentId { get; set; }
        public string ParentIdList { get; set; }
        public Nullable<int> MenuLevel { get; set; }
        public Nullable<int> RoleId { get; set; }
    }
}
