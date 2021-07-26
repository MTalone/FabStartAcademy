using FabStartAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                var x = 1;
            }
            var account = Models.Account.Account.GetAccountSession(HttpContext.Session, User.Identity.Name);
            if (account.IsSuperAdmin||account.IsAdmin) 
            {
                return RedirectToActionPermanent(Actions.Dashboard.Name,Actions.Dashboard.Controller);
            }

            if (account.IsUser) 
            {
                return RedirectToActionPermanent(Models.Controllers.Member.Actions.Dashboard.Name, Models.Controllers.Member.Controller);
            }

            if (account.IsMentor)
            {
                return RedirectToActionPermanent(Models.Controllers.Mentor.Actions.Dashboard.Name, Models.Controllers.Mentor.Controller);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
