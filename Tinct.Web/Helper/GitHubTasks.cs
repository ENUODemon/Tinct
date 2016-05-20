using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
//using Tinct.TinctTaskMangement.Base;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Net.MasterPointConsole
{
    public class GitHubTasks
    {
        public void GennerateTinctTasks()
        {
            GennerateArchiveEventsTask();
            //GennerateApiCallTasks(null, null);
            //tre.AddTinctTask(GennerateArchiveEventsTask());
        }
        public void GennerateArchiveEventsTask()
        {
            TinctTask t1 = new TinctTask();
            //DateTime startTime = new DateTime(2016, 5, 10);
            //DateTime endTime = new DateTime(2016, 5, 16);

            DateTime startTime = DateTime.Parse(ConfigurationManager.AppSettings["GitHubStartTime"]);
            DateTime endTime = DateTime.Parse(ConfigurationManager.AppSettings["GitHubEndTime"]);
           
            t1.Status = TinctTaskStatus.WaittingToRun;
            t1.Context.TaskData = string.Format("StartTime:{0},EndTime:{1}", startTime, endTime);
            t1.Context.ControllerName = "GitHubArchive1";
            t1.Context.ActionName = "LoadArchiveData";
            //t1.TinctTaskCompleted += null;
            t1.TinctTaskCompleted += new EventHandler<TinctTaskEventArgs>(GennerateApiCallTasks);
        }
        public void GennerateApiCallTasks(object sender, TinctTaskEventArgs args)
        {
            int EachTaskIdCount = 10000;
            int TempTableCount = 10;
            int taskCount;
            ////temp datas for test
            //int activeUserCount = 100;
            int activeUserCount = GitHubDataQuery.GetDataCount("[dbo].[1210_ActiveUsers]");
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(activeUserCount) / EachTaskIdCount).ToString());
            for (int i = 0; i < taskCount; i++)
            {
                int startUserID = EachTaskIdCount * i;
                int endUserID = EachTaskIdCount * i + EachTaskIdCount;
                int tempTableIndex = (i + 1) % TempTableCount == 0 ? TempTableCount : (i + 1) % TempTableCount;
                string tempTableName = string.Format("[dbo].[1210_UserTemp{0}]", tempTableIndex);
                TinctTask t = new TinctTask();
                t.Status = TinctTaskStatus.WaittingToRun;
                t.Context.TaskData = string.Format("Type:User,StartUserID:{0},EndUserID:{1},TempTableName:{2}", startUserID, endUserID, tempTableName);
                t.Context.ControllerName = "NewGitHub";
                t.Context.ActionName = "LoadActiveUsersAndReposByApi";
            }

            //int activeReposCount = 100;
            int activeReposCount = GitHubDataQuery.GetDataCount("[dbo].[1210_ActiveRepos]");
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(activeReposCount) / EachTaskIdCount).ToString());
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                int tempTableIndex = (i + 1) % TempTableCount == 0 ? TempTableCount : (i + 1) % TempTableCount;
                string tempTableName = string.Format("[dbo].[1210_ReposTemp{0}]", tempTableIndex);
                TinctTask t = new TinctTask();
                t.Status = TinctTaskStatus.WaittingToRun;
                t.Context.TaskData = string.Format("Type:Repos,StartID:{0},EndID:{1},TempTableName:{2}", startID, endID, tempTableName);
                t.Context.ControllerName = "NewGitHub";
                t.Context.ActionName = "LoadActiveUsersAndReposByApi";
            }
        }
    }
}
