using System;
using System.Web;
using System.Web.Mvc;
using WSF.Localization;
using WSF.Web.Mvc.Models;

namespace WSF.Web.Mvc.Controllers.Localization
{
    public class WSFLocalizationController : WSFController
    {
        public ActionResult ChangeCulture(string cultureName, string returnUrl = "")
        {
            if (!GlobalizationHelper.IsValidCultureCode(cultureName))
            {
                throw new WSFException("Unknown language: " + cultureName + ". It must be a valid culture!");
            }

            Response.Cookies.Add(new HttpCookie("WSF.Localization.CultureName", cultureName) { Expires = DateTime.Now.AddYears(2) });

            if (Request.IsAjaxRequest())
            {
                return Json(new MvcAjaxResponse(), JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
    }
}
