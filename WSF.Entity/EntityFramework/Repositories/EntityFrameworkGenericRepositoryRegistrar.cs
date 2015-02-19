﻿using System;
using WSF.Dependency;
using WSF.Domain.Entities;
using WSF.Domain.Repositories;
using WSF.EntityFramework.Extensions;

namespace WSF.EntityFramework.Repositories
{
    internal static class EntityFrameworkGenericRepositoryRegistrar
    {
        public static void RegisterForDbContext(Type dbContextType, IIocManager iocManager)
        {
            foreach (var entityType in dbContextType.GetEntityTypes())
            {
                foreach (var interfaceType in entityType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                    {
                        var primaryKeyType = interfaceType.GenericTypeArguments[0];
                        if (primaryKeyType == typeof(int))
                        {
                            var genericRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                            if (!iocManager.IsRegistered(genericRepositoryType))
                            {
                                iocManager.Register(
                                    genericRepositoryType,
                                    typeof(EfRepositoryBase<,>).MakeGenericType(dbContextType, entityType),
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                typeof(EfRepositoryBase<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType),
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }
    }
}