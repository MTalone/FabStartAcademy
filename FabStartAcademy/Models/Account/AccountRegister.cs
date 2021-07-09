using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models.Account
{
    public class AccountRegister
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.FabStartAcademy))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.FabStartAcademy))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName", ResourceType = typeof(Resources.FabStartAcademy))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Member_TeamToken", ResourceType = typeof(Resources.FabStartAcademy))]
        public string GroupToken { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.FabStartAcademy))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.FabStartAcademy))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

    public class AccountLogin 
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.FabStartAcademy))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.FabStartAcademy))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.FabStartAcademy))]
        public bool RememberMe { get; set; }
    }

    public class Account 
    { 
        public string UserID { get; set; }

        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsMentor { get; set; }

        public bool IsUser { get; set; }

        public bool IsSuperAdmin { get; set; }

        public int PartnerID { get; set; }


        public static void SetAccountSession(ISession session,string userName)
        {
            FBAData.Member m = FBAData.Member.GetByEmail(userName);
            List<FBAData.TeamMember> tms = FBAData.Member.GetTeams(m.ID);

            Account account = new Account { Email = userName, UserID = m.UserID };

            account.IsAdmin = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.Admin);
            account.IsSuperAdmin = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.SuperAdmin);
            account.IsMentor = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.Mentor);
            account.IsUser = tms.Any(x => x.RoleID == (int)FBAData.Role.Roles.User);
            account.PartnerID = m.PartnerID;

            session.SetString("UserID", account.UserID);
            session.SetString("IsAdmin", account.IsAdmin.ToString());
            session.SetString("IsMentor", account.IsMentor.ToString());
            session.SetString("IsUser", account.IsUser.ToString());
            session.SetString("IsSuperAdmin", account.IsSuperAdmin.ToString());
            session.SetString("Email", account.Email);
            session.SetInt32("PartnerID", account.PartnerID);

        }

        public static void SetAccountSession(Account account,ISession session) 
        {

            session.SetString("UserID", account.UserID);
            session.SetString("IsAdmin", account.IsAdmin.ToString());
            session.SetString("IsMentor", account.IsMentor.ToString());
            session.SetString("IsUser", account.IsUser.ToString());
            session.SetString("IsSuperAdmin", account.IsSuperAdmin.ToString());
            session.SetString("Email", account.Email);
            session.SetInt32("PartnerID", account.PartnerID);

        }

        internal static Account GetAccountSession(ISession session,string username)
        {
            if(!session.IsAvailable || string.IsNullOrEmpty(session.GetString("Email"))) 
            {
                SetAccountSession(session, username);
            
            }

            Account account = new Account();
            account.Email = session.GetString("Email");
            account.UserID = session.GetString("Email");
            account.IsAdmin = bool.Parse(session.GetString("IsAdmin"));
            account.IsMentor = bool.Parse(session.GetString("IsMentor"));
            account.IsUser = bool.Parse(session.GetString("IsUser"));
            account.IsSuperAdmin = bool.Parse(session.GetString("IsSuperAdmin"));
            int? partnerId = session.GetInt32("PartnerID");
            account.PartnerID = partnerId.HasValue?partnerId.Value:0 ;
            return account;
        }
    }
}
