using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAData
{
    public class FBADataContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            

            optionsBuilder.UseSqlServer( @"Server=(localdb)\mssqllocaldb;Database=FabStart;Integrated Security=True");
        }


    }
}
