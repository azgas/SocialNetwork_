using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPX_Template1.TemplateUtilities.AppConfiguration
{
    public class AppConfig
    {
        private static AppConfig appConfigInstance;
        public string ClientId { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }

        private AppConfig() { }

        public static AppConfig GetAppConfig
        {
            get
            {
                if (appConfigInstance == null)
                {
                    appConfigInstance = new AppConfig();
                }
                return appConfigInstance;
            }
        }
    }
}