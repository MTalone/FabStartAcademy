using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskStatus
    {
        public int ID { get; set; }
        public string Label { get; set; }

        public enum Status
        {
            Pending = 1,
            Started = 2,
            Submitted = 3,
            InReview = 4,
            Completed = 5
        }
    }
}
