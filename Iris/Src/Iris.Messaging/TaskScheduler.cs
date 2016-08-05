using Iris.Ioc;

namespace Iris.Messaging
{
    public abstract class TaskScheduler<TTask> : ScheduledWorkerService where TTask : ITask
    {
        protected override void DoWork()
        {
            var task = ServiceLocator.Current.GetInstance<TTask>();
            task.DoWork();
        }
    }
}