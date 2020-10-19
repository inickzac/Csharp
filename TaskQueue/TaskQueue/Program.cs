using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskQueue
{
    class Program
    {
      public static Random random = new Random();
        static void Main(string[] args)
        {           
            TaskQueue taskQueue = new TaskQueue(10);
            int name = 0;
            for (int i = 0; i < 30; i++)
            {
                taskQueue.AddTask(DoIt, (object)name);
                name++;
            }
            taskQueue.Start();
            Console.ReadLine();
        }
        
      static void DoIt (object o)
        {
            Console.WriteLine("Задача {0} начала выполнение ", (int)o);
            Thread.Sleep(random.Next(500));
        }

       
    }  
}
