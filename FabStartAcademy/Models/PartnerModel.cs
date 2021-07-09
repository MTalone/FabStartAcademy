using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class PartnerModel
    {
        public FBAData.Partner Partner { get; set; }
        public bool ReadOnly { get; set; }

        public MemberItem Member { get; set; }

        public List<MemberItem> Members { get; set; }

        public bool IsSuperAdmin { get; set; }
    }
}
