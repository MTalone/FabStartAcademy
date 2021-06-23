using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Data
{
    public class FBAUserDBContext : IdentityDbContext
    {
        public FBAUserDBContext(DbContextOptions<FBAUserDBContext> options)
            : base(options)
        {
        }
    }

    public class FBADBContext : DbContext
    {
        public FBADBContext(DbContextOptions<FBAUserDBContext> options)
            : base(options)
        {
        }
    }
}
