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
    }
}
