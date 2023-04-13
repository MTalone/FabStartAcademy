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

        public int PartnerID { get; set; }

        public bool IsForAll { get; set; }

        public Document Logo { get; set; }

        public List<Session> Session { get; set; }

        public List<Task> Task { get; set; }

        public static List<ProcessExtended> GetProcesses(int max,bool IsSuperAdmin,int PartnerID)
        {
            using (var a = new ProcessContext())
            {
                try
                {
                    if (max > 0)
                    {
                        return a.Process.Where(x => x.PartnerID == PartnerID || IsSuperAdmin || x.IsForAll).Include(x=>x.Session).Take(max).Select(x=> new ProcessExtended { Description=x.Description,ID=x.ID,LogoID=x.LogoID, Name=x.Name,SessionCount=x.Session.Count }).ToList();
                    }
                    else
                    {
                        return a.Process.Where(x => x.PartnerID == PartnerID || IsSuperAdmin || x.IsForAll).Include(x => x.Session).Select(x => new ProcessExtended { Description = x.Description, ID = x.ID, LogoID = x.LogoID, Name = x.Name, SessionCount = x.Session.Count }).ToList();
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

        public static int Save(Process process)
        {
            using (var a = new ProcessContext())
            {
                if (process.ID == 0)
                {
                    var newgroup = a.Process.Add(process);
                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                   var changes = a.Update(process);
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

        public static int Duplicate(int id)
        {
            int newID = 0;
            try
            {
                using (var a = new ProcessContext())
                {

                    var method = a.Process.Where(x=>x.ID==id).FirstOrDefault();

                    Process process = new Process { Description=method.Description,IsForAll=method.IsForAll,Name=method.Name,PartnerID=method.PartnerID};
                    a.Process.Add(process);
                      a.SaveChanges();
                    newID = process.ID;
                    List <Session> sessions = a.Session.Where(x=>x.ProcessID==id).ToList();
                    int i = 0;
                    foreach (var session in sessions)
                    { i++;
                        Session copy = new Session { ProcessID = newID, Description = session.Description, Name = session.Name, Order = i };
                         a.Session.Add(copy);
                        int newSession = a.SaveChanges();
                        List<Task> tasks = a.Task.Where(x=>x.SessionID==session.ID).ToList();
                        foreach (var task in tasks)
                        {
                            Task newTask = new Task {AvailableOn=task.AvailableOn,
                                                    Description=task.Description,
                                                    Instructions=task.Instructions, 
                                                    ProcessID= newID,
                                                    SessionID=copy.ID,Name=task.Name,Order=task.Order,IsEvaluated=task.IsEvaluated,TaskType=task.TaskType };
                           a.Task.Add(newTask);
                        }
                        a.SaveChanges();
                    }


                }
                return newID;
            }
            catch (Exception ex)
            {
                return -1;

            }
        }
    }


    public class ProcessExtended:Process
    {
        public int SessionCount { get; set; }
    }
    

}
