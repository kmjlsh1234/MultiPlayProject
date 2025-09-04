using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server.Game.Job
{
    public struct JobTimerElem : IComparable<JobTimerElem>
    {
        public int execTic;
        public IJob job;   

        public int CompareTo(JobTimerElem other)
        {
            return other.execTic.CompareTo(execTic);
        }
    }

    public class JobTimer
    {
        public PriorityQueue<JobTimerElem> priorityQueue = new PriorityQueue<JobTimerElem>();
        object key = new object();

        //
        public void Push(IJob job, int tickAter = 0)
        {
            JobTimerElem jobTimerElem = new JobTimerElem();
            jobTimerElem.job = job;
            jobTimerElem.execTic = Environment.TickCount + tickAter;

            lock (key)
            {
                priorityQueue.Push(jobTimerElem);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now = Environment.TickCount;

                JobTimerElem jobElem;

                lock (key)
                {
                    if (priorityQueue.Count == 0)
                    {
                        break;
                    }

                    jobElem = priorityQueue.Peek();
                    if(jobElem.execTic > now)
                    {
                        break;
                    }

                    priorityQueue.Pop();
                }

                jobElem.job.Execute();
            }
            
        }
    }
}
