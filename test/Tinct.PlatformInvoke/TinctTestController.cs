using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tinct.PlatformController
{
    [Serializable]
    public  class TinctTestController:MarshalByRefObject
    {

        public void LoadData1()
        {
            System.Threading.Thread.Sleep(200000); 
            Console.WriteLine("test tinct  tinct 2000!");
        }

        public void LoadData2(string taskDatas)
        {
            System.Threading.Thread.Sleep(10000);

            Console.WriteLine("test tinct  tinct 70000!");
        }

    }
}
