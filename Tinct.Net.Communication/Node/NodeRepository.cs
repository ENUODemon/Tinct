using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;
using Tinct.Net.Message.Node;
using System.Threading;
using System.Collections.Concurrent;

namespace Tinct.Net.Communication.Node
{
    public class NodeRepository : INodeRepository
    {

        private ConcurrentDictionary<string, NodeInfo> dictNodeInfos = new ConcurrentDictionary<string, NodeInfo>();

        private ManualResetEvent nodeSignal = new ManualResetEvent(false);


        public List<NodeInfo> NodeInfoList
        {
            get
            {
                return dictNodeInfos.Values.ToList();
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

            dictNodeInfos.TryAdd(nodeInfo.NodeName, nodeInfo);
        }

        public bool RemoveNode(NodeInfo nodeInfo)
        {
            NodeInfo outnodeInfo = null;
            dictNodeInfos.TryRemove(nodeInfo.NodeName, out outnodeInfo);
            if (dictNodeInfos.Keys.Count == 0)
            {
                nodeSignal.Reset();
            }

            if (outnodeInfo == null)
            {
                return false;
            }
            return true;
        }


        public bool UpdateNodeInfo(NodeInfo nodeInfo)
        {
            var result = dictNodeInfos.TryAdd(nodeInfo.NodeName, nodeInfo);
            if (result)
            {
                nodeSignal.Set();
            }
            else
            {
                dictNodeInfos[nodeInfo.NodeName].LastUpdateTime = nodeInfo.LastUpdateTime;
                dictNodeInfos[nodeInfo.NodeName].Status = nodeInfo.Status;
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

            foreach (var m in dictNodeInfos)
            {
                if (m.Value.Status == NodeStatus.Running)
                {
                    avalibleNodes.Add(m.Value);
                }

            }
            return avalibleNodes;
        }

        public NodeInfo GetNodeByName(string nodeName)
        {
            NodeInfo result = null;
            dictNodeInfos.TryGetValue(nodeName, out result);
            return result;
        }

    }
}
