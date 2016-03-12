using FancyError.Models;
using System.Collections.Generic;

namespace FancyError
{
    internal static class InternalData
    {
        internal static ClientConfiguration Configuration;
        internal static string ErrorPageTemplate;

        internal static Dictionary<string, ErrorData> Encounters = new Dictionary<string, ErrorData>();
    }
}
