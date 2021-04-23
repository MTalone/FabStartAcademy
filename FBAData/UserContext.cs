using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class UserContext:FBADataContext
    {
        public DbSet<TeamMember> TeamMember { get; set; }
        public DbSet<Member> Member { get; set; }

        public DbSet<Role> Role { get; set; }
    }
}
