using System;
using DomainEvents.Contracts;
using DomainEvents.Events;

namespace DomainEvents.Services
{
    public class EmailService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("EVENT HANDLED {0}: Emailing",@event.GetType().Name);
        }
    }

    public class LogService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("EVENT HANDLED {0}: Logging", @event.GetType().Name);
        }
    }

    public class ProjectionService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("EVENT HANDLED {0}: Projecting", @event.GetType().Name);
        }
    }
}