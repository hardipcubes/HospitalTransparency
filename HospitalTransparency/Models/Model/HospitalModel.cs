using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Model
{
    public class HospitalModel
    {
		public long HospitalId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string ContactNo { get; set; }
		public int CreatedBy { get; set; }
		public string CreatedOn { get; set; }
		public int ModifiedBy { get; set; }
		public string ModifiedOn { get; set; }
		public bool IsDeleted { get; set; }
		public string DeletedDate { get; set; }
		public bool IsActive { get; set; }
	}
}