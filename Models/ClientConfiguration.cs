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

        /// <summary>
        /// The link to your application status page that shows known issues
        /// </summary>
        public string StatusLink = "javascript:void(0);";

        /// <summary>
        /// The link (or mailto: address) that will be used to direct people to your support page
        /// </summary>
        public string SupportLink = "javascript:void(0);";
    }
}
