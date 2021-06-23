using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            
               var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
        }


    }
}
