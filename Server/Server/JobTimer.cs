using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public struct JobTimerElem : IComparable<JobTimerElem>
    {
        public int execTic;
        public Action action;   

        public int CompareTo(JobTimerElem other)
        {
            return other.execTic.CompareTo(execTic);
        }
    }

    public class JobTimer
    {
        public PriorityQueue<JobTimerElem> priorityQueue = new PriorityQueue<JobTimerElem>();
        object key = new object();

        public static JobTimer Instance { get; } = new JobTimer();

        //
        public void Push(Action action, int tickAter = 0)
        {
            JobTimerElem jobTimerElem = new JobTimerElem();
            jobTimerElem.action = action;
            jobTimerElem.execTic = System.Environment.TickCount + tickAter;

            lock (key)
            {
                priorityQueue.Push(jobTimerElem);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now = System.Environment.TickCount;

                JobTimerElem job;

                lock (key)
                {
                    if (priorityQueue.Count == 0)
                    {
                        break;
                    }

                    job = priorityQueue.Peek();
                    if(job.execTic > now)
                    {
                        break;
                    }

                    priorityQueue.Pop();
                }

                job.action.Invoke();
            }
            
        }
    }
}
