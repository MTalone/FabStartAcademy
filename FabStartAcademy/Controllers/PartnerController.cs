using FabStartAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class PartnerController : Controller
    {
        public IActionResult Partners()
        {

            var account = FabStartAcademy.Models.Account.Account.GetAccountSession(ControllerContext.HttpContext.Session, User.Identity.Name);
            List<FBAData.Partner> partners = FBAData.Partner.List(account.IsSuperAdmin,account.PartnerID);
            return View(partners);
        }

        public IActionResult Partner(int ID, bool readOnly=true)
        {
            readOnly = ID == 0 ? false : readOnly;
            FBAData.Partner partner = FBAData.Partner.Get(ID);
            if (partner is null)
                partner = new FBAData.Partner();
            var account = FabStartAcademy.Models.Account.Account.GetAccountSession(ControllerContext.HttpContext.Session, User.Identity.Name);
            PartnerModel model = new PartnerModel { Partner = partner, ReadOnly = readOnly,Members=new List<MemberItem>(),IsSuperAdmin=account.IsSuperAdmin };

            if (readOnly) 
            { 
                model.Members = FBAData.Partner.Members(ID).Select(x => new MemberItem { Email = x.Member.Email, RoleName = x.Role.Name, RoleID = x.RoleID, IsConfirmed = x.IsConfirmed }).OrderByDescending(x => x.IsConfirmed).ToList();
            }


            return View(model);
        }

        public JsonResult PartnerMethods(int partnerID) 
        {

            var processes =  FBAData.Process.GetProcesses(0,false,partnerID).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();

            return Json(new { data=processes });
        }


        [HttpPost]
        public IActionResult Partner(PartnerModel model) 
        {
            FBAData.Partner.Save(model.Partner);

            return RedirectToActionPermanent(Actions.Partners.Name);
        }

        [HttpPost]
        public IActionResult Suspend(int ID, bool suspend)
        {
            FBAData.Partner partner = FBAData.Partner.Get(ID);
            partner.IsSuspended = suspend;

            FBAData.Partner.Save(partner);

            return RedirectToActionPermanent(Actions.Partner.Name, new { ID = ID });
        }

        [HttpPost]
        public IActionResult Delete(int ID)
        {
            FBAData.Partner partner = FBAData.Partner.Get(ID);
            partner.IsDeleted = true;

            FBAData.Partner.Save(partner);

            return RedirectToActionPermanent(Actions.Partners.Name);
        }


        [HttpPost]
        public IActionResult Member(int partnerID, TeamModel model)
        {
            try
            {
                FBAData.Member m = FBAData.Member.GetByEmail(model.Member.Email);
                FBAData.Partner partner = FBAData.Partner.Get(partnerID);
                var team = FBAData.Team.GetMain(partnerID);

                if (team is null)
                {
                    team = new FBAData.Team { IsMain = true, PartnerID = partnerID, Name = partner.Name, Code = partner.Code };
                    team.ID = FBAData.Team.Save(team);
                }

                int memberid = 0;
                if (m is null || m.ID == 0)
                {
                    //Criar Member
                    memberid = FBAData.Member.Save(new FBAData.Member { Email = model.Member.Email, PartnerID = partnerID });

                }
                else
                {
                    memberid = m.ID;
                }
                //Criar TeamMember;
                // Check if member exists in team
                if (!FBAData.TeamMember.Exists(team.ID, memberid))
                {
                    FBAData.TeamMember.Save(new FBAData.TeamMember { MemberID = memberid, TeamID = team.ID, RoleID = partner.IsMain ? (int)FBAData.Role.Roles.SuperAdmin : (int)FBAData.Role.Roles.Admin });
                }

                var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
                var config = builder.Build();

                var smtpClient = new SmtpClient(config["Smtp:Host"])
                {
                    Port = int.Parse(config["Smtp:Port"]),
                    Credentials = new NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]),
                    EnableSsl = true,
                };
                //Enviar E-mails
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config["Smtp:Username"]),
                    Subject = string.Format("Invitation to join team {0}", team.Name),
                    Body = string.Format("Hello<br/> you are invited to join the team {0}. Please use this link to register, and insert the code {1}", team.Name, config["Constants:CodeBegin"] + team.Code),
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(model.Member.Email);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {

               
            }
            //verificar se Member existe.
           

            return RedirectToActionPermanent(Actions.Partner.Name, new { ID= partnerID});

        }
    }
}
