using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Job
{
    public class JobSerializer
    {
        JobTimer timer = new JobTimer();
        Queue<IJob> jobQueue = new Queue<IJob>();
        object key = new object();

        public void PushAfter(int tickAfter, IJob job)
        {
            timer.Push(job, tickAfter);
        }
        public void PushAfter(int tickAfter, Action action) { PushAfter(tickAfter, new Job(action)); }
        public void PushAfter<T1>(int tickAfter, Action<T1> action, T1 t1) { PushAfter(tickAfter, new Job<T1>(action, t1)); }

        public void PushAfter<T1, T2>(int tickAfter, Action<T1, T2> action, T1 t1, T2 t2) { PushAfter(tickAfter, new Job<T1, T2>(action, t1, t2)); }

        public void PushAfter<T1, T2, T3>(int tickAfter, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { PushAfter(tickAfter, new Job<T1, T2, T3>(action, t1, t2, t3)); }

        public void Push(Action action)
        {
            Push(new Job(action));
        }

        public void Push<T1>(Action<T1> action,T1 t1) { Push(new Job<T1>(action, t1)); }

        public void Push<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2) { Push(new Job<T1, T2>(action, t1, t2)); }

        public void Push<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { Push(new Job<T1,T2,T3>(action, t1,t2,t3)); }

        

        public void Push(IJob job)
        {
            lock (key)
            {
                jobQueue.Enqueue(job);
            }
        }

        public void Flush()
        {
            timer.Flush();

            while (true)
            {
                IJob job = Pop();
                if (job == null)
                {
                    return;
                }

                job.Execute();
            }
        }

        private IJob Pop()
        {
            lock (key)
            {
                if (jobQueue.Count == 0)
                {
                    return null;
                }
                return jobQueue.Dequeue();
            }
        }
    }
}
