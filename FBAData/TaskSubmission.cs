﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskSubmission
    {
        //    [ID] INT NOT NULL IDENTITY PRIMARY KEY,
        //[TaskID] INT NOT NULL, 
        //[TeamID] INT NOT NULL, 
        //[ProcessID] INT NOT NULL, 
        //[CreatedBy] NVARCHAR(450) NOT NULL,

        public int ID { get; set; }
        public int TaskID { get; set; }
        public int TeamID { get; set; }

        public Team Team { get; set; }
        public int ProcessID { get; set; }

        public int SessionID { get; set; }

        public int MemberID { get; set; }

        public int Rating { get; set; }

        public bool IsSubmitted { get; set; }

        public string Text { get; set; }

        public int TaskStatusID { get; set; }
        public Task Task { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public static TaskSubmission Get(int ID)
        {
            using (var a = new MemberContext())
            {
                return a.TaskSubmission.Find(ID);
            }

        }
    }
}
