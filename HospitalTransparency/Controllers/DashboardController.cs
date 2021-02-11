using HospitalTransparency.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalTransparency.Controllers
{
    public class DashboardController : SessionController
    {
        readonly DashboardRepository _repo = new DashboardRepository();
    }
}