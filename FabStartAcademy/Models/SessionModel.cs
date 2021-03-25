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

        

    }
    public class SessionModel
    {
        public string ProgramTitle { get; set; }
        public List<SessionItem> Sessions{ get; set; }

        public int ProgramID { get; set; }
        public SessionModel(int programID) 
        {
            ProgramTitle = FBAData.Process.GetProcess(programID).Name;
            Sessions = Session.GetSessions(programID).Select(x=>new SessionItem {Title=x.Name,NumberSubtopics=x.NumberTasks,ID=x.ID,ProcessID=x.ProcessID }).ToList() ;
            ProgramID = programID;
        }
    }
}
