using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.TinctTaskMangement.Util;
using System.IO;
using System.Configuration;
using System.Net;
using Tinct.Net.Message.Node;

namespace Tinct.Net.Communication.Slave
{
    public class DeployMessageHandler : IMessageHandler
    {
        private string masterName = ConfigurationManager.AppSettings["Master"].ToString();
        public bool HanderMessage(string message)
        {
            try
            {
                var deploycontent = DeployContent.GetObjectBySerializeString(message);
                if (deploycontent.FileName == null) 
                {
                    return true;
                }

                foreach (var appdomain in RunCarrier.Current.AppDomainDicts) 
                {
                    try
                    {
                        AppDomain.Unload(appdomain.Value);


                    }
                    catch 
                    {

                    }
                }
                RunCarrier.Current.AppDomainDicts.Clear();

                try
                {
                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "//" + deploycontent.FileName, deploycontent.Content);
                    deploycontent.Status = "Completed";
                }
                catch 
                {
                    deploycontent.Status = "Failed";
                    TinctSlaveNode.Current.SlaveNodeInfo.Status = NodeStatus.DeployFailed;
                }
                deploycontent.TargetNodeName =Dns.GetHostName();
               
                TinctSlaveNode.Current.SendMessageToMaster(TinctSlaveNode.Current.SlaveNodeInfo.ToJsonSerializeString(), masterName);
                TinctSlaveNode.Current.SendMessageToMaster(deploycontent.ToJsonSerializeString(), masterName);
            }
            catch
            {
                return true;
            }

            return false;
        }
    }
}
