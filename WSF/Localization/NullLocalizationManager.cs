using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using WSF.Localization.Sources;

namespace WSF.Localization
{
    public class NullLocalizationManager : ILocalizationManager
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullLocalizationManager Instance { get { return SingletonInstance; } }
        private static readonly NullLocalizationManager SingletonInstance = new NullLocalizationManager();

        public LanguageInfo CurrentLanguage { get { return new LanguageInfo(Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.DisplayName); } }

        private readonly IReadOnlyList<LanguageInfo> _emptyLanguageArray = new LanguageInfo[0];

        private readonly IReadOnlyList<ILocalizationSource> _emptyLocalizationSourceArray = new ILocalizationSource[0];

        public IReadOnlyList<LanguageInfo> GetAllLanguages()
        {
            return _emptyLanguageArray;
        }

        public ILocalizationSource GetSource(string name)
        {
            return NullLocalizationSource.Instance;
        }

        public IReadOnlyList<ILocalizationSource> GetAllSources()
        {
            return _emptyLocalizationSourceArray;
        }

        public string GetString(string sourceName, string name)
        {
            return name;
        }

        public string GetString(string sourceName, string name, CultureInfo culture)
        {
            return name;
        }
    }
}