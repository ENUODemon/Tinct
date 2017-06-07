using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tinct.Common.Extension;
using Tinct.Common.Log;
using Tinct.Net.Communication.Cfg;
using Tinct.Net.Communication.Slave;
using Tinct.Net.Message.Message;
using Tinct.TinctTaskMangement.Interface;
using Tinct.TinctTaskMangement.Util;

namespace Tinct.TinctTaskMangement.Enigine
{
    [Serializable]
    public class AssemblyTaskExcute : IExcuteTask
    {

        public AssemblyTaskExcute()
        {

        }


        public void ExuteTask(string datas,string loggerName,string loggerFileName)
        {
            var logger = TinctLoggerManger.GetLogger(loggerName, loggerFileName);
            var task = TinctTask.GetObjectBySerializeString(datas);
            if (logger != null)
            {
                logger.LogMessage(task.ToJsonSerializeString());
            }
            AssemblyExcuteEnvironment.Current.AppDomainDicts.TryGetValue(task.ClassName + "\\" + task.MethodName, out AppDomain runTimeActionDomain);
            if (runTimeActionDomain == null)
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.CachePath = setup.ApplicationBase;
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = setup.ApplicationBase;
                runTimeActionDomain = AppDomain.CreateDomain(task.ClassName + "\\" + task.MethodName, null, setup);
                AssemblyExcuteEnvironment.Current.AppDomainDicts.TryAdd(runTimeActionDomain.FriendlyName, runTimeActionDomain);
            }
            try
            {
                dynamic controler = runTimeActionDomain.CreateInstanceFrom(task.DllName + ".dll", task.NamespaceName + "." + task.ClassName).Unwrap();

                MethodInfo method = controler.GetType().GetMethod(task.MethodName);
                int parametercount = method.GetParameters().Count();
                if (parametercount == 0)
                {
                    method.Invoke(controler, null);
                }
                else
                {
                    method.Invoke(controler, new object[] { task.Datas });
                }

                task.Status = TinctTaskStatus.Completed;
            }
            catch (AppDomainUnloadedException e)
            {
                Console.WriteLine(e);
                //log 
                task.Status = TinctTaskStatus.Exception;

                task.Exption = e;
                task.HasException = true;
                task.ExceptionString = e.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //log 
                task.Status = TinctTaskStatus.Exception;
                task.Exption = e;
                task.HasException = true;
                task.ExceptionString = e.ToString();
            }

            AssemblyExcuteEnvironment.Current.UnloadDomain(runTimeActionDomain.FriendlyName);
            task.EndTime = DateTimeExtension.GetTimeStamp();
            if (logger != null)
            {
                logger.LogMessage(task.ToJsonSerializeString());
            }
            TinctMessage message = new TinctMessage();
            message.MessageHeader = new MessageHeader() { CommandType = CommandType.Return };
            MessageBody body = new MessageBody();
            body.Datas = task.ToJsonSerializeString();
            message.MessageBody = body;

            TinctSlaveNode.Current.SendMessage(TinctNodeCongratulations.MasterName, message);
        }
    }
}
