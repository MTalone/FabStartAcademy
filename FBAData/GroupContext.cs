using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FBAData
{
    public class ProgramContext : FBADataContext
    {
       public DbSet<Program> Program { get; set; }
        public DbSet<Process> Process { get; set; }

        
    }

    public class Program
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ProcessID { get; set; }

        public string Code { get; set; }

        public int? LogoID { get; set; }

        public Process Process { get;set;}
        public static List<Program> GetPrograms(int max) 
        {
            using(var a = new ProgramContext())
            {
                try
                {
                    if (max > 0)
                    {
                        var progs = a.Program.Include(x => x.Process).Take(max).ToList();
                        return progs;
                    }
                    else
                    {
                        var groups = a.Program.Include(x => x.Process).ToList();
                        return groups;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                
            }
        }

        public static Program GetProgram(int ID)
        {
            using (var a = new ProgramContext())
            {
                try
                {
                    if (ID > 0)
                    {
                        return a.Program.Include(x=>x.Process).Where(i => i.ID == ID).ToList().First();
                    }
                    else
                    {
                        return new Program();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static int Save(Program program)
        {
            if (program.ID == 0) 
            {
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                program.Code = GuidString.Substring(0,10);
            }


            using (var a = new ProgramContext())
            {
                if (program.ID == 0)
                {
                    var newgroup = a.Program.Add(program);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {

                    a.Program.Update(program);
                  
                    a.SaveChanges();
                    return program.ID;

                }
            }
        }
    }
}
