using FBAData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class MemberFlowTeam
    {
        public FBAData.Team Team { get; set; }
        public string ProgramName { get; set; }

        public string MethodName { get; set; }

        public string LogoPath { get; set; }
       public List<MemberFlowSession> Sessions { get; set; }
        public List<Document> ProcessDocuments { get;  set; }
        public List<TaskSubmissionLink> ProcessLinks { get;  set; }
    }

    public class MemberFlowTaskItem 
    {
        public FBAData.Task Task { get; set; }

        public int TeamID { get; set; }
        public int TaskSubmissionID { get; set; }

        public bool IsSubmitted { get; set; }

        public int Rate { get; set; }

        public string RateDisplay 
        {
            get
            {
                if (!Task.IsEvaluated) { return "N/A"; }

                if (Rate > 0) { return string.Empty; }

                return "-";
            }
        }
      

        public FBAData.Process Process { get; set; }

        public string Status { get {

                if (TaskSubmissionID == 0) 
                {
                    return Resources.FabStartAcademy.TaskStatus_NotStarted;
                }

                if (Rate >= 3) 
                {
                    return Resources.FabStartAcademy.TaskStatus_Finished;
                }

                if (IsSubmitted)
                {
                    return Resources.FabStartAcademy.TaskStatus_Submitted;
                }

                return Resources.FabStartAcademy.TaskStatus_Started;
            } }
    }

    public class MemberFlowSession
    {
        public int SessionID { get; set; }
        public string Name { get; set; }

        public List<MemberFlowTaskItem> Tasks { get; set; }
    }
}
