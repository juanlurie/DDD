using System;
using System.Threading;
using DomainEvents.Contracts;
using DomainEvents.Events;

namespace DomainEvents.Services
{
    public class ApplicationService :
          IHandle<ChangeName>
        , IHandle<DoSomethingThatTakesLong>
    {
        private readonly IInMemoryBus inMemoryBus;

        public ApplicationService(IInMemoryBus inMemoryBus)
        {
            this.inMemoryBus = inMemoryBus;
        }

        public void Handle(ChangeName command)
        {
            var nameChanged = new NameChanged(command.OldName, command.NewName);

            inMemoryBus.Publish(nameChanged);
        }

        public void Handle(DoSomethingThatTakesLong @event)
        {
            Thread.Sleep(10000);
            Console.WriteLine("Done Async");
        }
    }
}