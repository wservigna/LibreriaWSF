using System;
using WSF.Localization;
using WSF.Localization.Sources;

namespace WSF.Web.Localization
{
    /// <summary>
    /// This class is used to simplify getting localized messages in this assembly.
    /// </summary>
    internal static class WSFWebLocalizedMessages
    {
        private const string SourceName = "WSFWeb";

        public static string InternalServerError { get { return L("InternalServerError"); } }

        public static string ValidationError { get { return L("ValidationError"); } }
        
        private static readonly ILocalizationSource Source;

        static WSFWebLocalizedMessages()
        {
            Source = LocalizationHelper.GetSource(SourceName);
        }

        private static string L(string name)
        {
            try
            {
                return Source.GetString(name);
            }
            catch (Exception)
            {
                return name;
            }
        }
    }
}
