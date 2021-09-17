using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class MemberTaskSubmission
    {
        //int TaskID, int TeamID, int sessionID, int activityTypeID, int TaskSubmissionID

        public int TaskID { get; set; }
        public int SessionID { get; set; }
        public int ProcessID { get; set; }
        public int TeamID { get; set; }

       public int TaskStatusID { get; set; }

        public int ActivityTypeID { get; set; }

        public int TaskSubmissionID { get; set; }

        public string Comment { get; set; }
        public string URL { get; set; }

        public int MemberID { get; set; }

        public int Rate { get; set; }

        public bool IsSubmitted { get; set; }

        public bool IsMentor { get; set; }
        public string Text { get; set; }
    }
}
