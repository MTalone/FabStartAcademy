using AutoMapper;
using FabStartAcademy.Models.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace FabStartAcademy.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IMapper mapper, UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            AccountRegister model = new AccountRegister();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegister userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            FBAData.Team team =  FBAData.Team.GetByToken(userModel.GroupToken);
            if(team is null) 
            {
                ModelState.TryAddModelError("GroupToken",Resources.FabStartAcademy.InvalidTeam);
                return View(userModel);
            }

            var user = new IdentityUser { Email = userModel.Email,UserName=userModel.Email };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordRequiresUpper") { error.Code = "Password"; }

                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(userModel);
            }

            var member = FBAData.Member.GetByEmail(user.Email);
            if(member is null) 
            {
                member = new FBAData.Member { Email = user.Email };
            }
            member.UserID = user.Id;
            member.FirstName = userModel.FirstName;
            member.LastName = userModel.LastName;
            member.IsUser = true;

            member.ID=FBAData.Member.Save(member);
            var teammember = FBAData.TeamMember.Get(member.ID, team.ID);

            if(teammember is null) 
            {
                teammember = new FBAData.TeamMember {  MemberID = member.ID, TeamID = team.ID, RoleID = (int)FBAData.Role.Roles.User };
            }
            teammember.IsConfirmed = true;

            FBAData.TeamMember.Save(teammember);

            //await _userManager.AddToRoleAsync(user, "Visitor");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
             NewMethod();

            AccountLogin model = new AccountLogin();


            return View(model);
        }

        private async Task NewMethod()
        {
            try
            {
                if (!FBAData.Partner.CheckMainCreated())
                {

                    FBAData.Partner partner = new FBAData.Partner { Name = "Fábrica de Start Ups", IsMain = true };
                    partner.ID = FBAData.Partner.Save(partner);
                    FBAData.Team team = new FBAData.Team { Name = "FabStart", PartnerID = partner.ID };
                    team.ID = FBAData.Team.Save(team);
                    var user = new IdentityUser { Email = "talk@fabstart.pt", UserName = "talk@fabstart.pt", EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(user, "Fabrica.2020");
                    if (result.Succeeded)
                    {
                        FBAData.Member member = new FBAData.Member { Email = user.Email, IsUser = true, UserID = user.Id, PartnerID = partner.ID };
                        member.ID = FBAData.Member.Save(member);
                        FBAData.TeamMember teamMember = new FBAData.TeamMember { IsConfirmed = true, MemberID = member.ID, TeamID = team.ID, RoleID = (int)FBAData.Role.Roles.SuperAdmin };
                        FBAData.TeamMember.Save(teamMember);

                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLogin model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await _userManager.FindByNameAsync(model.Email);


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email,
                           model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {

                FBAData.Member m = FBAData.Member.GetByEmail(model.Email);
                List<FBAData.TeamMember> tms = FBAData.Member.GetTeams(m.ID);

                Account account = new Account { Email = model.Email, UserID = user.Id };

                account.IsAdmin = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.Admin || x.RoleID == (int)FBAData.Role.Roles.SuperAdmin);
                account.IsMentor = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.Mentor);
                account.IsUser = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.User);


                if (string.IsNullOrEmpty(returnUrl))
                {
                    if (account.IsAdmin) 
                    {
                        return RedirectToActionPermanent(Models.Actions.Dashboard.Name, Models.Actions.Dashboard.Controller);
                    }
                    else 
                    {
                        return RedirectToActionPermanent(Models.Actions.Home.Name, Models.Actions.Home.Controller);
                    }

                }
                    
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }



            
        }
    }
}
