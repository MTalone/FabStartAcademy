using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class MentorFlow
    {
        public static List<Program> GetPrograms(string username) 
        {
            using (MemberContext context = new MemberContext())
            {
                List<int> teamsID = context.Member.Include(x => x.TeamMember).Where(x => x.Email == username).Select(x => x.TeamMember.TeamID).ToList();


                List<Program> programs = context.Team.Include(x => x.Program.Process).Include(x => x.Program.Logo).Where(x => teamsID.Contains(x.ID)).Select(x=>x.Program).ToList();

                return programs;
            }
        }

        public static Program GetProgram(int iD)
        {
            using (MemberContext context = new MemberContext())
            {
               
                Program program = context.Program.Include(x=>x.Process).Include(x=>x.Logo).Where(x=>x.ID==iD).First();

                return program;
            }
        }

        public static DashBoard GetDashBoard(int programID) 
        {
            using (MemberContext context = new MemberContext())
            {

                DashBoard DashBoard = new DashBoard();
                DashBoard.MentorCount = context.TeamMember.Include(x => x.Team).Where(x => x.RoleID == (int)Role.Roles.Mentor && x.Team.ProgramID==programID).Count();
                DashBoard.TeamCount = context.Team.Where(x => x.ProgramID == programID).Count();
                DashBoard.TasksSubmitted = context.TaskSubmission.Include(x => x.Team).Where(x => x.Team.ProgramID == programID && x.IsSubmitted && x.Rating == 0).Count();
                DashBoard.TasksCompleted = context.TaskSubmission.Include(x => x.Team).Where(x => x.Team.ProgramID == programID && x.IsSubmitted && x.Rating >3).Count();
                var a = context.Program.Include(x => x.Process.Task).Where(x => x.ID == programID).Select(x => x.Process.Task.Count).ToList();
                DashBoard.TasksOpened=a.Sum() - DashBoard.TasksSubmitted - DashBoard.TasksCompleted;


                return DashBoard;
            }

        }

        public static List<Team> GetTeams(string userName,int programid)
        {
            using (MemberContext context = new MemberContext())
            {
                List<int> teamsID = context.Member.Include(x => x.TeamMember.Team).Where(x => x.Email == userName && x.TeamMember.Team.ProgramID==programid).Select(x => x.TeamMember.TeamID).ToList();

                List<Team> teams = context.Team.Include(x => x.Program.Process).Include(x => x.Logo).Where(x => teamsID.Contains(x.ID)).ToList();

                return teams;
            }
        }

        public static List<Activity> GetActivities(string username, int programid) 
        {

            using(MemberContext context= new MemberContext()) 
            {
                List<int> teamsID = context.Member.Include(x => x.TeamMember.Team).Where(x => x.Email == username && (x.TeamMember.Team.ProgramID == programid || programid==0)).Select(x => x.TeamMember.TeamID).ToList();
                List<Activity> activities = context.Activity.Include(x=>x.TaskSubmission.Task.Session).Include(a=>a.Team.Logo).Include(a => a.Team.Program).Include(a=>a.Member).Where(a => teamsID.Contains(a.TeamID) && a.TaskSubmission.Rating>0).ToList();
                return activities;
            }
            
        }

        public class DashBoard
        {
            public int TeamCount { get; set; }
            public int MentorCount { get; set; }

            public int MessageCount { get; set; }
            public int TasksCompleted { get; set; }

            public int TasksSubmitted { get; set; }

            public int TasksOpened { get; set; }
        }

    }
}
