using DomainEvents.Contracts;

namespace DomainEvents.Events
{
    public class NameChanged : IEvent
    {
        public string NewName;
        public string OldName;

        public NameChanged(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }

    public class DoSomethingThatTakesLong : ICommand
    {
    }

    public class ChangeName: ICommand
    {
        public string NewName;
        public string OldName;

        public ChangeName(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}