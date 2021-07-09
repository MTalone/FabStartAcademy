using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class MemberContext:FBADataContext
    {
        public DbSet<Team> Team { get; set; }
        public DbSet<Program> Program { get; set; }

        public DbSet<Process> Process { get; set; }

        public DbSet<Session> Session { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<TaskDocument> TaskDocument { get; set; }

        public DbSet<Document> Document { get; set; }

        public DbSet<Member> Member { get; set; }

    }

    public class MemberFlow 
    {
        public static List<Team> GetTeams(string userName) 
        {
            using (MemberContext context = new MemberContext())
            {
                List<int> teamsID = context.Member.Include(x => x.TeamMember).Where(x => x.Email == userName).Select(x => x.TeamMember.TeamID).ToList();

                List<Team> teams = context.Team.Include(x => x.Program.Process).Where(x => teamsID.Contains(x.ID)).ToList();

                return teams;
            }
        }

    }
}
