using DomainEvents.Contracts;
using DomainEvents.Events;

namespace DomainEvents.Services
{
    public class ApplicationService 
    {
        private readonly IInMemoryBus inMemoryBus;

        public ApplicationService(IInMemoryBus inMemoryBus)
        {
            this.inMemoryBus = inMemoryBus;
        }

        public void UpdateName()
        {
            var nameChanged = new NameChanged("", "");

            inMemoryBus.Publish(nameChanged);
        }
    }
}