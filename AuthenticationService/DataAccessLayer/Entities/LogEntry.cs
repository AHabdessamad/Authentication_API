using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Entities
{
    public class LogEntry
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; } //Timestamp for when the log entry was created
        public string? Level { get; set; } // Log level (Trace, Debug, Info, Warn, Error, Fatal)
        public string? Message { get; set; } // The log message content
        public string? Logger { get; set; } //name of the logger (usually the class name)
        public string? Exception { get; set; } // Exception details, if any

    }
}
