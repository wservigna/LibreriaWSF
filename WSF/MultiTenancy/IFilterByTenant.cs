using WSF.Domain.Entities;

namespace WSF.MultiTenancy
{
    /// <summary>
    /// Implement this interface for entities those must be filtered by Tenant always.
    /// If an <see cref="Entity{TPrimaryKey}"/> implements this interface, only current tenant's
    /// entities will be available in the application.
    /// </summary>
    public interface IFilterByTenant
    {
        
    }
}