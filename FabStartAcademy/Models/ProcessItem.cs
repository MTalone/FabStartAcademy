using FBAData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class ProcessItem:Item
    {
        

        public bool IsReadOnly { get; set; }

       public int? LogoID { get; set; }
        
        public byte[] Logo { get; set; }
      
        public int SessionCount { get; set; }
    }

    public class ProgramModel 
    { 
        public List<ProcessItem> Programs { get; set; }
        

        public ProgramModel(int max) 
        {
            var p = FBAData.Process.GetProcesses(max);
            Programs = p.Select(x => new ProcessItem { Title = x.Name, Description = x.Description, ID = x.ID, SessionCount=x.SessionCount }).ToList();

        }
        public ProgramModel(){}

        
        
    }

    public class ProcessBreadCrumbAction 
    {
        public Action Action { get; set; }
        public int ProgramID { get; set; }

        public int SessionID { get; set; }

        public string Session { get; set; }
    }
}
