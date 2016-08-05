using System.Data.Entity;

namespace Iris.EntityFramework
{
    public interface IContextFactory
    {
        DbContext GetContext(ContextConfiguration configuration);
    }
}