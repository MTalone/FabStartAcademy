using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskSubmissionDocument
    {
        //[Id] INT NOT NULL IDENTITY PRIMARY KEY,
        // [TaskSubmissionID] INT NOT NULL,
        //[DocumentID] INT NOT NULL,
        //[TeamID] INT NOT NULL,
        //[ProcessID] INT NOT NULL,
        public int ID { get; set; }
        public int TaskSubmissionID { get; set; }
        public int TeamID { get; set; }

        public int ProcessID { get; set; }

        public int DocumentID { get; set; }

        public Document Document { get; set; }

        public static int Save(TaskSubmissionDocument taskDocument)
        {
            using (var a = new MemberContext())
            {
                if (taskDocument.ID == 0)
                {


                    var newgroup = a.TaskSubmissionDocument.Add(taskDocument);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(taskDocument);
                    a.SaveChanges();

                    return taskDocument.ID;
                }
            }
        }
    }
}
