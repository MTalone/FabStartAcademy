using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class TasksModel:ProcessRelated
    {
        public List<FBAData.Task> Tasks { get; set; }

        public string SessionName { get; set; }

        public string ProgramName { get; set;}

        public int SessionID { get; set; }

      
        public FBAData.Task Task { get;  set; }

        public List<SelectListItem> TaskTypes { get; set; }

        public List<SelectListItem> Tools { get; set; }

        public bool ReadOnly { get; set; }

        public List<FBAData.TaskDocumentView> TaskDocuments { get; set; }
    }
}
