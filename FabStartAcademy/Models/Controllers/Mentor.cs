using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models.Controllers
{
    public class Mentor
    {
        public static string Controller { get { return "Mentor"; } }
        public static int ControllerID { get { return 4; } }

        public class Actions 
        {
            public static Action Dashboard { get { return new Action { ID = 1, Controller = Mentor.Controller, Name = "Dashboard" }; } }
            public static Action Program { get { return new Action { ID = 1, Controller = Mentor.Controller, Name = "Program" }; } }
        }
    }
}
