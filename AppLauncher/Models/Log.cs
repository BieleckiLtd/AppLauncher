using System;

namespace AppLauncher.Models
{
    public class Log
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string Message { get; set; }
        public Environ Environment { get; set; } = new Environ();
    }
}
