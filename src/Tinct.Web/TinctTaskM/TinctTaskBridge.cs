using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Web.TinctTaskM
{

    public static class TinctTaskBridge
    {
        public static TinctTaskMangement.TinctWork.TinctTaskManeger TaskManger= new TinctTaskManeger();
        public static ITinctTaskRepository taskRepos = new TinctTaskRepository(); 
        static TinctTaskBridge() 
        {
            TaskManger.TaskRepository = taskRepos;
        }

    }
}