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

        public DashBoardBO(bool IsSuperAdmin, int PartnerID)
        {
            using (var a = new DashBoardBOContext())
            {
                ProgramsCount =  a.Program.Where(x=>x.PartnerID==PartnerID||IsSuperAdmin).Count();
                ProcessesCount = a.Process.Where(x => x.PartnerID == PartnerID || IsSuperAdmin).Count();
                MentorsCount = a.TeamMember.Include(x => x.Member).Where(x => (x.RoleID == (int)(Role.Roles.Mentor) && (x.Member.PartnerID == PartnerID || IsSuperAdmin))).
                    Select(x => new { memberid = x.MemberID }).Distinct().Count();
                MembersCount = a.TeamMember.Include(x => x.Member).Where(x => (x.RoleID == (int)Role.Roles.User&&x.Member.IsUser  && (x.Member.PartnerID == PartnerID || IsSuperAdmin))).
                    Select(x => new { memberid = x.MemberID }).Distinct().Count();
            }
        }

    }

    class DashBoardBOContext:FBADataContext 
    {
        public DbSet<Program> Program { get; set; }
        public DbSet<Process> Process { get; set; }
        public DbSet<TeamMember> TeamMember { get; set; }

        public DbSet<Member> Member { get; set; }
    }
}
