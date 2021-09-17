using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class MemberTask
    {
        public FBAData.Task Task { get; set; }
        public FBAData.Team Team { get; set; }

        public List<FBAData.Document> TaskDocuments{get;set;}

        public List<FBAData.Document> SubmittedDocuments { get; set; }
        public List<FBAData.Document> ProcessDocuments { get; set; }

        public List<FBAData.TaskSubmissionComment> Comments { get; set; }
        
        public List<FBAData.TaskSubmissionLink> Links { get; set; }
        public List<FBAData.TaskSubmissionLink> ProcessLinks { get; set; }
        public int TaskSubmissionID { get; set; }

        public bool IsSubmitted { get; set; }

        public int Rate { get; set; }

        public int MemberID { get; set; }

        public bool IsMentor { get; set; }
        public int TaskStatusID { get; set; }

        public string Text { get; set; }
        public FBAData.Process Process { get; set; }
    }

   }
