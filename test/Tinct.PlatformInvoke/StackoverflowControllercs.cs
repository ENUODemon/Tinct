using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.Slave.ClientCrawler;
using Tinct.Slave.MaintainPlan;
using Tinct.Slave.MaintainPlan.Model;
using Tinct.Slave.Model.StackExchange;
using Tinct.Slave.PageAnalysis;

namespace Tinct.PlatformController
{
    public  class StackoverflowControllercs
    {
        public void LoadQuestionsData(IList<Uri> addresses, bool IsTruncateTempDB = true)
        {

            WebHttpCrawlerClient client = new WebHttpCrawlerClient();

            client.SetRequestDetail = (x) =>
            {
                x._HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                x._HttpWebRequest.Referer = "https://api.stackexchange.com/docs/questions";
            };

            WebThreadDesigation t = new WebThreadDesigation(client);
            t.EventWaitHandleName = "WebThreadDesigation";

            StackoverflowPageAnalysis ana = new StackoverflowPageAnalysis();
            AnalysisThreadDesigation<QuestionInfo> a = new AnalysisThreadDesigation<QuestionInfo>(ana);
            a.EventWaitHandleName = "StackoverflowPageAnalysis";
            DBThreadDesigation d = new DBThreadDesigation();
            d.EventWaitHandleName = "DBThreadDesigation";
            CrawlerSyncObject syscobject = new CrawlerSyncObject();
            AnalysisSyncObject anasissyncObject = new AnalysisSyncObject();

            SendImformation infos = new SendImformation();
            infos.ConnectString = "data source=CIA-SH-svr-sis;initial catalog=stackoverflowNew;integrated security=True;";

            infos.EventhandlerName = "QuestionTemp1_Tinct";
            infos.DestinationtableName = "QuestionTemp1_Tinct";
            infos.IsWantToMerged = false;
            infos.MergeSQlSPName = "";

            EventWaitHandle eventWaitHandle1 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, t.EventWaitHandleName);
            eventWaitHandle1.Reset();
            EventWaitHandle eventWaitHandle2 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, a.EventWaitHandleName);
            eventWaitHandle2.Reset();
            EventWaitHandle eventWaitHandle3 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, d.EventWaitHandleName);
            eventWaitHandle3.Reset();
            EventWaitHandle eventWaitHandle4 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, infos.EventhandlerName);
            eventWaitHandle4.Reset();
            Task.Run(() => { t.CrawlerUris(addresses, 10, syscobject); });
            Task.Run(() => { a.AnalysisResults(t.resultlists, 5, syscobject, anasissyncObject); });
            Task.Run(() =>
            {
                d.SendDispatchDatas<QuestionInfo>(ref a.datas, 500, ref anasissyncObject, infos,
                    null);
            });
            eventWaitHandle1.WaitOne();
            eventWaitHandle2.WaitOne();
            eventWaitHandle3.WaitOne();
            eventWaitHandle4.WaitOne();
        }

        public void LoadAnswersData(IList<Uri> addresses, bool IsTruncateTempDB = true)
        {

            WebHttpCrawlerClient client = new WebHttpCrawlerClient();

            client.SetRequestDetail = (x) =>
            {
                x._HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                x._HttpWebRequest.Referer = "https://api.stackexchange.com/docs/answers";
            };

            WebThreadDesigation t = new WebThreadDesigation(client);
            t.EventWaitHandleName = "WebThreadDesigation";

            StackoverflowPageAnalysis ana = new StackoverflowPageAnalysis();
            AnalysisThreadDesigation<AnswerInfo> a = new AnalysisThreadDesigation<AnswerInfo>(ana);
            a.EventWaitHandleName = "StackoverflowPageAnalysis";
            DBThreadDesigation d = new DBThreadDesigation();
            d.EventWaitHandleName = "DBThreadDesigation";
            CrawlerSyncObject syscobject = new CrawlerSyncObject();
            AnalysisSyncObject anasissyncObject = new AnalysisSyncObject();

            SendImformation infos = new SendImformation();
            infos.ConnectString = "data source=CIA-SH-svr-sis;initial catalog=stackoverflowNew;integrated security=True;"; 

            infos.EventhandlerName = "AnswerTemp1_Tinct";
            infos.DestinationtableName = "AnswerTemp1_Tinct";
            infos.IsWantToMerged = false;
            infos.MergeSQlSPName = "";

            EventWaitHandle eventWaitHandle1 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, t.EventWaitHandleName);
            eventWaitHandle1.Reset();
            EventWaitHandle eventWaitHandle2 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, a.EventWaitHandleName);
            eventWaitHandle2.Reset();
            EventWaitHandle eventWaitHandle3 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, d.EventWaitHandleName);
            eventWaitHandle3.Reset();
            EventWaitHandle eventWaitHandle4 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, infos.EventhandlerName);
            eventWaitHandle4.Reset();
            Task.Run(() => { t.CrawlerUris(addresses, 10, syscobject); });
            Task.Run(() => { a.AnalysisResults(t.resultlists, 5, syscobject, anasissyncObject); });
            Task.Run(() =>
            {
                d.SendDispatchDatas<AnswerInfo>(ref a.datas, 500, ref anasissyncObject, infos,
                    null);
            });
            eventWaitHandle1.WaitOne();
            eventWaitHandle2.WaitOne();
            eventWaitHandle3.WaitOne();
            eventWaitHandle4.WaitOne();
        }

        public void LoadUserData(IList<Uri> addresses, bool IsTruncateTempDB = true)
        {

  

            WebHttpCrawlerClient client = new WebHttpCrawlerClient();

            client.SetRequestDetail = (x) =>
            {
                x._HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                x._HttpWebRequest.Referer = "https://api.stackexchange.com/docs/users";
            };

            WebThreadDesigation t = new WebThreadDesigation(client);
            t.EventWaitHandleName = "WebThreadDesigation";

            StackoverflowPageAnalysis ana = new StackoverflowPageAnalysis();
            AnalysisThreadDesigation<UserInfo> a = new AnalysisThreadDesigation<UserInfo>(ana);
            a.EventWaitHandleName = "StackoverflowPageAnalysis";
            DBThreadDesigation d = new DBThreadDesigation();
            d.EventWaitHandleName = "DBThreadDesigation";
            CrawlerSyncObject syscobject = new CrawlerSyncObject();
            AnalysisSyncObject anasissyncObject = new AnalysisSyncObject();

            SendImformation infos = new SendImformation();
            infos.ConnectString = "data source=CIA-SH-svr-sis;initial catalog=stackoverflowNew;integrated security=True;";
            //infos.EventhandlerName = "dbo.SuggestionStagging";
            //infos.DestinationtableName = "dbo.SuggestionStagging";

            infos.EventhandlerName = "UserTemp1_Tinct";
            infos.DestinationtableName = "UserTemp1_Tinct";
            infos.IsWantToMerged = false;
            infos.MergeSQlSPName = "";

            EventWaitHandle eventWaitHandle1 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, t.EventWaitHandleName);
            eventWaitHandle1.Reset();
            EventWaitHandle eventWaitHandle2 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, a.EventWaitHandleName);
            eventWaitHandle2.Reset();
            EventWaitHandle eventWaitHandle3 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, d.EventWaitHandleName);
            eventWaitHandle3.Reset();
            EventWaitHandle eventWaitHandle4 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, infos.EventhandlerName);
            eventWaitHandle4.Reset();
            Task.Run(() => { t.CrawlerUris(addresses, 10, syscobject); });
            Task.Run(() => { a.AnalysisResults(t.resultlists, 5, syscobject, anasissyncObject); });
            Task.Run(() =>
            {
                d.SendDispatchDatas<UserInfo>(ref a.datas, 500, ref anasissyncObject, infos,
                    null);
            });
            eventWaitHandle1.WaitOne();
            eventWaitHandle2.WaitOne();
            eventWaitHandle3.WaitOne();
            eventWaitHandle4.WaitOne();




        }
    }
}
