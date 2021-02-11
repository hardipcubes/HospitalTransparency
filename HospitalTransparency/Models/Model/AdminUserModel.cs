using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Model
{
    public class AdminUserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string Image { get; set; }
    }
}