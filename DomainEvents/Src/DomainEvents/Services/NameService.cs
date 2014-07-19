using System;
using DomainEvents.Contracts;
using DomainEvents.Events;

namespace DomainEvents.Services
{
    public class EmailService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("Emailing");
        }
    }

    public class LogService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("Logging");
        }
    }

    public class ProjectionService : IHandle<NameChanged>
    {
        public void Handle(NameChanged @event)
        {
            Console.WriteLine("Projecting");
        }
    }
}