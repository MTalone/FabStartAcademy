using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FBAData
{
    public class Session
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ProcessID { get; set; }

        public Process Process { get; set; }

       
        public List<Task> Task { get; set; }
        public static List<SessionExtend> GetSessions(int programID) 
        {
            using (var a = new SessionContext())
            {
                try
                {

                    var query = a.Session.Include(x => x.Task).Where(x => x.ProcessID == programID).ToList();

                    ;

                    return query.Select(x=>new SessionExtend {Description=x.Description,Name=x.Name,ID=x.ID,ProcessID=x.ProcessID,NumberTasks=x.Task.Count() }).ToList() ;
                     
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
       
        }

        public static Session GetSession(int ID,bool includeProgram)
        {
            using (var a = new SessionContext())
            {
                try
                {
                    if (ID > 0)
                    {
                        if (includeProgram) { return a.Session.Include(x=>x.Process).Where(i => i.ID == ID).ToList().First(); }
                        else
                        {
                            return a.Session.Where(i => i.ID == ID).ToList().First();
                        }
                        
                    }
                    else
                    {
                        return new Session();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public static int SaveSession(Session session)
        {
            using (var a = new SessionContext())
            {
                if (session.ID == 0)
                {
                    var newgroup = a.Session.Add(session);
                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(session);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }

            }
        }

        
    }

    public class SessionExtend:Session 
    {
        public int NumberTasks { get; set; }
    }

   

    public class SessionContext:FBADataContext 
    {
        public DbSet<Session> Session { get; set; }

        public DbSet<Process> Process { get; set; }

        public DbSet<Task> Task { get; set; }
    }
}