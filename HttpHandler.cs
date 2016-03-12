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
            InternalData.ErrorPageTemplate = GetErrorPageTemplate();
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

            var customErrorPage = RenderErrorPage(error);

            app.Context.Response.Clear();
            app.Context.Response.Write(customErrorPage);
            app.Context.Response.End();
        }

        /// <summary>
        /// Loads the error page template from the file
        /// </summary>
        /// <returns></returns>
        private string GetErrorPageTemplate()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var errorPage = System.IO.File.ReadAllText(baseDirectory + InternalData.Configuration.TemplateLocation);

            return errorPage;
        }

        /// <summary>
        /// Builds the error page using the exception data and page template
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string RenderErrorPage(Exception ex)
        {
            var exceptionTypeData = ex.GetType();
            var exceptionType = string.Format("{0}.{1}", exceptionTypeData.Namespace, exceptionTypeData.Name);

            return InternalData.ErrorPageTemplate
                           .Replace("{EXCEPTION_TYPE}", exceptionType)
                           .Replace("{EXCEPTION_MESSAGE}", ex.Message)
                           .Replace("{OUTAGE}", IsPossibleOutage(ex) ? "show-outage" : "hide-outage")
                           .Replace("{APP_TITLE}", InternalData.Configuration.ApplicationName);
        }

        /// <summary>
        /// Checks to see if the issue is a possible application outage. This is done
        /// based on exception data and how often we are seeing it come in
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool IsPossibleOutage(Exception ex)
        {
            var isMajorIssue = false;

            var exceptionType = ex.GetType();
            var errorKey = string.Format("{0}-{1}", exceptionType.Namespace, exceptionType.Name);

            if (InternalData.Encounters.ContainsKey(errorKey))
            {
                var encounter = InternalData.Encounters[errorKey];
                if ((DateTime.UtcNow - encounter.LastEncounter) < InternalData.Configuration.ErrorCountTimeout
                    && encounter.TotalEncounters >= InternalData.Configuration.ErrorCountBeforeTrend)
                {
                    isMajorIssue = true;
                }

                encounter.LastEncounter = DateTime.UtcNow;
                encounter.TotalEncounters++;
            } else
            {
                InternalData.Encounters.Add(errorKey, new Models.ErrorData
                {
                    LastEncounter = DateTime.UtcNow,
                    TotalEncounters = 1
                });
            }

            return isMajorIssue;
        }
    }
}
