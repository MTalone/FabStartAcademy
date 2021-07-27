using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class MentorModel
    {
        public List<ProgramItem> Programs { get; set; }

        public List<TeamItem> Teams { get; set; }

        public List<ActivityDisplay> Activities { get; set; }
        public ProgramItem Program { get; set; }
        public FBAData.MentorFlow.DashBoard DashBoard { get; set; }
    }
}
