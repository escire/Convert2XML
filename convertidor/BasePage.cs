using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace jats
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage() { }

        protected override void InitializeCulture()
        {
            

            string lang = Request.UserLanguages[0].ToLower().Substring(0, 2);
            string culture = string.Empty;

            switch (lang)
            {
                case "es":
                    culture = "es-MX";
                    break;
                case "en":
                    culture = "en-US";
                    break;
                case "pt":
                    culture = "pt-BR";
                    break;
                default:
                    culture = "es-MX";
                    break;
            }

            Session["lenguaje"] = lang;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            base.InitializeCulture();
        }
    }
}