using DomainEvents.Contracts;
using DomainEvents.Events;

namespace DomainEvents.Services
{
    public class ApplicationService : IHandle<ChangeName>
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
    }
}