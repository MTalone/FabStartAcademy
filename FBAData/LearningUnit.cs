using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FBAData
{
    public class LearningUnit
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public string Teaser { get; set; }
        public string Content { get; set; }
        public int CatergoryID { get; set; }
    }
}