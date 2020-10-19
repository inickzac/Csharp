using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskQueue
{
    class Task
    {
       public delegate void TaskDelegate(object obj);
       TaskDelegate taskDelegate;
       object param;

        internal TaskDelegate TaskDelegate1 { get => taskDelegate; set => taskDelegate = value; }
        public object Param { get => param; set => param = value; }

        public Task(object param, TaskDelegate taskDelegate)
        {
            this.TaskDelegate1 = taskDelegate;
            this.Param = param;
        }

        public void Execute()
        {
            taskDelegate?.Invoke(param);
        }
    }
}
