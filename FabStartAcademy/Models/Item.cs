using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class Item
    {

        public int ID { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.FabStartAcademy))]
        public string Title { get; set; }
       

        [Display(Name = "Description", ResourceType = typeof(Resources.FabStartAcademy))]
        public string Description { get; set; }
    }

    public class ProcessRelated :Item
    {
        [Display(Name = "Method", ResourceType = typeof(Resources.FabStartAcademy))]
        public int ProcessID { get; set; }
    }
}
