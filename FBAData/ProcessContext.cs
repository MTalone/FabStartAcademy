using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class ProcessContext:FBADataContext
    {
        public DbSet<Process> Process { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Task> Task { get; set; }

        public DbSet<Program> Program { get; set; }
        public DbSet<Document> Document { get; set; }
    }
    public class Process
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int? LogoID { get; set; }

        public Document Logo { get; set; }

        public List<Session> Session { get; set; }

        public static List<ProcessExtended> GetProcesses(int max)
        {
            using (var a = new ProcessContext())
            {
                try
                {
                    if (max > 0)
                    {
                        return a.Process.Include(x=>x.Session).Take(max).Select(x=> new ProcessExtended { Description=x.Description,ID=x.ID,LogoID=x.LogoID, Name=x.Name,SessionCount=x.Session.Count }).ToList();
                    }
                    else
                    {
                        return a.Process.Include(x => x.Session).Select(x => new ProcessExtended { Description = x.Description, ID = x.ID, LogoID = x.LogoID, Name = x.Name, SessionCount = x.Session.Count }).ToList();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static Process  GetProcess(int ID)
        {
            using (var a = new ProcessContext())
            {
                try
                {
                    if (ID > 0)
                    {
                        return a.Process.Include(z=>z.Logo).Where(i=>i.ID==ID).ToList().First();
                    }
                    else
                    {
                        return new Process();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static int Save(Process program)
        {
            using (var a = new ProcessContext())
            {
                if (program.ID == 0)
                {
                    var newgroup = a.Process.Add(program);
                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                   var changes = a.Update(program);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }
               
            }
        }

        public static void Delete(int id)
        {
            try
            {
                using (var a = new ProcessContext())
                {
                    var s = a.Session.Where(x => x.ProcessID == id).ToList();
                    foreach (var item in s)
                    {
                        var t = a.Task.Where(x => x.SessionID == item.ID);

                        a.Task.RemoveRange(t);

                        a.Session.Remove(item);
                    }

                    var p = a.Process.Where(x => x.ID == id).First();
                    if (p.LogoID.HasValue)
                    {
                        a.Document.Remove(a.Document.Where(x => x.ID == p.LogoID).FirstOrDefault());
                    }

                    var g = a.Program.Where(x => x.ProcessID == id);

                    foreach (var i in g) { i.ProcessID = null; }

                    a.Program.UpdateRange(g);

                    a.Process.Remove(a.Process.Where(x => x.ID == id).First());
                    a.SaveChanges();
                }
            }
            catch (Exception ex)
            {

               
            }
           
        }
    }


    public class ProcessExtended:Process
    {
        public int SessionCount { get; set; }
    }
    

}
