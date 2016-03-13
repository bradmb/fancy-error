using System;
using System.Collections.Generic;

namespace FancyError.Models
{
    internal class ErrorData
    {
        internal List<string> UsersEncountered { get; set; }
        internal DateTime LastEncounter { get; set; }
    }
}
