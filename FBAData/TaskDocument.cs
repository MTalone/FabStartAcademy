using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class TaskDocument
    {
        public int ID { get; set; }
        public int TaskID { get; set; }

        public int DocumentID { get; set; }

        public Document Document{ get; set; }
    }
}
