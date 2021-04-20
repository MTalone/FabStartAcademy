using FabStartAcademy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FabStartAcademy.Controllers
{
    public class BackOfficeController : Controller
    {

        private IWebHostEnvironment Environment;

        public BackOfficeController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DashBoard()
        {
            DashBoardModel dash = new DashBoardModel();

            return View(dash);
        }

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

            var ps = FBAData.Program.GetPrograms(0);

            string programDefaultPath = "/imgs/placeholder-group.png";
            model.Programs = new List<ProgramItem>();
            model.GetProgramItems(ps, programDefaultPath, Environment.WebRootPath);

            return View(model);


        }

        public IActionResult Program(int ID = 0, bool read = true)
        {
            ProgramItem groupItem = new ProgramItem();
            read = ID == 0 ? false : read;

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
                    ProcessID = group.ProcessID
                };

            }
            groupItem.IsRead = read;

            ProgramsModel model = new ProgramsModel();
            model.Program = groupItem;
            model.Processes = FBAData.Process.GetProcesses(0).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();

            return View(model);
        }

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
                LogoID = documentID
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

        public IActionResult Teams(int programID)
        {
            TeamModel model = new TeamModel(programID,Environment.WebRootPath, "/imgs/placeholder-team.png");


            return View(model);
        }

        public IActionResult Methods()
        {
            return View(new ProgramModel(0));

        }

        public IActionResult Method(int ID = 0, bool read = true)
        {
            ProcessItem item = new ProcessItem();

            read = ID == 0 ? false : read;

            if (ID > 0)
            {
                var i = FBAData.Process.GetProcess(ID);



                item = new ProcessItem { ID = i.ID, Description = i.Description, Title = i.Name, LogoID = i.LogoID, Logo = i.Logo == null ? new byte[0] : System.IO.File.ReadAllBytes(i.Logo.Path) };
                item.IsReadOnly = read;
            }

            return View(item);
        }

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


            int id = FBAData.Process.Save(new FBAData.Process { ID = item.ID, Name = item.Title, Description = item.Description, LogoID = documentID });

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

        [HttpPost]
        public IActionResult ProgramDelete(int id)
        {

            FBAData.Process.Delete(id);

            return RedirectToActionPermanent(Actions.Methods.Name);
        }

        public IActionResult Sessions(int ProgramID)
        {

            SessionModel model = new SessionModel(ProgramID);

            return View(model);
        }

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

        [HttpPost]
        public IActionResult Session(SessionItem item)
        {
            FBAData.Session.SaveSession(new FBAData.Session { ID = item.ID, Name = item.Title, Description = item.Description, ProcessID = item.ProcessID });

            return RedirectToActionPermanent(Actions.Methods.Name);
        }

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
        [HttpPost]
        public IActionResult TaskDelete(int id, int sessionID)
        {

            FBAData.Task.DeleteTask(id);

            return RedirectToActionPermanent(Actions.Tasks.Name, new { sessionID = sessionID });
        }

        [HttpPost]
        public IActionResult TaskOrder(int id, int order, int sessionID)
        {

            FBAData.Task.ChangeOrder(id, order);

            return RedirectToActionPermanent(Actions.Tasks.Name, new { sessionID = sessionID });
        }


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
                model.Task = new FBAData.Task { SessionID = sessionID };
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



        [HttpPost]
        public IActionResult Task(TasksModel item, IFormFile template, bool isNext = false)
        {
            int? documentID = item.Task.DocumentID;

            if (template != null)
            {
                if (template.Length > 0)
                {
                    FBAData.Document document = new FBAData.Document
                    {
                        ID = documentID.HasValue ? documentID.Value : 0,
                        FileName = template.FileName,
                        FileType = template.ContentType
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        template.CopyTo(memoryStream);

                        document.Content = memoryStream.ToArray();

                    }

                    documentID = FBAData.Document.Upload(document);
                }

            }
            item.Task.DocumentID = documentID;

            int id = FBAData.Task.Save(item.Task);
            return RedirectToActionPermanent(Actions.Tasks.Name, new { SessionID = item.Task.SessionID });

        }

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
    }
}
