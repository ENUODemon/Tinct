using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Slave;


namespace Tinct.Net.SlavePointConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TinctSlaveNode p = new TinctSlaveNode();
            p.StartSlave();
            Console.Read();
        }
    }
}
