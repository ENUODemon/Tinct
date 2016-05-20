using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Message.Node;

namespace Tinct.Net.Communication.Interface
{
    public interface INodeRepository
    {


        /// <summary>
        /// Add nodeInfo to NodeRepository
        /// </summary>
        /// <param name="nodeInfo"></param>
        void AddNode(NodeInfo nodeInfo);
        /// <summary>
        /// remove nodeInfo from NodeRepository
        /// </summary>
        /// <param name="nodeInfo">the target nodeinfo</param>
        /// <returns></returns>
        bool RemoveNode(NodeInfo nodeInfo);
        /// <summary>
        /// Update NodeInfo by node message 
        /// </summary>
        /// <param name="NodeInfo"></param>
        /// <returns>true for successfully,flase for failed</returns>
        bool UpdateNodeInfo(NodeInfo nodeInfo);
        /// <summary>
        /// Update NodeInfo by string message 
        /// </summary>
        /// <param name="NodeInfo"></param>
        /// <returns>true for successfully,flase for failed</returns>
        bool UpdateNodeInfo(string message);
        /// <summary>
        /// Get  Avaliable  NodeName
        /// </summary>
        /// <returns>Node List</returns>
        List<NodeInfo> GetAvaliableNodes();
    }
}
