using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
   public class TaskContext : FBADataContext
    {
        public DbSet<Session> Session { get; set; }

        public DbSet<Process> Program { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<TaskDocument> TaskDocument { get; set; }

        public DbSet<Document> Document { get; set; }
    }
}
