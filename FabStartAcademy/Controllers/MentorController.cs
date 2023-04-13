using AutoMapper;
using FabStartAcademy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class MentorController : Controller
    {
        private IWebHostEnvironment Environment;

        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MentorController(IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment _environment)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            Environment = _environment;
        }
        public IActionResult Dashboard()
        {
            var progs = FBAData.MentorFlow.GetPrograms(User.Identity.Name);
            var act = FBAData.MentorFlow.GetActivities(User.Identity.Name, 0);
            List<ProgramItem> items=progs.Select(x=>new ProgramItem
            {
                ID = x.ID,
                Process = x.Process is null ? string.Empty : x.Process.Name,
                Image = TeamModel.GetImageFromDocument(x.Logo, Environment.WebRootPath, "/imgs/placeholder-group.png"),
                Title = x.Name
            }).ToList();

            List<ActivityDisplay> activities = act.Select(x=>new ActivityDisplay
            {
                Date = x.CreatedOn,
                SessionName = x.TaskSubmission.Task.Session.Name,
                TaskTitle = x.TaskSubmission.Task.Name,
                TeamName = x.Team.Name,
                Image = TeamModel.GetImageFromDocument(x.Team.Logo, Environment.WebRootPath, "/imgs/placeholder-team.png"),
                Type=(FBAData.ActivityType.Types)x.ActivityTypeID,
                MemberName=x.Member.FirstName+" "+x.Member.LastName,
                TeamID = x.TeamID,
                TaskID = x.TaskSubmission.TaskID

            }).ToList();

            MentorModel mentorModel = new MentorModel { Programs = items, Activities = activities };

            return View(mentorModel);
        }

        public IActionResult Program(int ID) 
        {
            FBAData.Program program = FBAData.MentorFlow.GetProgram(ID);
           



            var act = FBAData.MentorFlow.GetActivities(User.Identity.Name, 0).OrderByDescending(x=>x.CreatedOn);
            ProgramItem item = new ProgramItem
            {
                Process = program.Process is null?"":program.Process.Name,
                ID = program.ID,
                Description = program.Description,
                Title = program.Name,
                Image = TeamModel.GetImageFromDocument(program.Logo, Environment.WebRootPath, "/imgs/placeholder-group.png")
            };

            var x = FBAData.MentorFlow.GetDashBoard(ID);
            List<TeamItem> teams = FBAData.MentorFlow.GetTeams(User.Identity.Name, ID).Select(x => new TeamItem
            {
                Title = x.Name,
                ID = x.ID,
                ProgramID = x.ProgramID ?? 0,
                Description = x.Description,
                ProgramTitle = x.Program.Name,
                Image = TeamModel.GetImageFromDocument(x.Logo, Environment.WebRootPath, "/imgs/placeholder-team.png"),
                MethodName = !(x.Program.Process is null) ? x.Program.Process.Name : null
            }).ToList();

            int total = 0;
            int complete = 0;
            int totalRate = 0;

            foreach (var t in teams)
            {
                List<FBAData.TaskSubmission> submissions = new List<FBAData.TaskSubmission>();

                if(program.ProcessID.HasValue)
                    submissions = FBAData.MemberFlow.GetTaskSubmissions(program.ProcessID.Value, t.ID);

                if(program.Process != null) { 
                foreach (var s in program.Process.Session)
                {
                    MemberFlowSession currentSession = new MemberFlowSession { Name = s.Name, SessionID = s.ID };
                    currentSession.Tasks = new List<MemberFlowTaskItem>();
                    foreach (var task in s.Task.OrderBy(x => x.Order))
                    {
                        total++;

                        FBAData.TaskSubmission current = submissions.Where(x => x.TaskID == t.ID).FirstOrDefault();
                        if(!(current is null))
                        {
                            complete += current.TaskStatusID == (int)FBAData.TaskStatus.Status.Completed ? 1 : 0;
                            totalRate += current.TaskStatusID == (int)FBAData.TaskStatus.Status.Completed ? current.Rating : 0;

                        }
                        current = current is null ? new FBAData.TaskSubmission() : current;

                    }



                }
                }

                
                t.Progress = total==0?0: (int)((complete / (float)total) * 100);

                if (complete > 0)
                {
                    t.Rate = (totalRate / (complete));

                }
            }



            List<ActivityDisplay> activities = act.Select(x => new ActivityDisplay
            {
                Date = x.CreatedOn,
                SessionName = x.TaskSubmission.Task.Session.Name,
                TaskTitle = x.TaskSubmission.Task.Name,
                TeamName = x.Team.Name,
                Image = TeamModel.GetImageFromDocument(x.Team.Logo, Environment.WebRootPath, "/imgs/placeholder-team.png"),
                Type = (FBAData.ActivityType.Types)x.ActivityTypeID,
                MemberName = x.Member.FirstName + " " + x.Member.LastName,
                TeamID=x.TeamID,
                TaskID=x.TaskSubmission.TaskID

            }).ToList();

            MentorModel mentorModel = new MentorModel { Program = item ,DashBoard=x,Teams=teams,Activities= activities };
            return View(mentorModel);
        }
    }
}
