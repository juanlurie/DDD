namespace Iris.Persistence
{
    public interface ILookupTable
    {
        int Id { get; }
        string Description { get; set; }
    }
}