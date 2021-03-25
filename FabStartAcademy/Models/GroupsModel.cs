using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class ProgramsModel
    {
        public List<ProgramItem> Programs { get; set; }

        public List<SelectListItem> Processes { get; set;}

        public ProgramItem Program { get; set; }  

        public ProgramsModel() 
        { 
            
        }
    }
}
