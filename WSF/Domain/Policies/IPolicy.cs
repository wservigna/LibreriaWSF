﻿using WSF.Dependency;

namespace WSF.Domain.Policies
{
    /// <summary>
    /// This interface must be implemented by all Policy classes/interfaces to identify them by convention.
    /// </summary>
    public interface IPolicy : ITransientDependency
    {

    }
}
