using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskSubmissionLink
    {
        public int ID { get; set; }
        public int TaskSubmissionID { get; set; }

        public string URL { get; set; }

        public TaskSubmission TaskSubmission { get;set;}
     

        public static int Save(TaskSubmissionLink link)
        {
           
            using (var a = new MemberContext())
            {

                
                var newgroup = a.TaskSubmissionLink.Add(link);

                a.SaveChanges();
                return newgroup.Entity.ID;


            }
        }
    }
}
