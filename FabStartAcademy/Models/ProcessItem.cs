using FBAData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class ProcessItem:Item
    {
        
        public bool IsSuperAdmin { get; set; }

        [Display(Name = "Partner", ResourceType = typeof(Resources.FabStartAcademy))]
        public int PartnerID { get; set; }
        public bool IsReadOnly { get; set; }

       public int? LogoID { get; set; }
        
        public byte[] Logo { get; set; }
      
        public int SessionCount { get; set; }

        public List<SelectListItem> Partners { get; set; }
    }

    public class ProgramModel 
    { 
        public List<ProcessItem> Programs { get; set; }
        public Account.Account CurrentAccount { get; set; }

        public ProgramModel(int max,ISession session,string username) 
        {
            CurrentAccount = Account.Account.GetAccountSession(session,username);
            var p = FBAData.Process.GetProcesses(max,CurrentAccount.IsSuperAdmin,CurrentAccount.PartnerID);
            
            Programs = p.Select(x => new ProcessItem { Title = x.Name, Description = x.Description, ID = x.ID, SessionCount=x.SessionCount }).ToList();

        }
        public ProgramModel(){}

        
        
    }

    public class ProcessBreadCrumb
    {
        public Action Action { get; set; }
        public int ProcessID { get; set; }

        public int SessionID { get; set; }

        public string Session { get; set; }
    }
}
