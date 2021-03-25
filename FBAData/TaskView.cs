using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBAData
{
    public class TaskView:Task
    {
        public string ProgramName { get; set; }
        public string SessionName { get; set; }

        public static TaskView GetTaskView(int ID) 
        {
            using (var a = new TaskContext())
            {
                Task task = a.Task.Include(x=>x.Session.Process).Where(x => x.ID == ID).First();
                TaskView tv = (TaskView)task;
                tv.SessionName = tv.Session.Name;
                tv.ProgramName = tv.Session.Process.Name;
                tv.Session = null;
                
                return tv;
            }
        }
    }
}
