using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Task;
//using Tinct.TinctTaskMangement.Base;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.Net.MasterPointConsole
{
    public class MeetupTasks
    {
        public  int GrpLeaderTaskTotalCount = 0;
        public  int GrpLeaderCompleteCount = 0;
        public  int SimilarGrpTaskTotalCount = 0;
        public  int SimilarGrpCompleteCount = 0;
        public  int EventTaskTotalCount = 0;
        public  int EventTaskCompleteCount = 0;
        public  int RsvpTaskTotalCount = 0;
        public  int RsvpTaskCompleteCount = 0;
        public  int MemberTaskTotalCount = 0;
        public  int MemberTaskCompleteCount = 0;
        public  void GennerateTinctTasks()
        {
            GennerateCrawlGroupsTask();
        }

        public void GennerateCrawlGroupsTask()
        {
            MeetupDataQuery.SaveExecuteTime();
            TinctTask t1 = new TinctTask();
            t1.Status = TinctTaskStatus.WaittingToRun;
            t1.Context.TaskData = "";
            t1.Context.ControllerName = "NewMeetup";
            t1.Context.ActionName = "LoadMeetupGroups";
            t1.TinctTaskCompleted += CreateLeaderTasks;
        }
        public void CreateLeaderTasks(object sender, TinctTaskEventArgs args)
        {
            GennerateCrawlLeaderTasks(null);       
        }
        public void GennerateCrawlLeaderTasks(TinctTask WaitTask)
        {
            int GroupsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctGrpUrlName]");
            int EachTaskIdCount = 400;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
            GrpLeaderTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadGroupLeaders";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += CreateSimilarGroupTasks;
            }  
        }
        public void CreateSimilarGroupTasks(object sender, TinctTaskEventArgs args)
        {
            GrpLeaderCompleteCount++;
            if (GrpLeaderCompleteCount == GrpLeaderTaskTotalCount)
            {
                GennerateCrawlSimilarGrpsTask(null);
            }
        }
        public void GennerateCrawlSimilarGrpsTask(TinctTask WaitTask)
        {
            int GroupsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctGrpUrlName]");
            int EachTaskIdCount = 400;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
            SimilarGrpTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadMeetupSimilarGrps";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += CreateEventTasks;
            }
        }
        public void CreateEventTasks(object sender, TinctTaskEventArgs args)
        {
            SimilarGrpCompleteCount++;
            if (SimilarGrpCompleteCount == SimilarGrpTaskTotalCount)
            {
               GennerateCrawlEventsTasks(null);
            }
        }
        public List<TinctTask> GennerateCrawlEventsTasks(TinctTask WaitTask)
        {
            List<TinctTask> EventTaskList = new List<TinctTask>();
            int GroupsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctGroup]");
            int EachTaskIdCount = 400;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
            EventTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadMeetupEvents";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += SaveDistinctEventIds;
                EventTaskList.Add(t1);
            }
            return EventTaskList;
        }
        public void SaveDistinctEventIds(object sender, TinctTaskEventArgs args)
        {
            EventTaskCompleteCount++;
            if (EventTaskCompleteCount == EventTaskTotalCount)
            {
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = null;
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "InsertDistinctEvent";        
                t1.TinctTaskCompleted += CreateRsvpTasks;
            }
        }
        public void CreateRsvpTasks(object sender, TinctTaskEventArgs args)
        {
            GennerateCrawlRsvpsTask(null);
        }
        public void GennerateCrawlRsvpsTask(TinctTask WaitTask)
        {
            int EventsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctEvent]");
            int EachTaskIdCount = 2000;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(EventsCount) / EachTaskIdCount).ToString());
            RsvpTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName: ", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadMeetupRsvps";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += CreateMemberTasks;
            }
        }
        public void CreateMemberTasks(object sender, TinctTaskEventArgs args)
        {
            RsvpTaskCompleteCount++;
            if (RsvpTaskCompleteCount == RsvpTaskTotalCount)
            {
                GennerateCrawlMembersTasks(null);
            }
        }
        public void GennerateCrawlMembersTasks(TinctTask WaitTask)
        {  
            int GroupsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctGroup]");
            int EachTaskIdCount = 400;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
            MemberTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadMeetupMembers";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += CreateWentMemberTasks;
            }
        }
        public void CreateWentMemberTasks(object sender, TinctTaskEventArgs args)
        {
            MemberTaskCompleteCount++;
            if (MemberTaskCompleteCount == MemberTaskTotalCount)
            {
               GennerateCrawlWentMemberTasks(null);
            }
        }

        public void GennerateCrawlWentMemberTasks(TinctTask WaitTask)
        {
            List<TinctTask> EventTaskList = new List<TinctTask>();
            int GroupsCount = MeetupDataQuery.GetDataCount("[dbo].[0127_DistinctPastEvent]");
            int EachTaskIdCount = 1000;
            int taskCount;
            taskCount = int.Parse(Math.Ceiling(Convert.ToDecimal(GroupsCount) / EachTaskIdCount).ToString());
            EventTaskTotalCount = taskCount;
            for (int i = 0; i < taskCount; i++)
            {
                int startID = EachTaskIdCount * i;
                int endID = EachTaskIdCount * i + EachTaskIdCount;
                TinctTask t1 = new TinctTask();
                t1.Status = TinctTaskStatus.WaittingToRun;
                t1.Context.TaskData = string.Format("Type:Event,StartID:{0},EndID:{1},TempTableName:", startID, endID);
                t1.Context.ControllerName = "NewMeetup";
                t1.Context.ActionName = "LoadWentMembers";
                if (WaitTask != null)
                {
                    t1.WaittingTinctTasks.Add(WaitTask);
                }
                t1.TinctTaskCompleted += null;
            }
        }

    }
}
