﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class KnowledgeBaseContext:FBADataContext
    {
        public DbSet<LearningCategory> LearningCategory { get; set; }

        public DbSet<LearningUnit> LearningUnit { get; set; }

        public DbSet<Document> Document { get; set; }

    }
}