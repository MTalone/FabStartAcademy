using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Download(int documentID)
        {

            var d = FBAData.Document.Download(documentID);

            if (!(d.Path is  null)&& d.Path!=string.Empty ) 
            {
                d.Content = System.IO.File.ReadAllBytes(d.Path);

            }
            

            return File(d.Content, d.FileType, d.FileName);

        }
    }
}
