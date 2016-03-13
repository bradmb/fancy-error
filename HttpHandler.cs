using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FancyError
{
    public class HttpHandler : IHttpModule
    {
        public void Dispose() { }

        /// <summary>
        /// Binds our library to the exception handling process
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.Error += OnException;
            InternalData.ErrorPageTemplate = PageRendering.GetErrorPageTemplate();
        }

        /// <summary>
        /// Renders the custom error page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnException(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var error = app.Server.GetLastError();

            var visitorGuid = InternalData.Configuration.TrackUniqueVisitors
                              ? app.Context.Request.UserHostAddress
                              : Guid.NewGuid().ToString();

            var customErrorPage = PageRendering.RenderErrorPage(error, IsPossibleOutage(error, visitorGuid));

            app.Context.Response.Clear();
            app.Context.Response.Write(customErrorPage);
            app.Context.Response.End();
        }

        /// <summary>
        /// Checks to see if the issue is a possible application outage. This is done
        /// based on exception data and how often we are seeing it come in
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsPossibleOutage(Exception ex, string clientAddress)
        {
            var isMajorIssue = false;

            var exceptionType = ex.GetType();
            var errorKey = string.Format("{0}-{1}", exceptionType.Namespace, exceptionType.Name);

            if (InternalData.Encounters.ContainsKey(errorKey))
            {
                var encounter = InternalData.Encounters[errorKey];
                var withinTimeoutLimits = (DateTime.UtcNow - encounter.LastEncounter) < InternalData.Configuration.ErrorCountTimeout;
                var meetsUserThreshold = encounter.UsersEncountered.Count >= InternalData.Configuration.ErrorCountBeforeTrend - 1;

                if (withinTimeoutLimits && meetsUserThreshold)
                {
                    isMajorIssue = true;
                }

                encounter.LastEncounter = DateTime.UtcNow;

                if (!encounter.UsersEncountered.Contains(clientAddress))
                {
                    encounter.UsersEncountered.Add(clientAddress);
                }
            } else
            {
                InternalData.Encounters.Add(errorKey, new Models.ErrorData
                {
                    LastEncounter = DateTime.UtcNow,
                    UsersEncountered = new List<string> { clientAddress }
                });
            }

            return isMajorIssue;
        }
    }
}
