using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace Utils
{
#if !UTILS_INTERNAL
    public class CustomThreadPool : IDisposable
#else
    internal class CustomThreadPool : IDisposable
#endif
    {
        private const int MAX = 5;
        private const int MIN = 2;
        private const int MIN_WAIT = 10; // milliseconds
        private const int MAX_WAIT = 15000; // milliseconds
        private const int CLEANUP_INTERVAL = 60000; // millisecond
        private const int SCHEDULING_INTERVAL = 10; // millisecond
        //private instance members
        private Queue<TaskHandle> ReadyQueue = null;
        private List<TaskItem> Pool = null;
        private Thread taskScheduler = null;
        private bool endThread;
        // Locks
        private object syncLock = new object(); // main lock - to be used for locking ReadyQueue 
        private object criticalLock = new object(); // to be used for locking Pool
        private bool threadEnded;

        public CustomThreadPool() 
        {
            InitializeThreadPool();
        }

        private void InitializeThreadPool()
        {
            ReadyQueue = new Queue<TaskHandle>();
            Pool = new List<TaskItem>();

            InitPoolWithMinCapacity(); // initialize Pool with Minimum capacity - that much thread must be kept ready

            DateTime LastCleanup = DateTime.Now; // monitor this time for next cleaning activity

            taskScheduler = new Thread(() =>
            {
                bool end = false;

                do
                {
                    lock (syncLock) // obtaining lock for ReadyQueue
                    {
                        while (ReadyQueue.Count > 0 && ReadyQueue.Peek().task == null) ReadyQueue.Dequeue(); // remove cancelled item/s - cancelled item will have it's task set to null

                        int itemCount = ReadyQueue.Count;

                        for (int i = 0; i < itemCount; i++)
                        {
                            TaskHandle readyItem = ReadyQueue.Peek(); // the Top item of queue
                            bool Added = false;
                            lock (criticalLock) // lock for the Pool
                            {
                                foreach (TaskItem ti in Pool) // while reading the pool another thread should not add/remove to that pool
                                {
                                    lock (ti) // locking item
                                    {
                                        if (ti.taskState == TaskState.completed)
                                        { // if in the Pool task state is completed then a different task can be handed over to that thread
                                            ti.taskHandle = readyItem;
                                            ti.taskState = TaskState.notstarted;
                                            Added = true;
                                            ReadyQueue.Dequeue();
                                            break;
                                        }
                                    }
                                }

                                if (!Added && Pool.Count < MAX)
                                { // if all threads in pool are busy and the count is still less than the Max limit set then create a new thread and add that to pool
                                    TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                                    ti.taskHandle = readyItem;
                                    // add a new TaskItem in the pool
                                    AddTaskToPool(ti);
                                    Added = true;
                                    ReadyQueue.Dequeue();
                                }
                            }
                            if (!Added) break; // It's already crowded so try after sometime
                        }

                        end = endThread;
                    }

                    if ((DateTime.Now - LastCleanup) > TimeSpan.FromMilliseconds(CLEANUP_INTERVAL)) // It's long time - so try to cleanup Pool once.
                    {
                        CleanupPool();
                        LastCleanup = DateTime.Now;
                    }
                    else
                    {
                        Thread.Yield(); // either of these two can work - the combination is also fine for our demo. 
                        Thread.Sleep(SCHEDULING_INTERVAL); // Dont run madly in a loop - wait for sometime for things to change.
                        // the wait should be minimal - close to zero
                    }

                } while (!end);

                lock (syncLock) // obtaining lock for ReadyQueue
                {
                    threadEnded = true;
                }
            });

            taskScheduler.Priority = ThreadPriority.AboveNormal;

            taskScheduler.Start();
        }

        private void InitPoolWithMinCapacity()
        {
            for (int i = 0; i <= MIN; i++)
            {
                TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                ti.taskHandle = new TaskHandle() { task = () => { } };
                ti.taskHandle.callback = (taskStatus) => { };
                ti.taskHandle.Token = new ClientHandle() { ID = Guid.NewGuid() };
                AddTaskToPool(ti);
            }
        }

        private void AddTaskToPool(TaskItem taskItem)
        {
            taskItem.handler = new Thread(() =>
            {
                bool end = false;

                do
                {
                    bool Enter = false;

                    lock (taskItem) // the taskState of taskItem is exposed to scheduler thread also so access that always with this lock
                    {               // Only two thread can contend for this [cancel and executing thread as taskItem itself is is mapped to a dedicated thread]
                        // if aborted then allow it to exit the loop so that it can complete and free-up thread resource.
                        // this state means it has been removed from Pool already.
                        if (taskItem.taskState == TaskState.aborted) break;

                        if (taskItem.taskState == TaskState.notstarted)
                        {
                            taskItem.taskState = TaskState.processing;
                            taskItem.startTime = DateTime.Now;
                            Enter = true;
                        }
                    }
                    if (Enter)
                    {
                        TaskStatus taskStatus = new TaskStatus();
                        try
                        {
                            taskItem.taskHandle.task.Invoke(); // execute the UserTask
                            taskStatus.Success = true;
                        }
                        catch (Exception ex)
                        {
                            taskStatus.Success = false;
                            taskStatus.InnerException = ex;
                        }
                        lock (taskItem) // Only two thread can contend for this [cancel and executing thread as taskItem itself is is mapped to a dedicated thread]
                        {
                            if (taskItem.taskHandle.callback != null && taskItem.taskState != TaskState.aborted)
                            {
                                try
                                {
                                    taskItem.taskState = TaskState.completed;
                                    taskItem.startTime = DateTime.MaxValue;

                                    taskItem.taskHandle.callback(taskStatus); // notify callback with task-status
                                }
                                catch
                                {
                                    // supress exception
                                }
                            }
                        }
                    }

                    lock (syncLock) // obtaining lock for ReadyQueue
                    {
                        end = endThread;
                    }

                    // give other thread a chance to execute as it's current execution completed already
                    
                    Thread.Yield(); Thread.Sleep(MIN_WAIT); //TODO: need to see if Sleep is required here

                } while (!end);
            });

            taskItem.handler.Start();

            lock (criticalLock) // always use this lock for Pool
            {
                Pool.Add(taskItem);
            }
        }

        private void CleanupPool()
        {
            List<TaskItem> filteredTask = null;
            lock (criticalLock) // aquiring lock for Pool
            {
                filteredTask = Pool.Where(ti => ti.taskHandle.Token.IsSimpleTask == true && (DateTime.Now - ti.startTime) > TimeSpan.FromMilliseconds(MAX_WAIT)).ToList();
            }
            foreach (var taskItem in filteredTask)
            {
                CancelUserTask(taskItem.taskHandle.Token);
            }
            lock (criticalLock)
            {
                filteredTask = Pool.Where(ti => ti.taskState == TaskState.aborted).ToList();
                foreach (var taskItem in filteredTask) // clean all aborted thread
                {
                    try
                    {
                        taskItem.handler.Abort(); // does not work
                        taskItem.handler.Priority = ThreadPriority.Lowest;
                        taskItem.handler.IsBackground = true;
                    }
                    catch { }
                    Pool.Remove(taskItem);
                }
                int total = Pool.Count;
                if (total >= MIN) // clean waiting threads over minimum limit
                {
                    filteredTask = Pool.Where(ti => ti.taskState == TaskState.completed).ToList();
                    foreach (var taskItem in filteredTask)
                    {
                        taskItem.handler.Priority = ThreadPriority.AboveNormal;
                        taskItem.taskState = TaskState.aborted;
                        Pool.Remove(taskItem);
                        total--;
                        if (total == MIN) break;
                    }
                }
                while (Pool.Count < MIN)
                {
                    TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                    ti.taskHandle = new TaskHandle() { task = () => { } };
                    ti.taskHandle.Token = new ClientHandle() { ID = Guid.NewGuid() };
                    ti.taskHandle.callback = (taskStatus) => { };
                    AddTaskToPool(ti);
                }
            }
        }

        #region public interface
        public ClientHandle QueueUserTask(UserTask task, Action<TaskStatus> callback)
        {
            TaskHandle th = new TaskHandle()
            {
                task = task,
                Token = new ClientHandle()
                {
                    ID = Guid.NewGuid()
                },
                callback = callback
            };

            lock (syncLock) // main-lock - will be used for accessing ReadyQueue always
            {
                ReadyQueue.Enqueue(th);
            }

            return th.Token;
        }

        public void CancelUserTask(ClientHandle clientToken)
        {
            lock (syncLock)
            {
                var thandle = this.ReadyQueue.FirstOrDefault((th) => th.Token.ID == clientToken.ID);
                if (thandle != null) // in case task is still in queue only
                {
                    thandle.task = null;
                    thandle.callback = null;
                    thandle.Token = null;
                }
                else // in case theread is running the task - try aborting the thread to cancel the operation (rude behavior)
                {
                    int itemCount = this.ReadyQueue.Count;
                    TaskItem taskItem = null;
                    lock (this.criticalLock)
                    {
                        taskItem = this.Pool.FirstOrDefault(task => task.taskHandle.Token.ID == clientToken.ID);
                    }
                    if (taskItem != null)
                    {
                        lock (taskItem) // only item need the locking
                        {
                            if (taskItem.taskState != TaskState.completed) // double check - in case by the time this lock obtained callback already happened
                            {
                                taskItem.taskState = TaskState.aborted;
                                taskItem.taskHandle.callback = null; // stop callback
                            }
                            if (taskItem.taskState == TaskState.aborted) // this does not need criticalLock
                            {
                                try
                                {
                                    taskItem.handler.Abort(); // **** it does not work ****
                                    taskItem.handler.Priority = ThreadPriority.BelowNormal;
                                    taskItem.handler.IsBackground = true;
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            var ended = false;

            lock (syncLock) // obtaining lock for ReadyQueue
            {
                endThread = true;
            }

            while (!ended)
            {
                lock (syncLock) // obtaining lock for ReadyQueue
                {
                    ended = threadEnded;
                }

                Thread.Sleep(100);
            }
        }

        #endregion

        #region nested private types
        enum TaskState // to represent current state of a task
        {
            notstarted,
            processing,
            completed,
            aborted
        }
        class TaskHandle // Item in waiting queue
        {
            public ClientHandle Token; // generate this everytime an usertask is queued and return to the caller as a reference. 
            public UserTask task; // the item to be queued - supplied by the caller
            public Action<TaskStatus> callback; // optional - in case user want's a notification of completion
        }
        class TaskItem // running items in the pool - TaskHandle gets a thread to execute it 
        {
            public TaskHandle taskHandle;
            public Thread handler;
            public TaskState taskState = TaskState.notstarted; // contention will be there between executing thread and cancelling thread
            public DateTime startTime = DateTime.MaxValue;
        }
        #endregion
    }

    public delegate void UserTask();
    public class ClientHandle
    {
        public Guid ID;
        public bool IsSimpleTask = false;
    }
    public class TaskStatus
    {
        public bool Success = true;
        public Exception InnerException = null;
    }
}
