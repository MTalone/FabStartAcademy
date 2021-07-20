using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class ActivityType
    {
        public int ID { get; set; }
        public string Type { get; set; }

        public enum Types
        {
            Document=1,
            Submission=2,
            Message=3,
            Comment=4,
            Evaluation=5
        }
    }
}
