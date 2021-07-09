using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class Actions
    {
        public static Action Dashboard { get { return new Action { ID = 0, Controller = "BackOffice", Name = "Dashboard" }; } }
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
        public static Action TaskOrder { get { return new Action { ID = 11, Controller = "BackOffice", Name = "TaskOrder" }; } }

        public static Action Download { get { return new Action { ID = 12, Controller = "File", Name = "Download" }; } }
        public static Action Teams { get { return new Action { ID = 13, Controller = "BackOffice", Name = "Teams" }; } }

        public static Action Team { get { return new Action { ID = 14, Controller = "BackOffice", Name = "Team" }; } }

        public static Action Members { get { return new Action { ID = 15, Controller = "BackOffice", Name = "Members" }; } }
        public static Action Member { get { return new Action { ID = 16, Controller = "BackOffice", Name = "Member" }; } }

        public static Action SessionOrder { get { return new Action { ID = 17, Controller = "BackOffice", Name = "SessionOrder" }; } }

        public static Action TaskDocumentDelete { get { return new Action { ID = 18, Controller = "BackOffice", Name = "TaskDocumentDelete" }; } }

        public static Action Home { get { return new Action { ID = 19, Controller = "Home", Name = "Index" }; } }

        public static Action Partners { get { return new Action { ID = 20, Controller = "Partner", Name = "Partners" }; } }
        public static Action Partner { get { return new Action { ID = 21, Controller = "Partner", Name = "Partner" }; } }
        public static Action PartnerSuspend { get { return new Action { ID = 22, Controller = "Partner", Name = "Suspend" }; } }
        public static Action PartnerDelete { get { return new Action { ID = 23, Controller = "Partner", Name = "Delete" }; } }

        public static Action LogOut { get { return new Action { ID = -1, Controller = "Account", Name = "LogOut" }; } }
        public static Action PartnerMethods { get { return new Action { ID = 23, Controller = "Partner", Name = "PartnerMethods" }; } }
    }

    public class Action
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }

    }
}
