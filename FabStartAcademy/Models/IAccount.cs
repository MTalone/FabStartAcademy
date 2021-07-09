using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Models
{
    interface IAccount
    {
        Account.Account CurrentAccount { get; set; }
    }
}
