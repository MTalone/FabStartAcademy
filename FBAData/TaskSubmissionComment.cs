using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskSubmissionComment
    {
   

        public int ID { get; set; }
        public int TaskSubmissionID { get; set; }

        public string Comment { get; set; }

        public int MemberID { get; set; }

        public DateTime CreatedOn { get; set; }

        public Member Member { get; set; }

        public static int Save(TaskSubmissionComment comment,string username)
        {
            int createdby = Member.GetByEmail(username).ID;
            using (var a = new MemberContext())
            {

                comment.MemberID = createdby;
                comment.CreatedOn = DateTime.Now;

                var newgroup = a.TaskSubmissionComment.Add(comment);

                a.SaveChanges();
                return newgroup.Entity.ID;


            }
        }

    }

   
}
