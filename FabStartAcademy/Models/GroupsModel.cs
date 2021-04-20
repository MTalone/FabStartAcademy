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

        public void GetProgramItems(List<FBAData.Program> ps, string programDefaultPath,string WebRootPath)
        {
            Programs = new List<ProgramItem>();
            foreach (var x in ps)
            {
                ProgramItem pi = new ProgramItem
                {

                    Title = x.Name,

                    Process = x.Process != null ? x.Process.Name : string.Empty,
                    ID = x.ID
                };
                if (!(x.Logo is null))
                {
                    pi.Image = x.Logo.Path;
                    if (!(pi.Image is null))
                    {
                        pi.Image = pi.Image.Replace(WebRootPath, "");
                    }
                }
                if (pi.Image is null | pi.Image == string.Empty)
                {
                    pi.Image = programDefaultPath;
                }
                Programs.Add(pi);
            }
        }
    }
}
