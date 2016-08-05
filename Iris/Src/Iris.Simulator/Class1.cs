using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Iris.EntityFramework;
using Iris.Ioc;
using Iris.Messaging;
using Iris.Messaging.EndPoints;
using Iris.ObjectBuilder.Autofac;
using Iris.Persistence;
using Iris.Serialization.Json;
using MySql.Data.Entity;

namespace Iris.Simulator
{
    public class TestTable
    {
        public int Id { get; set; }
    }

    public class MediaContextInitializer : ContextInitializer<TestContext>
    {

    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TestContext : FrameworkContext
    {
        public IDbSet<TestTable> TestTable { get; set; }

        public TestContext()
        {
        }

        public TestContext(string databaseName) : base(databaseName)
        {
        }
    }

    public class Endpoint : LocalEndpoint<AutofacAdapter>
    {
        protected override void ConfigureEndpoint(IConfigureEndpoint configuration)
        {
            configuration
                .UseJsonSerialization()
                .RegisterDependencies(new DependencyRegistrar())
                .ConfigureEntityFramework<TestContext>("TestDb")
                .DefineCommandAs(IsCommand)
                .DefineEventAs(IsEvent)
                .DefineMessageAs(IsMessage);
        }

        private static bool IsCommand(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            return typeof(IDomainCommand).IsAssignableFrom(type);
        }

        private static bool IsEvent(Type type)
        {
            return typeof(IDomainEvent).IsAssignableFrom(type);
        }

        private static bool IsMessage(Type type)
        {
            return typeof(IMessage).IsAssignableFrom(type);
        }
    }

    public class DependencyRegistrar : IRegisterDependencies
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TestWorker>();
        }
    }

    public interface IDomainCommand
    {
    }

    public class TestCommand : IDomainCommand
    {

    }

    public class TestEvent : IDomainEvent
    {

    }

    public class Test : IHandleMessage<TestCommand>
                      , IHandleMessage<TestEvent>
    {
        private readonly IInMemoryBus localBus;
        private EntityFramework.IRepository<TestTable> repository;
        private IQueryable<TestTable> query;

        public Test(IInMemoryBus localBus, IRepositoryFactory repositoryFactory, IDatabaseQuery databaseQuery)
        {
            this.localBus = localBus;
            query = databaseQuery.GetQueryable<TestTable>();
            repository = repositoryFactory.GetRepository<TestTable>();
        }

        public void Handle(TestEvent m)
        {
            repository.Add(new TestTable());
        }

        public void Handle(TestCommand m)
        {
            var a = query.Count();
            var b = query.FirstOrDefault();
            var c = query.SingleOrDefault(x => x.Id == 1);
            var d = query.Any();
            var e = query.Where(x => x.Id == 1);

            localBus.Raise(new TestEvent());
        }
    }

    public class TestWorker : ScheduledWorkerService
    {
        private readonly IInMemoryBus localBus;

        public TestWorker(IInMemoryBus localBus)
        {
            SetSchedule(TimeSpan.FromSeconds(10));
            this.localBus = localBus;
        }

        protected override void DoWork()
        {
            localBus.Execute(new TestCommand());
        }
    }
}
