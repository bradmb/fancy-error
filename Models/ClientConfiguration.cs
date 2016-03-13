using System;

namespace FancyError.Models
{
    public class ClientConfiguration
    {
        /// <summary>
        /// The default application name that shows in the page title
        /// (Default: Application Error)
        /// </summary>
        public string ApplicationName = "Application Error";

        /// <summary>
        /// The relative path for the error page template. If not set, uses the internal embedded page
        /// </summary>
        public string ExternalTemplateLocation = "";

        /// <summary>
        /// How many errors need to occur before it is considered a trend
        /// (Default: 5)
        /// </summary>
        public int ErrorCountBeforeTrend = 5;

        /// <summary>
        /// How long before the error trend resets
        /// (Default: 1 minute)
        /// </summary>
        public TimeSpan ErrorCountTimeout = new TimeSpan(0, 1, 0);

        /// <summary>
        /// The link to your application status page that shows known issues. If not set, hides that section of the page
        /// </summary>
        public string StatusLink = string.Empty;

        /// <summary>
        /// The link (or mailto: address) that will be used to direct people to your support page. If not set, hides that section of the page
        /// </summary>
        public string SupportLink = string.Empty;

        /// <summary>
        /// Makes it so the outage detection code will only trigger if the amount of hits to it are from unique visitors, instead of one persion hitting refresh over and over again
        /// </summary>
        public bool TrackUniqueVisitors = true;
    }
}
