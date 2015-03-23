using System.Globalization;
using System.Text;
using System.Web.Mvc;
using WSF.Authorization;
using WSF.Configuration;
using WSF.Localization;
using WSF.Localization.Sources;
using WSF.Reflection;
using WSF.Runtime.Session;
using WSF.Web.Models;
using WSF.Web.Mvc.Controllers.Results;
using Castle.Core.Logging;

namespace WSF.Web.Mvc.Controllers
{
    /// <summary>
    /// Base class for all MVC Controllers in WSF system.
    /// </summary>
    public abstract class WSFController : Controller
    {
        /// <summary>
        /// Gets current session information.
        /// </summary>
        public IWSFSession CurrentSession { get; set; }

        /// <summary>
        /// Reference to the permission manager.
        /// </summary>
        public IPermissionManager PermissionManager { get; set; }

        /// <summary>
        /// Reference to the setting manager.
        /// </summary>
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to the localization manager.
        /// </summary>
        public ILocalizationManager LocalizationManager { protected get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// </summary>
        protected string LocalizationSourceName
        {
            get { return LocalizationSource.Name; }
            set { LocalizationSource = LocalizationManager.GetSource(value); }
        }

        /// <summary>
        /// Gets localization source.
        /// It's valid if <see cref="LocalizationSourceName"/> is set.
        /// </summary>
        protected ILocalizationSource LocalizationSource { get; private set; }

        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFController()
        {
            CurrentSession = NullWSFSession.Instance;
            Logger = NullLogger.Instance;
            LocalizationSource = NullLocalizationSource.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }

        /// <summary>
        /// Json the specified data, contentType, contentEncoding and behavior.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="contentEncoding">Content encoding.</param>
        /// <param name="behavior">Behavior.</param>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (data == null)
            {
                data = new AjaxResponse();
            }
            else if (!ReflectionHelper.IsAssignableToGenericType(data.GetType(), typeof(AjaxResponse<>)))
            {
                data = new AjaxResponse(data);
            }

            return new WSFJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}