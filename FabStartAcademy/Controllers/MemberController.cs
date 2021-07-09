using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class MemberController : Controller
    {
        private IWebHostEnvironment Environment;

        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MemberController(IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment _environment)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            Environment = _environment;
        }
        public IActionResult Dashboard()
        {
            

           List<FBAData.Team> teams = FBAData.MemberFlow.GetTeams(User.Identity.Name);

            return View();
        }

        public IActionResult Task(int TaskID, int TeamID) 
        {
            FBAData.Task task = FBAData.Task.GetTask(TaskID);
            task.Session = FBAData.Session.GetSession(task.SessionID, true);
            FBAData.Team team = FBAData.Team.Get(TeamID, true);


        }
    }
}
