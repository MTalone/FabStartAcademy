using AutoMapper;
using FabStartAcademy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class MemberController : Controller
    {
        private IWebHostEnvironment Environment;

        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MemberController(IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment _environment)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            Environment = _environment;
        }
        public IActionResult Dashboard()
        {
            var a = FBAData.MemberFlow.GetTeams(User.Identity.Name);

           List<TeamItem> teams =a.Select(x => new TeamItem
           {
               Title = x.Name,
               ID = x.ID,
               ProgramID = x.ProgramID ?? 0,
               Description = x.Description,
               ProgramTitle = x.Program.Name,
               Image = TeamModel.GetImageFromDocument(x.Logo, Environment.WebRootPath, "/imgs/placeholder-team.png"),
               MethodName=x.Program.Process.Name
           }).ToList(); ;

            return View(teams);
        }

        public IActionResult Task(int TaskID, int TeamID) 
        {
            var account = Models.Account.Account.GetAccountSession(HttpContext.Session, User.Identity.Name);
            FBAData.Task task = FBAData.Task.GetTask(TaskID);
            task.Session = FBAData.Session.GetSession(task.SessionID, true);
            FBAData.Team team = FBAData.Team.Get(TeamID, true);
            List<FBAData.Document> taskdocs = FBAData.MemberFlow.GetTaskDocuments(TaskID);
            FBAData.TaskSubmission taskSubmission = FBAData.MemberFlow.GetTaskSubmission(TaskID, TeamID);

            taskSubmission = taskSubmission is null ? new FBAData.TaskSubmission() : taskSubmission;

            List <FBAData.Document> submdocs = FBAData.MemberFlow.GetSubmittedDocuments(taskSubmission.ID);
            List<FBAData.Document> procdocs = FBAData.MemberFlow.GetProcessDocuments(task.Session.ProcessID, TeamID,taskSubmission.ID);
            List<FBAData.TaskSubmissionComment> comments = FBAData.MemberFlow.GetComments(taskSubmission.ID);
            List<FBAData.TaskSubmissionLink> links = FBAData.MemberFlow.GetLinks(taskSubmission.ID);
            List<FBAData.TaskSubmissionLink> proclinks = FBAData.MemberFlow.GetProcessLinks(task.Session.ProcessID, TeamID, taskSubmission.ID);
            Models.MemberTask model = new Models.MemberTask
            {
                Task = task,
                TaskDocuments = taskdocs,
                Team = team,
                TaskSubmissionID = taskSubmission is null ? 0 : taskSubmission.ID,
                MemberID = taskSubmission.MemberID,
                SubmittedDocuments = submdocs,
                ProcessDocuments = procdocs,
                Comments=comments,
                Process = FBAData.MemberFlow.GetProcess(task.Session.ProcessID),
                IsSubmitted=taskSubmission.IsSubmitted,
                Rate=taskSubmission.Rating,
                IsMentor= account.IsSuperAdmin || account.IsAdmin || account.IsMentor,
                Links=links,
                ProcessLinks=proclinks
                
            };

            return View(model);
            
        }

        [HttpPost]
        public IActionResult TaskSubmission(Models.MemberTaskSubmission memberTaskSubmission, IFormFile doc)
        {

            FBAData.TaskSubmission taskSubmission = new FBAData.TaskSubmission
            {
                ProcessID = memberTaskSubmission.ProcessID,
                TaskID = memberTaskSubmission.TaskID,
                TeamID = memberTaskSubmission.TeamID,
                SessionID=memberTaskSubmission.SessionID,
                ID=memberTaskSubmission.TaskSubmissionID,
                MemberID=memberTaskSubmission.MemberID,
                IsSubmitted=memberTaskSubmission.IsSubmitted,
                Rating=memberTaskSubmission.Rate
            };

            memberTaskSubmission.TaskSubmissionID = FBAData.MemberFlow.SubmitTask(taskSubmission,memberTaskSubmission.ActivityTypeID,User.Identity.Name,memberTaskSubmission.IsMentor);




            if (memberTaskSubmission.ActivityTypeID == (int)FBAData.ActivityType.Types.Document) 
            {
                if (!(doc is  null))
                {
                    FBAData.Document document = new FBAData.Document
                    {

                        FileName = doc.FileName,
                        FileType = doc.ContentType,
                        Path = string.Format("/docs/submissions/{0}/{1}", memberTaskSubmission.TaskSubmissionID, doc.FileName)
                    };

                    if (doc.Length > 0)
                    {

                        string uploads = Path.Combine(Environment.WebRootPath, "docs");
                        uploads = Path.Combine(uploads, "submissions");
                        uploads = Path.Combine(uploads, memberTaskSubmission.TaskSubmissionID.ToString());

                        string filePath = Path.Combine(uploads, doc.FileName);
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }
                        document.Path = filePath;
                        using (Stream fileStream = new FileStream(document.Path, FileMode.Create))
                        {
                            doc.CopyTo(fileStream);
                        }

                        int docID = FBAData.Document.Upload(document);
                        FBAData.TaskSubmissionDocument taskDocument = new FBAData.TaskSubmissionDocument
                        {
                            DocumentID = docID,
                            TaskSubmissionID = memberTaskSubmission.TaskSubmissionID,
                            ProcessID = memberTaskSubmission.ProcessID,
                            TeamID = memberTaskSubmission.TeamID
                        };
                        int id = FBAData.TaskSubmissionDocument.Save(taskDocument);


                    }
                   
                }
                else
                {
                    FBAData.TaskSubmissionLink.Save(new FBAData.TaskSubmissionLink { TaskSubmissionID = taskSubmission.ID, URL = memberTaskSubmission.URL });
                }
            }

            if(memberTaskSubmission.ActivityTypeID == (int)FBAData.ActivityType.Types.Comment) 
            {
                FBAData.TaskSubmissionComment.Save(new FBAData.TaskSubmissionComment { Comment = memberTaskSubmission.Comment, TaskSubmissionID = memberTaskSubmission.TaskSubmissionID }, User.Identity.Name);
            }

            return RedirectToActionPermanent(Models.Controllers.Member.Actions.Task.Name,new { TaskID= memberTaskSubmission.TaskID, TeamID =memberTaskSubmission.TeamID });

        }
    
        [HttpPost]
        public IActionResult Rate(int ID,int rate) 
        {
            FBAData.TaskSubmission taskSubmission = FBAData.TaskSubmission.Get(ID);
            taskSubmission.Rating = rate;
            FBAData.MemberFlow.SubmitTask(taskSubmission, (int)FBAData.ActivityType.Types.Evaluation,User.Identity.Name,true);

            return RedirectToActionPermanent(Models.Controllers.Member.Actions.Task.Name, new { TaskID = taskSubmission.TaskID, TeamID = taskSubmission.TeamID });
        }

        public IActionResult Team(int ID)
        {
            FBAData.Team team = FBAData.MemberFlow.GetTeam(ID);
            team.Logo = team.Logo is null ? new FBAData.Document() : team.Logo;
            team.Logo.Path = TeamModel.GetImageFromDocument(team.Logo, Environment.WebRootPath, "/imgs/placeholder-team.png");

            List<FBAData.TaskSubmission> submissions = FBAData.MemberFlow.GetTaskSubmissions(team.Program.ProcessID.Value,team.ID);

            MemberFlowTeam model = new MemberFlowTeam { Team = new FBAData.Team { Name = team.Name, ID = team.ID }, MethodName = team.Program.Process.Name, ProgramName = team.Program.Name, Sessions = new List<MemberFlowSession>(),LogoPath=team.Logo.Path };

            foreach(var s in team.Program.Process.Session) 
            {
                MemberFlowSession currentSession = new MemberFlowSession { Name = s.Name, SessionID = s.ID };
                currentSession.Tasks = new List<MemberFlowTaskItem>();
                foreach (var t in s.Task.OrderBy(x=>x.Order))
                {
                    FBAData.TaskSubmission current = submissions.Where(x => x.TaskID == t.ID).FirstOrDefault();
                    current = current is null ? new FBAData.TaskSubmission() : current;
                    MemberFlowTaskItem currentTask = new MemberFlowTaskItem { Task = t, IsSubmitted = current.IsSubmitted, Process = team.Program.Process, Rate = current.Rating, TeamID = team.ID, TaskSubmissionID = current.ID };
                    currentSession.Tasks.Add(currentTask);
                }
                model.Sessions.Add(currentSession);
            }

            List<FBAData.Document> procdocs = FBAData.MemberFlow.GetProcessDocuments(team.Program.ProcessID.Value, ID, 0);
            model.ProcessDocuments = procdocs;
            model.ProcessLinks =  FBAData.MemberFlow.GetProcessLinks(team.Program.ProcessID.Value, ID, 0); 
            return View(model);
        }
    }
}
