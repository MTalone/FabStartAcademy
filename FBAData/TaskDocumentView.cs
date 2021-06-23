using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{

    public class TaskDocumentView
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        public string FileName { get; set; }

        public int TaskID { get; set; }
    }
}
