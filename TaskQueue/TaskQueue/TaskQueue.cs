using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static TaskQueue.Task;

namespace TaskQueue
{

 
   
    class TaskQueue: IDisposable
    {
        bool exitToken = false;
        ConcurrentQueue<Task> taskAsynQueue = new ConcurrentQueue<Task>();
         BlockingCollection<Thread> threads = new BlockingCollection<Thread>();

      public  TaskQueue(int quantity)
        {
            for(int i=0; i<quantity; i++)
            {
                var thread = new Thread(new ThreadStart(EnqueueTask));
                threads.Add(thread);
            } 
        }

        public ConcurrentQueue<Task> TaskAsynQueue { get => taskAsynQueue; set => taskAsynQueue = value; }

        public void AddTask(TaskDelegate taskDelegate, object param)
        {
            TaskAsynQueue.Enqueue(new Task(param,taskDelegate));
        }

        public void Dispose()
        {
            exitToken = true;
        }

        public void Start()
        {
            threads.ToList().ForEach(o => o.Start());
        }

        void EnqueueTask()
        {
            Task task = null;
            while (!exitToken)
            {
                try
                {
                    if(TaskAsynQueue.Count==0)
                    {                      
                        Dispose();
                        break;
                    }

                    if (taskAsynQueue.TryDequeue(out task))
                    {
                        task.Execute();
                        Console.WriteLine("Количество задач в очереди на выполнение {0}", TaskAsynQueue.Count);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }
            }

            if(exitToken)
            {
                Console.WriteLine("Поток {0} завершается", Thread.CurrentThread.Name);
            }
        }

    }
}
