using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace VacationAPI.Controllers
{
    public class VacationServiceController : Controller
    {
        [Authorize]
        [HttpGet("api/upcoming")]
        public IActionResult GetUpcomingVacations()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Upn).Value;
            if (userId.Equals("john@example.com", StringComparison.OrdinalIgnoreCase))
                return Json(new List<Vacation>
                {
                    new Vacation
                    {
                        From = new DateTime(2018, 5, 5),
                        To = new DateTime(2018, 5, 15),
                        Comments = "Vacation in Europe",
                        AppliedOn = new DateTime(2018, 1, 15),
                        IsApproved = true,
                        ApprovedOn = new DateTime(2018, 2, 15)
                    },
                    new Vacation
                    {
                        From = new DateTime(2018, 7, 5),
                        To = new DateTime(2018, 7, 10),
                        Comments = "Secret location",
                        AppliedOn = new DateTime(2018, 1, 15),
                        IsApproved = true,
                        ApprovedOn = new DateTime(2018, 2, 15)
                    }
                });
            if (userId.Equals("jane@example.com", StringComparison.OrdinalIgnoreCase))
                return Json(new List<Vacation>
                {
                    new Vacation
                    {
                        From = new DateTime(2018, 4, 15),
                        To = new DateTime(2018, 4, 25),
                        Comments = "Asia trip",
                        AppliedOn = new DateTime(2018, 2, 10),
                        IsApproved = true,
                        ApprovedOn = new DateTime(2018, 2, 20)
                    }
                });
            return Json(new List<Vacation>());
        }
    }
}