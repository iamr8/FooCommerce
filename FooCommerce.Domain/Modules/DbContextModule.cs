using Autofac;

using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace FooCommerce.Domain.Modules
{
    public class DbContextModule : Module
    {
        private readonly DbContextOptionsBuilder<AppDbContext> _dbContextOptionsBuilder;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextModule(DbContextOptionsBuilder<AppDbContext> dbContextOptionsBuilder, ILoggerFactory loggerFactory)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<SqlConnectionFactory>()
            //    .As<ISqlConnectionFactory>()
            //    .WithParameter("connectionString", _databaseConnectionString)
            //    .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    _dbContextOptionsBuilder
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                    return new AppDbContext(_dbContextOptionsBuilder.Options, _loggerFactory);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DbContextPool<AppDbContext>>()
                .As<IDbContextPool<AppDbContext>>()
                .InstancePerLifetimeScope();

            builder.Register(x => new PooledDbContextFactory<AppDbContext>(x.Resolve<IDbContextPool<AppDbContext>>()))
                .As<IDbContextFactory<AppDbContext>>()
                .InstancePerLifetimeScope();

            //var infrastructureAssembly = typeof(AppDbContext).Assembly;

            //builder.RegisterAssemblyTypes(infrastructureAssembly)
            //    .Where(type => type.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}