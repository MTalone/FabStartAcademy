using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class MemberItem
    {
        public string Email { get; set; }
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public bool IsConfirmed { get; set; }

        
    }
}
