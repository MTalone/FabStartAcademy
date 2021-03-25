using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class DashBoardBO
    {
        public int ProgramsCount { get; set; }

        public int ProcessesCount { get; set; }

        public int MentorsCount { get; set; }

        public int MembersCount { get; set; }

        public DashBoardBO()
        {
            using (var a = new DashBoardBOContext())
            {
                ProgramsCount =  a.Program.Count();
                ProcessesCount = a.Process.Count();

            }
        }

    }

    class DashBoardBOContext:FBADataContext 
    {
        public DbSet<Program> Program { get; set; }
        public DbSet<Process> Process { get; set; }

    }
}
