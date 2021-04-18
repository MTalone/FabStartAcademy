using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class ProgramItem: Item
    {  
        [Display(Name = "Method", ResourceType = typeof(Resources.FabStartAcademy))]
        public string Process { get; set; }

        public string Image { get; set; }

        [Display(Name = "MembersConfirmed", ResourceType = typeof(Resources.FabStartAcademy))]
        public int MembersConfirmed { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resources.FabStartAcademy))]

        public bool IsRead { get; set; }

       

        public string Code { get; set; }

        public int? LogoID { get; set; }
        public int? ProcessID { get; set; }
    }

    public class ProgramBreadCrumb
    {
        public Action Action { get; set; }
        public int ProgramID { get; set; }

        public int TeamID { get; set; }

        public string Team { get; set; }

    }

}
