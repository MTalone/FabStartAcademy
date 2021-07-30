using FBAData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabStartAcademy.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        public IActionResult LearningCategories()
        {
            List<LearningCategory> lc = LearningCategory.GetList();

            return View(lc);
        
        }

        public IActionResult LearningCategory2()
        {
            List<LearningCategory> lc = LearningCategory.GetList();

            return View(lc);
        }
    }
}
