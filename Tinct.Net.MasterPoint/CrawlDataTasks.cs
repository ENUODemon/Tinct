using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Net.MasterPointConsole
{
    public class CrawlDataTasks
    {
        public int GrpLeaderTaskTotalCount = 0;
        public int GrpLeaderCompleteCount = 0;

        public void GennerateTinctTaskManeger()
        {

            //tm.TaskRepos.AddTinctTask(GennerateCrawlWentMembersTask());

            //GennerateCrawlGroupsTask();
             GennerateArchiveEventsTask();
            //tm.TaskRepository.AddTinctTask(CrawlGroupTask);

        }

        public TinctTask GennerateCrawlGroupsTask()
        {
            //MeetupDataQuery.SaveExecuteTime();
            TinctTask t1 = new TinctTask();
            t1.Context.TaskData = "";
            t1.Context.ControllerName = "NewMeetup";
            t1.Context.ActionName = "LoadMeetupGroups";
            //t1.Context.TaskID = t1.ID;
            t1.TinctTaskCompleted += CreateLeaderTasks;
            t1.Status = TinctTaskStatus.WaittingToRun;
            return t1;
        }
        public void CreateLeaderTasks(object sender, TinctTaskEventArgs args)
        {
          //  GennerateCrawlLeaderTasks(null);
        }
        //public void GennerateCrawlLeaderTasks(TinctTask WaitTask)
        //{
        //   // int GroupsCount  = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctGrpUrlName_0419]");
        //    int EachTaskIdCount = 400;
        //    int taskCount;
        //    taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
        //    GrpLeaderTaskTotalCount = taskCount;
        //    for (int i = 0; i < taskCount; i++)
        //    {
        //        int startID = EachTaskIdCount * i;
        //        int endID = EachTaskIdCount * i + EachTaskIdCount;
        //        TinctTask t1 = new TinctTask();
        //        t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
        //        t1.Context.ControllerName = "NewMeetup";
        //        t1.Context.ActionName = "LoadGroupLeaders";
        //        if (WaitTask != null)
        //        {
        //            t1.WaittingTinctTasks.Add(WaitTask);
        //        }
        //        //t1.TinctTaskCompleted += CreateSimilarGroupTasks;
        //        t1.TinctTaskCompleted += null;
        //        t1.Status = TinctTaskStatus.WaittingToRun;
        //    }           
        //}

        public TinctTask GennerateArchiveEventsTask()
        {
            TinctTask t1 = new TinctTask();
            DateTime startTime = new DateTime(2016, 4, 5);
            DateTime endTime = startTime.AddHours(0.5);
            t1.Context.TaskData = string.Format("StartTime:{0},EndTime:{1}", startTime, endTime);
            t1.Context.ControllerName = "GitHubArchive1";
            t1.Context.ActionName = "LoadArchiveData";
            t1.TinctTaskCompleted += new EventHandler<TinctTaskEventArgs>(CreateApiCallTasks);
            t1.Status = TinctTaskStatus.WaittingToRun;
            return t1;
        }

        public void CreateApiCallTasks(object sender, TinctTaskEventArgs args)
        {
            GennerateApiCallTasks();
        }
        public void GennerateApiCallTasks()
        { 
            int EachTaskIdCount = 10000;
            int TempTableCount = 10;
            int taskCount;
            ////temp datas for test
            int activeUserCount = 20;
            EachTaskIdCount = 10;
            //int activeUserCount = GitHubDataQuery.GetDataCount("[dbo].[1210_ActiveUsers]");
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(activeUserCount) / EachTaskIdCount).ToString());
            for (int i = 0; i < taskCount; i++)
            {
                int startUserID = EachTaskIdCount * i;
                int endUserID = EachTaskIdCount * i + EachTaskIdCount;
                int tempTableIndex = (i + 1) % TempTableCount == 0 ? TempTableCount : (i + 1) % TempTableCount;
                //string tempTableName = string.Format("[dbo].[1210_UserTemp{0}]", tempTableIndex);
                string tempTableName = string.Format("[dbo].[1210_UserTempTest]");
                TinctTask t = new TinctTask();
                t.Context.TaskData = string.Format("Type:User,StartUserID:{0},EndUserID:{1},TempTableName:{2}", startUserID, endUserID, tempTableName);
                t.Context.ControllerName = "NewGitHub";
                t.Context.ActionName = "LoadActiveUsersAndReposByApi";
                t.Status = TinctTaskStatus.WaittingToRun;
            }

            int activeReposCount = 20;
            //int activeReposCount = GitHubDataQuery.GetDataCount("[dbo].[1210_ActiveRepos]");
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(activeReposCount) / EachTaskIdCount).ToString());
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                int tempTableIndex = (i + 1) % TempTableCount == 0 ? TempTableCount : (i + 1) % TempTableCount;
                string tempTableName = string.Format("[dbo].[1210_ReposTempTest]");
                TinctTask t = new TinctTask();
                t.Context.TaskData = string.Format("Type:Repos,StartID:{0},EndID:{1},TempTableName:{2}", startID, endID, tempTableName);
                t.Context.ControllerName = "NewGitHub";
                t.Context.ActionName = "LoadActiveUsersAndReposByApi";
                t.Status = TinctTaskStatus.WaittingToRun;

            }
        }
    }
}
