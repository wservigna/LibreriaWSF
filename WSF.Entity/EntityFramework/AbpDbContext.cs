using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using WSF.Configuration.Startup;
using WSF.Domain.Entities;
using WSF.Domain.Entities.Auditing;
using WSF.Events.Bus.Entities;
using WSF.Runtime.Session;

namespace WSF.EntityFramework
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class WSFDbContext : DbContext
    {
        /// <summary>
        /// Used to get current session values.
        /// </summary>
        public IWSFSession WSFSession { get; set; }

        /// <summary>
        /// Used to trigger entity change events.
        /// </summary>
        public IEntityChangedEventHelper EntityChangedEventHelper { get; set; }

        /// <summary>
        /// Constructor.
        /// Uses <see cref="IWSFStartupConfiguration.DefaultNameOrConnectionString"/> as connection string.
        /// </summary>
        protected WSFDbContext()
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(DbCompiledModel model)
            : base(model)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            WSFSession = NullWSFSession.Instance;
            EntityChangedEventHelper = NullEntityChangedEventHelper.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Types<ISoftDelete>().Configure(c => c.HasTableAnnotation(WSFEfConsts.SoftDeleteCustomAnnotationName, true));
        }
        
        public override int SaveChanges()
        {
            ApplyWSFConcepts();
            return base.SaveChanges();
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplyWSFConcepts();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyWSFConcepts()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditProperties(entry);
                        EntityChangedEventHelper.TriggerEntityCreatedEvent(entry.Entity);
                        break;
                    case EntityState.Modified:
                        PreventSettingCreationAuditProperties(entry);
                        SetModificationAuditProperties(entry);
                        EntityChangedEventHelper.TriggerEntityUpdatedEvent(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        PreventSettingCreationAuditProperties(entry);
                        HandleSoftDelete(entry);
                        EntityChangedEventHelper.TriggerEntityDeletedEvent(entry.Entity);
                        break;
                }
            }
        }

        private void SetCreationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IHasCreationTime)
            {
                entry.Cast<IHasCreationTime>().Entity.CreationTime = DateTime.Now; //TODO: UtcNow?
            }

            if (entry.Entity is ICreationAudited)
            {
                entry.Cast<ICreationAudited>().Entity.CreatorUserId = WSFSession.UserId;
            }
        }

        private void PreventSettingCreationAuditProperties(DbEntityEntry entry)
        {
            //TODO@Halil: Implement this when tested well (Issue #49)
            //if (entry.Entity is IHasCreationTime && entry.Cast<IHasCreationTime>().Property(e => e.CreationTime).IsModified)
            //{
            //    throw new DbEntityValidationException(string.Format("Can not change CreationTime on a modified entity {0}", entry.Entity.GetType().FullName));
            //}

            //if (entry.Entity is ICreationAudited && entry.Cast<ICreationAudited>().Property(e => e.CreatorUserId).IsModified)
            //{
            //    throw new DbEntityValidationException(string.Format("Can not change CreatorUserId on a modified entity {0}", entry.Entity.GetType().FullName));
            //}
        }

        private void SetModificationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IModificationAudited)
            {
                var auditedEntry = entry.Cast<IModificationAudited>();

                auditedEntry.Entity.LastModificationTime = DateTime.Now; //TODO: UtcNow?
                auditedEntry.Entity.LastModifierUserId = WSFSession.UserId;
            }
        }

        private void HandleSoftDelete(DbEntityEntry entry)
        {
            if (entry.Entity is ISoftDelete)
            {
                var softDeleteEntry = entry.Cast<ISoftDelete>();

                softDeleteEntry.State = EntityState.Unchanged;
                softDeleteEntry.Entity.IsDeleted = true;
                
                if (entry.Entity is IDeletionAudited)
                {
                    var deletionAuditedEntry = entry.Cast<IDeletionAudited>();
                    deletionAuditedEntry.Entity.DeletionTime = DateTime.Now; //TODO: UtcNow?
                    deletionAuditedEntry.Entity.DeleterUserId = WSFSession.UserId;
                }
            }
        }
    }
}
