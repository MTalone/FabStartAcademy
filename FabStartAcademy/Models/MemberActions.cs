﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    public class Member
    {
        public static string Controller { get { return "Member"; } }
        public static int ControllerID { get { return 3; } }

        public class Actions
        {

            public static Action Dashboard { get { return new Action { ID = 1, Controller = Member.Controller, Name = "Dashboard" }; } }
        }

    }
}
