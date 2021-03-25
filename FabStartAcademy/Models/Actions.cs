﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class Actions
    {
       public static Action Programs { get { return new Action {ID=1, Controller = "BackOffice", Name = "Programs" }; } }
        public static Action Program { get { return new Action { ID = 2, Controller = "BackOffice", Name = "Program" }; } }
        public static Action Methods { get { return new Action { ID = 3, Controller = "BackOffice", Name = "Methods" }; } }
        public static Action Method { get { return new Action { ID = 4, Controller = "BackOffice", Name = "Method" }; } }
        public static Action MethodDelete { get { return new Action { ID = 11, Controller = "BackOffice", Name = "MethodDelete" }; } }
        public static Action Sessions { get { return new Action { ID = 5, Controller = "BackOffice", Name = "Sessions" }; } }
        public static Action Session { get { return new Action { ID = 6, Controller = "BackOffice", Name = "Session" }; } }

        public static Action Tasks { get { return new Action { ID = 7, Controller = "BackOffice", Name = "Tasks" }; } }

        public static Action Task { get { return new Action { ID = 8, Controller = "BackOffice", Name = "Task" }; } }

        public static Action TaskDelete { get { return new Action { ID = 9, Controller = "BackOffice", Name = "TaskDelete" }; } }
        public static Action TaskOrder { get { return new Action { ID = 10, Controller = "BackOffice", Name = "TaskOrder" }; } }
    }

    public class Action
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }

    }
}
