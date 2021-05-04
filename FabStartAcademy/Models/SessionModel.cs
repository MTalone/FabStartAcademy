using FBAData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class SessionItem :ProcessRelated
    {
       
       
        public int NumberSubtopics { get; set; }

        public bool IsReadOnly { get; set; }

        public string ProgramTitle { get; set; }

        public int Order { get; set; }

    }
    public class SessionModel
    {
        public string ProcessTitle { get; set; }
        public List<SessionItem> Sessions{ get; set; }

        public int ProcessID { get; set; }


        public SessionModel(int programID) 
        {
            ProcessTitle = FBAData.Process.GetProcess(programID).Name;
            Sessions = Session.GetSessions(programID).Select(x=>new SessionItem {Title=x.Name,NumberSubtopics=x.NumberTasks,ID=x.ID,ProcessID=x.ProcessID,Order=x.Order }).ToList() ;
            ProcessID = programID;
        }
    }
}
