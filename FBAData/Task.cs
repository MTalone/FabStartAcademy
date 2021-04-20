using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace FBAData
{
    public class Task
    {
        public enum TaskTypes
        {
            Template=1,
            Tool=2,
            URL=3,

        
        }

        public Session Session { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SessionID { get; set; }

        public string Instructions { get; set; }

        public int Order { get; set; }
        [Display(Name = "IsEvaluated", ResourceType = typeof(Resources.FabStartAcademy))]
        public bool IsEvaluated { get; set; }
        [Display(Name = "TaskType", ResourceType = typeof(Resources.FabStartAcademy))]
        public int TaskType { get; set; }

        public int? DocumentID { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "AvailableOn", ResourceType = typeof(Resources.FabStartAcademy))]
        public DateTime? AvailableOn { get; set; }

        public int? ToolID { get; set; }

        public static List<Task> GetTasks(int sessionID)
        {
            using (var a = new TaskContext())
            {
                try
                {

                    var query = a.Task.Where(x => x.SessionID == sessionID).OrderBy(x => x.Order).ToList();



                    return query.ToList();

                }
                catch (Exception ex)
                {

                    return null;
                }

            }
        }

        public static int Save(Task task)
        {
            using (var a = new TaskContext())
            {
                if (task.ID == 0)
                {
                    task.Order = a.Task.Where(x => x.SessionID == task.SessionID).Count() + 1;

                    var newgroup = a.Task.Add(task);

                    a.SaveChanges();
                    return newgroup.Entity.ID;
                }
                else
                {
                    var changes = a.Update(task);
                    a.SaveChanges();
                    return changes.Entity.ID;

                }

            }
        }

        public static int ChangeOrder(int taskID, int order)
        {

            using (var a = new TaskContext())
            {
                Task task = a.Task.Where(x => x.ID == taskID).First();




                if (task.Order == order)
                    return 0;

                List<Task> tasks;
                tasks = a.Task.Where(x => x.SessionID == task.SessionID && x.ID!=taskID).OrderBy(x=>x.Order).ToList();
                int ret = tasks.Count;
                foreach (var item in tasks)
                {

                    if (item.Order == order)
                    {
                        if (order > task.Order)
                        {


                            item.Order--;
                            



                        }
                        else
                        {
                            item.Order++;
                            

                        }
                        a.Update(item);
                    }
                   
                    else{


                    }
                }


                task.Order = order;
                a.Update(task);
                a.SaveChanges();
                return ret + 1;
            }
        }



        public static Task GetTask(int ID)
        {
            using (var a = new TaskContext())
            {
                Task task = a.Task.Where(x => x.ID == ID).First();
                return task;
            }
        }

        public static void DeleteTask(int id)
        {
            using (var a = new TaskContext())
            {
                var task = GetTask(id);
                int order = task.Order;
                var tasksOrder = a.Task.Where(a => a.Order > order).ToList();
                foreach (var item in tasksOrder)
                {
                    item.Order--;
                    a.Update(item);
                }
                a.Task.Remove(task);
                a.SaveChanges();
            }
        }

        public static Dictionary<int,string> GetTaskTypes() 
        {
            Dictionary<int, string> toRet=new Dictionary<int, string>();

            toRet.Add((int)TaskTypes.Template, Resources.FabStartAcademy.TaskType_Template);
            toRet.Add((int)TaskTypes.Tool, Resources.FabStartAcademy.TaskType_Tool);

            return toRet;
        }

        public static List<TaskDocumentView> GetDocuments(int iD)
        {
            using (var a = new TaskContext())
            {
                return a.TaskDocument.Include(x => x.Document).Select(x => new TaskDocumentView { FileName = x.Document.FileName, DocumentID = x.DocumentID, TaskID = x.TaskID }).ToList();
            }


               
        }
    }



}
