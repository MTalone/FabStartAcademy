using FabStartAcademy.Models;
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

            model.Programs = FBAData.Program.GetPrograms(0).Select(x => new ProgramItem
            {
                Title = x.Name,
                Image = "/imgs/thumb_351_image_big.png",
                Process = x.Process != null ? x.Process.Name : string.Empty,
                ID = x.ID
            }).ToList();

            return View(model);
        }

        public IActionResult Program(int ID = 0, bool read = true)
        {
            ProgramItem groupItem = new ProgramItem();
            read = ID == 0 ? false : read;

            if (ID > 0)
            {
                var group = FBAData.Program.GetProgram(ID);
                groupItem = new ProgramItem { Title = group.Name, Process = group.Process != null ? group.Process.Name : string.Empty, ID = group.ID, Code = group.Code,Description=group.Description };

            }
            groupItem.IsRead = read;

            ProgramsModel model = new ProgramsModel();
            model.Program = groupItem;
            model.Processes = FBAData.Process.GetProcesses(0).Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Program(ProgramsModel item)
        {
            FBAData.Program.Save(new FBAData.Program
            {
                ID = item.Program.ID,
                Name = item.Program.Title,
                Description = item.Program.Description,
                ProcessID = item.Program.ProcessID,
                Code = item.Program.Code
            });

            return RedirectToActionPermanent(Actions.Programs.Name);


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
                item = new ProcessItem { ID = i.ID, Description = i.Description, Title = i.Name, LogoID = i.LogoID, Logo = i.Logo == null ? new byte[0] : i.Logo.Content };
                item.IsReadOnly = read;
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult Method(ProcessItem item, IFormFile Logo, bool isNext = false)
        {
            int? documentID = item.LogoID;

            if (Logo != null)
            {
                if (Logo.Length > 0)
                {
                    FBAData.Document document = new FBAData.Document
                    {
                        ID = item.LogoID.HasValue ? item.LogoID.Value : 0,
                        FileName = Logo.FileName,
                        FileType = Logo.ContentType
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        Logo.CopyTo(memoryStream);

                        document.Content = memoryStream.ToArray();

                    }

                    documentID = FBAData.Document.Upload(document);
                }

            }


            int id = FBAData.Process.Save(new FBAData.Process { ID = item.ID, Name = item.Title, Description = item.Description, LogoID = documentID });

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


        public IActionResult Task(int ID, int sessionID,bool isReadOnly=true)
        {
            isReadOnly = false; //ID == 0 ? false : isReadOnly;

            FBAData.Session session = FBAData.Session.GetSession(sessionID, true);

            var taskTypes = FBAData.Task.GetTaskTypes();
            var tools = FBAData.Tool.GetTools();


            TasksModel model = new TasksModel
            {
                SessionID = sessionID,
                ProcessID = session.ProcessID,
                SessionName = session.Name,
                ProgramName = session.Process.Name,
                TaskTypes = taskTypes.Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList(),
                Tools = tools.Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() }).ToList(),
                ReadOnly=isReadOnly
            };
            if (ID == 0)
            {
                model.Task = new FBAData.Task { SessionID = sessionID };
            }
            else { model.Task = FBAData.Task.GetTask(ID); }



            return View(model);
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

        
    }
}
