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
    }
}
