using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class Activity
    {
        public int ID { get; set; }
        public int ActivityTypeID { get; set; }
        public int? TaskSubmissionID { get; set; }
        public int TeamID { get; set; }
        public int MemberID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Team Team { get; set; }

        public Member Member { get; set; }
       // public IdentityUser IdentityUser { get; set; }

        public TaskSubmission TaskSubmission { get; set; }
        public bool IsMentor { get; set; }
        public bool IsRead { get; set; }
    }
}
