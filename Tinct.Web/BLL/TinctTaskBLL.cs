using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tinct.TinctTaskMangement.TinctWork;
using Tinct.Web.TinctTaskM;

namespace Tinct.Web.BLL
{
    public class TinctTaskBLL
    {
      

        public IEnumerable<TinctTask> GetAllTasks() 
        {
            return  TinctTaskBridge.TaskManger.TaskRepository.GetCurrentTinctTasks();
        }

    

    }
}