using System;

namespace FancyError.Models
{
    public class ClientConfiguration
    {
        /// <summary>
        /// The default application name that shows in the page title
        /// </summary>
        public string ApplicationName = "Application Error";

        /// <summary>
        /// The relative path for the error page template
        /// </summary>
        public string TemplateLocation = "Views/Shared/FancyError.html";

        /// <summary>
        /// How many errors need to occur before it is considered a trend
        /// </summary>
        public int ErrorCountBeforeTrend = 5;

        /// <summary>
        /// How long before the error trend resets
        /// </summary>
        public TimeSpan ErrorCountTimeout = new TimeSpan(0, 1, 0);
    }
}
