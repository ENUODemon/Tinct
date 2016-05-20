using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinct.Net.Communication.Master;
using Tinct.Net.Communication.Node;
using Tinct.Net.Message.Node;
using Tinct.TinctTaskMangement.TinctWork;

namespace Tinct.TinctTaskMangement.Util
{
    public class DeployTask
    {

        public DeployContent Content { get; set; }

        public  void Start() 
        {
            //sleep 12 seconds for connect slave machines
            System.Threading.Thread.Sleep(15000);
            foreach (var node in NodeRepository.Current.NodeInfoList) 
            {
                node.Status = NodeStatus.Deploy;
            }
            try
            {
                Content.Content = File.ReadAllBytes(Content.FilePath +"\\"+ Content.FileName);
            }
            catch (Exception e)
            {
                Content = null;

            }



            new Task(() =>
            {

                foreach (var node in NodeRepository.Current.NodeInfoList)
                {
                    TinctMasterNode.Current.SendMessageToSlave(Content.ToJsonSerializeString(), node.NodeName);
                }

               
            }
            ).Start();

        }
        /// <summary>
        /// wait is not necessary ,further to improve
        /// </summary>
        public void Wait() 
        {

        }


     

      
    }
}
