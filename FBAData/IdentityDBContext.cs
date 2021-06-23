using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
  
    public class FBAUserDBContext : IdentityDbContext
    {
        public FBAUserDBContext(DbContextOptions<FBAUserDBContext> options)
            : base(options)
        {
        }
    }
}
