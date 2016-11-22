using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Node;
using System.Threading;

namespace Tinct.Net.Communication.Node
{
    public class NodeRepository : INodeRepository
    {
        private List<NodeInfo> nodeInfoList = new List<NodeInfo>();

        private List<NodeInfo> oriNodeInfolist = new List<NodeInfo>();

        private object syncNodeInfoObject = new object();
        private ManualResetEvent nodeSignal = new ManualResetEvent(false);


        public List<NodeInfo> NodeInfoList
        {
            get
            {
                return nodeInfoList;
            }
            private set { }
        }


        static NodeRepository()
        {
            Current = new NodeRepository();
        }


        public static NodeRepository Current { get; private set; }

        public NodeRepository()
        {
            Current = this;
        }

        public void AddNode(NodeInfo nodeInfo)
        {
            lock (syncNodeInfoObject)
            {
                nodeInfoList.Add(nodeInfo);
                nodeSignal.Set();
            }
        }

        public bool RemoveNode(NodeInfo nodeInfo)
        {
            lock (syncNodeInfoObject)
            {
                var finditem= nodeInfoList.Find(item => item.NodeName == nodeInfo.NodeName);
                if (finditem == null)
                {
                    return true;
                }
                var reNode = finditem;
                var result = nodeInfoList.Remove(reNode);
                if (nodeInfoList.Count == 0)
                {
                    nodeSignal.Reset();
                }
                return result;
            }
        }


        public bool UpdateNodeInfo(NodeInfo nodeInfo)
        {
            lock (syncNodeInfoObject)
            {
                var updateNode = nodeInfoList.Find(item => item.NodeName == nodeInfo.NodeName);
                if (updateNode == null)
                {
                    nodeInfoList.Add(nodeInfo);
                    nodeSignal.Set();
                  
                }
                else
                {
                    updateNode.LastUpdateTime = nodeInfo.LastUpdateTime;
                    updateNode.Status = nodeInfo.Status;
                }


            }
            return true;

        }

        public bool UpdateNodeInfo(string message)
        {
            try
            {
                NodeInfo NodeInfo = NodeInfo.GetObjectBySerializeString(message);

                return UpdateNodeInfo(NodeInfo);
            }
            catch
            {
                //log
                return false;
            }
        }

        public List<NodeInfo> GetAvaliableNodes()
        {
            List<NodeInfo> avalibleNodes = new List<NodeInfo>();


            nodeSignal.WaitOne();
            lock (syncNodeInfoObject)
            {
                foreach (var m in nodeInfoList)
                {
                    if (m.Status == NodeStatus.Running)
                    {
                        avalibleNodes.Add(m);
                    }

                }
            }
            return avalibleNodes;
        }

        public NodeInfo GetNodeByName(string nodeName)
        {
            return nodeInfoList.FirstOrDefault(t => t.NodeName == nodeName);
        }

        public void SyncNodeInfo(List<IUnConnectNodeHandler> UnConnectHandlers) 
        {
            lock (syncNodeInfoObject)
            {
                List<NodeInfo> reNodes = new List<NodeInfo>();
                for (int i = 0; i < nodeInfoList.Count(); i++)
                {
                    if (DateTime.Now-NodeInfoList[i].LastUpdateTime>TimeSpan.FromSeconds(120))
                    {
                        var item = nodeInfoList[i];

                        foreach (var handler in UnConnectHandlers)
                        {
                            new Task(() => { handler.HandleUnConnectNode(item); }).Start();  
                        }

                        reNodes.Add(item);        
                    }

                }
                foreach (var reNode in nodeInfoList)
                {
                    reNodes.Remove(reNode);
                }


            }
        }
    }
}
