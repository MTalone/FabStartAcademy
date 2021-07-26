using AutoMapper;
using FabStartAcademy.Models;
using FabStartAcademy.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace FabStartAcademy.Controllers
{
    public class BackOfficeController : Controller
    {

        private IWebHostEnvironment Environment;

        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BackOfficeController(IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment _environment)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            Environment = _environment;
        }



        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult DashBoard()
        {
            DashBoardModel dash = new DashBoardModel(Environment.WebRootPath, ControllerContext.HttpContext.Session, User.Identity.Name);
            //dash.ItemList.Last().Count = _userManager.Users.Count();
            return View(dash);
        }
        [Authorize]
        public IActionResult Programs()
        {
            ProgramsModel model = new ProgramsModel();

            //var ps = FBAData.Program.GetPrograms(0).Select(x => new ProgramItem
            //{
            //    Title = x.Name,
            //    Image = "/imgs/thumb_351_image_big.png",
            //    Process = x.Process != null ? x.Process.Name : string.Empty,
            //    ID = x.ID
            //}).ToList();

            Account account = Account.GetAccountSession(ControllerContext.HttpContext.Session, User.Identity.Name);

            var ps = FBAData.Program.GetPrograms(0,account.IsSuperAdmin,account.PartnerID);

            string programDefaultPath = "/imgs/placeholder-group.png";
            model.Programs = new List<ProgramItem>();
            model.GetProgramItems(ps, programDefaultPath, Environment.WebRootPath);

            return View(model);


        }
        [Authorize]
        public IActionResult Program(int ID = 0, bool read = true)
        {
            ProgramItem groupItem = new ProgramItem();
            read = ID == 0 ? false : read;
            var account = Account.GetAccountSession(HttpContext.Session, User.Identity.Name);
            if (ID > 0)
            {
                var group = FBAData.Program.GetProgram(ID);
                groupItem = new ProgramItem
                {
                    Title = group.Name,
                    Process = group.Process != null ? group.Process.Name : string.Empty,
                    ID = group.ID,
                    Code = group.Code,
                    Description = group.Description,
                    ProcessID = group.ProcessID,
                    Image = group.Logo != null ? group.Logo.Path.Replace(Environment.WebRootPath, "") : "",
                    PartnerID=group.PartnerID
                };

            }
            else
            {
                groupItem.PartnerID = account.PartnerID;

            }



            

            groupItem.IsRead = read;
            
            ProgramsModel model = new ProgramsModel();
            model.Program = groupItem;
            model.IsSuperAdmin = account.IsSuperAdmin;
            if (account.IsSuperAdmin)
            {
                model.Partners = FBAData.Partner.List(true, 0).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();
            }

            model.Processes = FBAData.Process.GetProcesses(0,false,groupItem.PartnerID).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Program(ProgramsModel item, IFormFile Logo, bool isNext = false)
        {

            int? documentID = item.Program.LogoID;
            FBAData.Document document = new FBAData.Document();
            string firstPath = string.Empty;

            if (Logo != null)
            {
                if (Logo.Length > 0)
                {
                    document = new FBAData.Document
                    {
                        ID = item.Program.LogoID.HasValue ? item.Program.LogoID.Value : 0,
                        FileName = Logo.FileName,
                        FileType = Logo.ContentType
                    };

                    string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                    uploads = Path.Combine(uploads, "Programs");
                    if (item.Program.ID > 0)
                    {
                        uploads = Path.Combine(uploads, item.Program.ID.ToString());
                    }
                    else
                    {
                        uploads = Path.Combine(uploads, Guid.NewGuid().ToString());
                        firstPath = uploads;
                    }

                    string filePath = Path.Combine(uploads, Logo.FileName);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    document.Path = filePath;
                    using (Stream fileStream = new FileStream(document.Path, FileMode.Create))
                    {
                        Logo.CopyTo(fileStream);
                    }



                    documentID = FBAData.Document.Upload(document);
                    document.ID = documentID.Value;
                }

            }

             

            int id = FBAData.Program.Save(new FBAData.Program
            {
                ID = item.Program.ID,
                Name = item.Program.Title,
                Description = item.Program.Description,
                ProcessID = item.Program.ProcessID,
                Code = item.Program.Code,
                LogoID = documentID,
                PartnerID=item.Program.PartnerID
            });

            if (item.Program.ID == 0 && Logo != null && Logo.Length > 0)
            {
                string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                uploads = Path.Combine(uploads, "Programs");
                uploads = Path.Combine(uploads, id.ToString());
                string filePath = Path.Combine(uploads, Logo.FileName);
                Directory.CreateDirectory(uploads);

                System.IO.File.Move(document.Path, filePath);

                System.IO.Directory.Delete(firstPath);
                document.Path = filePath;
                FBAData.Document.Upload(document);
            }

            if (isNext)
                return RedirectToActionPermanent(Actions.Teams.Name, new { ProgramID = id });
            else
                return RedirectToActionPermanent(Actions.Programs.Name);


        }
        [Authorize]
        [HttpPost]
        public IActionResult ProgramDelete(int id)
        {

            FBAData.Program.Delete(id);

            return RedirectToActionPermanent(Actions.Methods.Name);
        }
        public IActionResult Teams(int programID)
        {
            TeamModel model = new TeamModel(programID, Environment.WebRootPath, "/imgs/placeholder-team.png");


            return View(model);
        }

        [Authorize]
        public IActionResult Team(int ID, int programID, bool read = true)
        {


            TeamItem item = new TeamItem { ProgramID = programID };

            read = ID == 0 ? false : read;

            if (ID > 0)
            {
                var i = FBAData.Team.Get(ID, true);
                item = new TeamItem
                {
                    ID = i.ID,
                    Description = i.Description,
                    Title = i.Name,
                    ProgramID = i.ProgramID ?? 0,
                    ProgramTitle = i.Program.Name,
                    LogoID = i.LogoID,
                    Image = i.Logo != null ? i.Logo.Path.Replace(Environment.WebRootPath, "") : "",
                    Code = i.Code
                };
                item.IsReadOnly = read;
            }

            return View(item);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Team(TeamItem item, IFormFile Logo, bool isNext = false)
        {
            int? documentID = item.LogoID;
            FBAData.Document document = new FBAData.Document();
            string firstPath = string.Empty;
            if (Logo != null)
            {
                if (Logo.Length > 0)
                {
                    document = new FBAData.Document
                    {
                        ID = item.LogoID.HasValue ? item.LogoID.Value : 0,
                        FileName = Logo.FileName,
                        FileType = Logo.ContentType
                    };

                    string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                    uploads = Path.Combine(uploads, "Teams");
                    if (item.ID > 0)
                    {
                        uploads = Path.Combine(uploads, item.ID.ToString());
                    }
                    else
                    {
                        uploads = Path.Combine(uploads, Guid.NewGuid().ToString());
                        firstPath = uploads;
                    }

                    string filePath = Path.Combine(uploads, Logo.FileName);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    document.Path = filePath;
                    using (Stream fileStream = new FileStream(document.Path, FileMode.Create))
                    {
                        Logo.CopyTo(fileStream);
                    }



                    documentID = FBAData.Document.Upload(document);
                    document.ID = documentID.Value;
                }

            }

            var program = FBAData.Program.GetProgram(item.ProgramID);

            int id = FBAData.Team.Save(new FBAData.Team { ID = item.ID, Name = item.Title, Description = item.Description, LogoID = documentID, ProgramID = item.ProgramID,PartnerID=program.PartnerID });

            if (item.ID == 0 && Logo != null && Logo.Length > 0)
            {
                string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                uploads = Path.Combine(uploads, "Teams");
                uploads = Path.Combine(uploads, id.ToString());
                string filePath = Path.Combine(uploads, Logo.FileName);
                Directory.CreateDirectory(uploads);

                System.IO.File.Move(document.Path, filePath);

                System.IO.Directory.Delete(firstPath);
                document.Path = filePath;
                FBAData.Document.Upload(document);
            }

            if (isNext)
                return RedirectToActionPermanent(Actions.Members.Name, new { TeamID = id });
            else
                return RedirectToActionPermanent(Actions.Teams.Name, new { programID = item.ProgramID });
        }

        public IActionResult Members(int teamID)
        {
            TeamModel model = new TeamModel
            {
                TeamID = teamID,
                Team = FBAData.Team.Get(teamID, true),
                Members = FBAData.TeamMember.GetList(teamID).Select(x => new MemberItem { Email = x.Member.Email, RoleName = x.Role.Name, RoleID = x.RoleID, IsConfirmed = x.IsConfirmed }).OrderByDescending(x => x.IsConfirmed).ToList()
            };
            model.ProgramID = model.Team.ProgramID.Value;
            model.ProgramTitle = model.Team.Program.Name;
            model.Roles = FBAData.Role.GetList(false).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Member(int teamID, TeamModel model)
        {
            //verificar se Member existe.
            FBAData.Member m = FBAData.Member.GetByEmail(model.Member.Email);

            var team = FBAData.Team.Get(teamID, false);
            bool roleAllowed = true;
            int memberid = 0;
            if (m is null || m.ID == 0)
            {
               
                //Criar Member
                memberid = FBAData.Member.Save(new FBAData.Member { Email = model.Member.Email,PartnerID=team.PartnerID });

            }
            else
            {
                roleAllowed = false;
                memberid = m.ID;
                roleAllowed = FBAData.TeamMember.CheckAllowedRole(memberid, model.Member.RoleID);

            }

            if (!roleAllowed) 
            {
                model.TeamID = teamID;
                return RedirectToActionPermanent(Actions.Members.Name, new { teamID = teamID });
            }

            //Criar TeamMember;
            // Check if member exists in team

            //If role isn't the same as existing error

            

            if (!FBAData.TeamMember.Exists(teamID, memberid))
            {
                
                FBAData.TeamMember.Save(new FBAData.TeamMember { MemberID = memberid, TeamID = teamID, RoleID = model.Member.RoleID });
            }

            

            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var smtpClient = new SmtpClient(config["Smtp:Host"])
            {
                Port = int.Parse(config["Smtp:Port"]),
                Credentials = new NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]),
                EnableSsl = true,
            };
            //Enviar E-mails
            var mailMessage = new MailMessage
            {
                From = new MailAddress(config["Smtp:Username"]),
                Subject = string.Format("Invitation to join team {0}", team.Name),
                Body = string.Format("Hello<br/> you are invited to join the team {0}. Please use this link to register, and insert the code {1}", team.Name, config["Constants:CodeBegin"] + team.Code),
                IsBodyHtml = true,
            };
            mailMessage.To.Add(model.Member.Email);

            smtpClient.Send(mailMessage);

            return RedirectToActionPermanent(Actions.Members.Name, new { teamID = teamID });

        }

        [Authorize]
        public IActionResult Methods()
        {
            return View(new ProgramModel(0,ControllerContext.HttpContext.Session, User.Identity.Name));

        }
        [Authorize]
        public IActionResult Method(int ID = 0, bool read = true)
        {
            ProcessItem item = new ProcessItem();

            read = ID == 0 ? false : read;
            var account = Account.GetAccountSession(HttpContext.Session, User.Identity.Name);
            if (ID > 0)
            {
                var i = FBAData.Process.GetProcess(ID);

              

                item = new ProcessItem
                {
                    ID = i.ID,
                    Description = i.Description,
                    Title = i.Name,
                    LogoID = i.LogoID,
                    Logo = i.Logo == null ? new byte[0] : System.IO.File.ReadAllBytes(i.Logo.Path),
                    IsSuperAdmin = account.IsSuperAdmin,
                    PartnerID=i.PartnerID
                };
                item.IsReadOnly = read;

            }
            else
            {
                item.PartnerID = account.PartnerID;

            }

            if (account.IsSuperAdmin)
            {
                item.Partners = FBAData.Partner.List(true, 0).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();
            }

            item.IsSuperAdmin = account.IsSuperAdmin;
            

            return View(item);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Method(ProcessItem item, IFormFile Logo, bool isNext = false)
        {
            int? documentID = item.LogoID;
            FBAData.Document document = new FBAData.Document();
            string firstPath = string.Empty;
            if (Logo != null)
            {
                if (Logo.Length > 0)
                {
                    document = new FBAData.Document
                    {
                        ID = item.LogoID.HasValue ? item.LogoID.Value : 0,
                        FileName = Logo.FileName,
                        FileType = Logo.ContentType
                    };

                    string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                    uploads = Path.Combine(uploads, "Methods");
                    if (item.ID > 0)
                    {
                        uploads = Path.Combine(uploads, item.ID.ToString());
                    }
                    else
                    {
                        uploads = Path.Combine(uploads, Guid.NewGuid().ToString());
                        firstPath = uploads;
                    }

                    string filePath = Path.Combine(uploads, Logo.FileName);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    document.Path = filePath;
                    using (Stream fileStream = new FileStream(document.Path, FileMode.Create))
                    {
                        Logo.CopyTo(fileStream);
                    }



                    documentID = FBAData.Document.Upload(document);
                    document.ID = documentID.Value;
                }

            }

            var account = Account.GetAccountSession(HttpContext.Session, User.Identity.Name);
            int id = FBAData.Process.Save(new FBAData.Process { ID = item.ID, Name = item.Title, Description = item.Description, LogoID = documentID,PartnerID=item.PartnerID });

            if (item.ID == 0 && Logo != null && Logo.Length > 0)
            {
                string uploads = Path.Combine(Environment.WebRootPath, "imgs");
                uploads = Path.Combine(uploads, "Methods");
                uploads = Path.Combine(uploads, id.ToString());
                string filePath = Path.Combine(uploads, Logo.FileName);
                Directory.CreateDirectory(uploads);

                System.IO.File.Move(document.Path, filePath);

                System.IO.Directory.Delete(firstPath);
                document.Path = filePath;
                FBAData.Document.Upload(document);
            }

            if (isNext)
                return RedirectToActionPermanent(Actions.Sessions.Name, new { ProgramID = id });
            else
                return RedirectToActionPermanent(Actions.Methods.Name);
        }
        [Authorize]
        [HttpPost]
        public IActionResult MethodDelete(int id)
        {

            FBAData.Process.Delete(id);

            return RedirectToActionPermanent(Actions.Methods.Name);
        }
        [Authorize]
        public IActionResult Sessions(int ProgramID)
        {

            SessionModel model = new SessionModel(ProgramID);

            return View(model);
        }
        [Authorize]
        public IActionResult Session(int ID, int programID, bool read = true)
        {


            SessionItem item = new SessionItem { ProcessID = programID };

            read = ID == 0 ? false : read;

            if (ID > 0)
            {
                var i = FBAData.Session.GetSession(ID, true);
                item = new SessionItem { ID = i.ID, Description = i.Description, Title = i.Name, ProcessID = i.ProcessID, ProgramTitle = i.Process.Name };
                item.IsReadOnly = read;
            }

            return View(item);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Session(SessionItem item)
        {
            FBAData.Session.SaveSession(new FBAData.Session { ID = item.ID, Name = item.Title, Description = item.Description, ProcessID = item.ProcessID });

            return RedirectToActionPermanent(Actions.Sessions.Name, new { programid = item.ProcessID });
        }

        [Authorize]
        [HttpPost]
        public IActionResult SessionOrder(int id, int order, int processID)
        {

            FBAData.Session.ChangeOrder(id, order);

            return RedirectToActionPermanent(Actions.Sessions.Name, new { programID = processID });
        }

        [Authorize]
        public IActionResult Tasks(int sessionID)
        {
            TasksModel tasksModel = new TasksModel();

            FBAData.Session session = FBAData.Session.GetSession(sessionID, true);
            tasksModel.SessionID = sessionID;
            tasksModel.ProgramName = session.Process.Name;
            tasksModel.SessionName = session.Name;
            tasksModel.Tasks = FBAData.Task.GetTasks(sessionID);
            tasksModel.ProcessID = session.ProcessID;

            return View(tasksModel);

        }

        [Authorize]
        [HttpPost]
        public IActionResult TaskDelete(int id, int sessionID)
        {

            FBAData.Task.DeleteTask(id);

            return RedirectToActionPermanent(Actions.Tasks.Name, new { sessionID = sessionID });
        }

        [Authorize]
        [HttpPost]
        public IActionResult TaskOrder(int id, int order, int sessionID)
        {

            FBAData.Task.ChangeOrder(id, order);

            return RedirectToActionPermanent(Actions.Tasks.Name, new { sessionID = sessionID });
        }

        [Authorize]
        public IActionResult Task(int ID, int sessionID, bool read = true)
        {
            read = ID == 0 ? false : read;

            FBAData.Session session = FBAData.Session.GetSession(sessionID, true);




            TasksModel model = new TasksModel
            {
                SessionID = sessionID,
                ProcessID = session.ProcessID,
                SessionName = session.Name,
                ProgramName = session.Process.Name,
                ReadOnly = read
            };
            if (ID == 0)
            {
                model.Task = new FBAData.Task { SessionID = sessionID,ProcessID=session.ProcessID };
            }
            else
            {
                model.Task = FBAData.Task.GetTask(ID);
                if (read)
                {
                    model.TaskDocuments = FBAData.Task.GetDocuments(ID);
                }
            }

            if (!read)
            {
                var taskTypes = FBAData.Task.GetTaskTypes();
                var tools = FBAData.Tool.GetTools();
                model.TaskTypes = taskTypes.Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();
                model.Tools = tools.Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();

            }


            if (!read)
            {
                return View(model);
            }
            else
            {
                return View("TaskDetail", model);
            }

        }


        [Authorize]
        [HttpPost]
        public IActionResult Task(TasksModel item, IFormFile template, bool isNext = false)
        {
          

            int id = FBAData.Task.Save(item.Task);
            return RedirectToActionPermanent(Actions.Tasks.Name, new { SessionID = item.Task.SessionID });

        }

        [Authorize]
        [HttpPost]
        public IActionResult TaskDocument(int taskID, int sessionID, IFormFile template)
        {

            FBAData.Document document = new FBAData.Document
            {

                FileName = template.FileName,
                FileType = template.ContentType,
                Path = string.Format("/docs/tasks/{0}/{1}", taskID, template.FileName)
            };

            if (template.Length > 0)
            {

                string uploads = Path.Combine(Environment.WebRootPath, "docs");
                uploads = Path.Combine(uploads, "tasks");
                uploads = Path.Combine(uploads, taskID.ToString());

                string filePath = Path.Combine(uploads, template.FileName);
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                document.Path = filePath;
                using (Stream fileStream = new FileStream(document.Path, FileMode.Create))
                {
                    template.CopyTo(fileStream);
                }

                int docID = FBAData.Document.Upload(document);
                FBAData.TaskDocument taskDocument = new FBAData.TaskDocument { DocumentID = docID, TaskID = taskID };
                FBAData.TaskDocument.Save(taskDocument);


            }



            return RedirectToActionPermanent(Actions.Task.Name, new { ID = taskID, SessionID = sessionID });
        }

        [Authorize]
        [HttpPost]
        public IActionResult TaskDocumentDelete(int taskDocumentID, int documentId, int taskID)
        {
            FBAData.Task t = FBAData.Task.GetTask(taskID);
            var template = FBAData.Document.Download(documentId);
            try
            {


                bool success = FBAData.TaskDocument.Delete(taskDocumentID);
                if (success)
                {

                    System.IO.File.Delete(template.Path);
                }
            }
            catch (Exception ex)
            {


            }



            return RedirectToActionPermanent(Actions.Task.Name, new { ID = taskID, SessionID = t.SessionID });
        }
    }
}
