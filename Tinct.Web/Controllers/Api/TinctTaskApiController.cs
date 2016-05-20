using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tinct.TinctTaskMangement.TinctWork;
using Tinct.Web.BLL;

namespace Tinct.Web.Controllers
{
    public class TinctTaskApiController : ApiController
    {


        [HttpGet]
        [HttpPost]
        public IEnumerable<TinctTask> SearchTinctTask() 
        {
            string id = string.Empty;
            TinctTaskBLL bll = new TinctTaskBLL();
            return null;
           // return bll.SerachTask(id);
        }




    }
}
