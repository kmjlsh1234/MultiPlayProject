using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class JobQueue
    {
        Queue<Action> jobQueue = new Queue<Action>();
        object key = new object();
        bool isFlush = false;   //현재 ;Flush()가 동작 중인지 여부를 나타냄

        public void Push(Action action)
        {
            bool flush = false;
            lock (key)
            {
                jobQueue.Enqueue(action);

                if(isFlush == false) //아무도 Flush사용 안하고 있음
                {
                    isFlush = true;
                    flush = true;
                }
            }

            if (flush)
            {
                Flush();
            }
        }

        private void Flush()
        {
            while (true)
            {
                Action action = Pop();
                if (action == null)
                {
                    return;
                }

                action.Invoke();
            }
        }

        private Action Pop()
        {
            lock (key)
            {
                if(jobQueue.Count == 0)
                {
                    isFlush= false;
                    return null;
                }
                return jobQueue.Dequeue();
            }
        }
    }
}
