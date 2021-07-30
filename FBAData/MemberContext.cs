using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class MemberContext:FBADataContext
    {
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamMember> TeamMember { get; set; }
        public DbSet<Program> Program { get; set; }

        public DbSet<Process> Process { get; set; }

        public DbSet<Session> Session { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<TaskDocument> TaskDocument { get; set; }

        public DbSet<Document> Document { get; set; }

        public DbSet<Member> Member { get; set; }
        public DbSet<TaskSubmission> TaskSubmission { get; set; }

        public DbSet<Activity> Activity { get; set; }

        public DbSet<TaskSubmissionDocument> TaskSubmissionDocument { get; set; }

        public DbSet<TaskSubmissionComment> TaskSubmissionComment { get; set; }

        public DbSet<TaskSubmissionLink> TaskSubmissionLink { get; set; }

        
    }

    public class MemberFlow 
    {
        public static List<Team> GetTeams(string userName) 
        {
            using (MemberContext context = new MemberContext())
            {
                List<int> teamsID = context.Member.Include(x => x.TeamMember).Where(x => x.Email == userName).Select(x => x.TeamMember.TeamID).ToList();

                List<Team> teams = context.Team.Include(x => x.Program.Process).Include(x=>x.Logo).Where(x => teamsID.Contains(x.ID)).ToList();

                return teams;
            }
        }

        public static List<Document> GetTaskDocuments(int TaskID)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskDocument.Include(x => x.Document).Where(x => x.TaskID == TaskID).Select(x => x.Document).ToList();
            }
        }

        public static List<Document> GetSubmittedDocuments(int iD)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmissionDocument.Include(x => x.Document).Where(x => x.TaskSubmissionID == iD).Select(x => x.Document).ToList();
            }
        }

        public static List<Document> GetProcessDocuments(int processID, int teamID, int tasksubmissionID)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmissionDocument.Include(x => x.Document).Where(x => x.ProcessID == processID && x.TeamID==teamID && x.TaskSubmissionID!=tasksubmissionID).Select(x => x.Document).ToList();
            }
        }


        public static List<TaskSubmissionLink> GetLinks(int tasksubmissionID)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmissionLink.Where(x => x.TaskSubmissionID == tasksubmissionID).ToList();
            }
        }

        public static List<TaskSubmissionLink> GetProcessLinks(int processID, int teamID, int tasksubmissionID)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmissionLink.Include(x=>x.TaskSubmission).Where(x => x.TaskSubmission.ProcessID == processID && x.TaskSubmission.TeamID == teamID && x.TaskSubmissionID != tasksubmissionID).ToList();
            }
        }

        public static Process GetProcess(int processID) 
        {
            using (MemberContext context = new MemberContext())
            {
                
                var process = context.Process.Include(x=>x.Session).ThenInclude(y=>y.Task).Where(x => x.ID==processID).FirstOrDefault();

                process.Session.ForEach(s => s.Task = s.Task.OrderBy(t => t.Order).ToList());

                return process;
            }
        }


        public static TaskSubmission GetTaskSubmission(int TaskID,int TeamID) 
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmission.Where(x => x.TaskID == TaskID && x.TeamID==TeamID).FirstOrDefault();
            }
        }

        public static int SubmitTask(TaskSubmission taskSubmission,int activityTypeID,string username,bool IsMentor)
        {
            int createdby = Member.GetByEmail(username).ID;

            using (var a = new MemberContext())
            {
                if (taskSubmission.ID == 0)
                {
                    taskSubmission.MemberID = createdby;

                    var newgroup = a.TaskSubmission.Add(taskSubmission);

                    a.SaveChanges();
                    taskSubmission.ID= newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(taskSubmission);
                    a.SaveChanges();
                    

                }
                
                Activity activity = new Activity { ActivityTypeID = activityTypeID, TaskSubmissionID = taskSubmission.ID, MemberID = createdby, CreatedOn = DateTime.Now,TeamID=taskSubmission.TeamID,IsMentor=IsMentor };

                a.Activity.Add(activity);
                a.SaveChanges();

            }

            return taskSubmission.ID;
        }

        public static List<TaskSubmissionComment> GetComments(int taskSubmissionID) 
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmissionComment.Include(x=>x.Member).Where(x => x.TaskSubmissionID == taskSubmissionID).ToList();
            }
        }

        public static Team GetTeam(int iD)
        {
            using (MemberContext context = new MemberContext())
            {
               

                Team teams = context.Team.Include(x => x.Logo).Include(x => x.Program.Process.Session).ThenInclude(y=>y.Task).Where(x => x.ID==iD).FirstOrDefault();

                return teams;
            }
        }

        public static List<TaskSubmission> GetTaskSubmissions(int processID, int teamID)
        {
            using (MemberContext context = new MemberContext())
            {
                return context.TaskSubmission.Where(x => x.ProcessID == processID&& x.TeamID == teamID).ToList();
            }
        }
    }
}
