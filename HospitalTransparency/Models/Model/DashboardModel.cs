using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalTransparency.Models.Model
{

	public class SpecialityChartModel
	{
		public SpecialityChartModel()
		{
			CurrentYear = new List<SpecialityPropModel>();
			PreviousYear = new List<SpecialityPropModel>();
		}
		public List<SpecialityPropModel> CurrentYear { get; set; }
		public List<SpecialityPropModel> PreviousYear { get; set; }
	}

	public class SpecialityPropModel:ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();

		}

	}

	public class SpecialitySummaryModel
	{
		public string SpecialityName { get; set; }
		public string TitleClass { get; set; }
		public List<SummaryModel> lstSummary { get; set; }
	}

	public class SummaryModel
	{
		public string TitlePreviousYear { get; set; }
		public string TitleCurrentYear { get; set; }

		public int PreviousYear { get; set; }
		
		public int CurrentYear { get; set; }
		
		public string Variance { get; set; }
		public string Difference { get; set; }
		public string ClassName { get; set; }
	}

	public class OBSChartModel
	{
		public OBSChartModel()
		{
			CurrentYear = new List<OBSPropModel>();
			PreviousYear = new List<OBSPropModel>();
		}
		public List<OBSPropModel> CurrentYear { get; set; }
		public List<OBSPropModel> PreviousYear { get; set; }
	}

	public class OBSPropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class OPRegistrationsChartModel
	{
		public OPRegistrationsChartModel()
		{
			CurrentYear = new List<OPRegistrationsPropModel>();
			PreviousYear = new List<OPRegistrationsPropModel>();
		}
		public List<OPRegistrationsPropModel> CurrentYear { get; set; }
		public List<OPRegistrationsPropModel> PreviousYear { get; set; }
	}

	public class OPRegistrationsPropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class InPatientSurgeriesChartModel
	{
		public InPatientSurgeriesChartModel()
		{
			CurrentYear = new List<InPatientSurgeriesPropModel>();
			PreviousYear = new List<InPatientSurgeriesPropModel>();
		}
		public List<InPatientSurgeriesPropModel> CurrentYear { get; set; }
		public List<InPatientSurgeriesPropModel> PreviousYear { get; set; }
	}

	public class InPatientSurgeriesPropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class OPCTChartModel
	{
		public OPCTChartModel()
		{
			CurrentYear = new List<OPCTPropModel>();
			PreviousYear = new List<OPCTPropModel>();
		}
		public List<OPCTPropModel> CurrentYear { get; set; }
		public List<OPCTPropModel> PreviousYear { get; set; }
	}

	public class OPCTPropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class OPMRIChartModel
	{
		public OPMRIChartModel()
		{
			CurrentYear = new List<OPMRIPropModel>();
			PreviousYear = new List<OPMRIPropModel>();
		}
		public List<OPMRIPropModel> CurrentYear { get; set; }
		public List<OPMRIPropModel> PreviousYear { get; set; }
	}

	public class OPMRIPropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public class TotalRevenueChartModel
	{
		public TotalRevenueChartModel()
		{
			CurrentYear = new List<TotalRevenuePropModel>();
			PreviousYear = new List<TotalRevenuePropModel>();
		}
		public List<TotalRevenuePropModel> CurrentYear { get; set; }
		public List<TotalRevenuePropModel> PreviousYear { get; set; }
	}

	public class TotalRevenuePropModel : ICloneable
	{
		public int HospitalId { get; set; }
		public string HospitalName { get; set; }
		public string SpecialityName { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Count { get; set; }
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}


	public class NegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
		
	}

	public class ObservationNegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class OPRegistrationsNegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class InPatientSurgeriesNegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class OPCTNegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class OPMRINegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class TotalRevenueNegativeChartModel
	{
		public string Name { get; set; }
		public int CurrentYear { get; set; }
		public int PreviousYear { get; set; }
		public int CountDifference { get; set; }
	}

	public class KeyValuePair
	{
		public string Key { get; set; }
		public int Value { get; set; }

	}
}