using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinct.PlatformController
{
    [Serializable]
    public class TinctTest1Controller
    {
        public void LoadData1(string taskDatas)
        {

            System.Threading.Thread.Sleep(5000);
            Console.WriteLine("test tinct  5000");
        }

        public void LoadData(string taskDatas)
        {

            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("test tinct  1000");
        }

        public void LoadData2(string taskDatas)
        {
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("test tinct  20000");
        }
    }
}
