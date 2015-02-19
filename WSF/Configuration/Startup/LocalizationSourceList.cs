using System.Collections.Generic;
using WSF.Localization.Sources;

namespace WSF.Configuration.Startup
{
    /// <summary>
    /// A specialized list to store <see cref="ILocalizationSource"/> object.
    /// </summary>
    public class LocalizationSourceList : List<ILocalizationSource>, ILocalizationSourceList
    {
        
    }
}