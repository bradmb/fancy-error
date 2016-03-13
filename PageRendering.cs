using System;

namespace FancyError
{
    internal static class PageRendering
    {
        /// <summary>
        /// Loads the error page template from the file
        /// </summary>
        /// <returns></returns>
        internal static string GetErrorPageTemplate()
        {
            if (!string.IsNullOrWhiteSpace(InternalData.Configuration.ExternalTemplateLocation))
            {
                var templatePath = AppDomain.CurrentDomain.BaseDirectory + InternalData.Configuration.ExternalTemplateLocation;
                
                if (System.IO.File.Exists(templatePath)) {
                    return (System.IO.File.ReadAllText(templatePath));
                }
            }

            return ErrorTemplate.BasePage.Replace("{STYLESHEET}", ErrorTemplate.StyleSheet);
        }

        /// <summary>
        /// Builds the error page using the exception data and page template
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        internal static string RenderErrorPage(Exception ex, bool potentialOutage)
        {
            var exceptionTypeData = ex.GetType();
            var exceptionType = string.Format("{0}.{1}", exceptionTypeData.Namespace, exceptionTypeData.Name);

            var statusPage = !string.IsNullOrWhiteSpace(InternalData.Configuration.StatusLink)
                             ? ErrorTemplate.StatusPageText.Replace("{STATUS_URL}", InternalData.Configuration.StatusLink)
                             : string.Empty;

            var supportPage = !string.IsNullOrWhiteSpace(InternalData.Configuration.SupportLink)
                              ? ErrorTemplate.SupportPageText.Replace("{SUPPORT_URL}", InternalData.Configuration.SupportLink)
                              : string.Empty;

            return InternalData.ErrorPageTemplate
                           .Replace("{EXCEPTION_TYPE}", exceptionType)
                           .Replace("{EXCEPTION_MESSAGE}", ex.Message)
                           .Replace("{STATUS_PAGE_TEXT}", statusPage)
                           .Replace("{SUPPORT_PAGE_TEXT}", supportPage)
                           .Replace("{OUTAGE}", potentialOutage ? "show-outage" : "hide-outage")
                           .Replace("{APP_TITLE}", InternalData.Configuration.ApplicationName);
        }
    }
}
