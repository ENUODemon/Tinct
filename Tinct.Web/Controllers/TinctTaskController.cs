using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tinct.TinctTaskMangement.TinctWork;
using Tinct.Web.BLL;
using Tinct.Web.TinctTaskM;

namespace Tinct.Web.Controllers
{
    public class TinctTaskController : Controller
    {
        private TinctTaskBLL bll;

        public TinctTaskController()
        {
            bll= new TinctTaskBLL();
         
           
        }
        public ActionResult Tasks()
        {

            ViewBag.TaskInfosLists = bll.GetAllTasks();
            return View("Tasks");
        }




	}
}