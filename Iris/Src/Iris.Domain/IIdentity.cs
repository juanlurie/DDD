namespace Iris.Domain
{
    public interface IIdentity
    {
        dynamic GetId();
        bool IsEmpty();
        string GetTag();
    }
}