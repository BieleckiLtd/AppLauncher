using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLauncher.Models
{
    public class Environ
    {
        public string EnvironmentUserName { get; set; } = Environment.UserName;
        public string EnvironmentUserDomainName { get; set; } = Environment.UserDomainName;
        public string EnvironmentCurrentDirectory { get; set; } = Environment.CurrentDirectory;
        public string EnvironmentMachineName { get; set; } = Environment.MachineName;
        public string EnvironmentOSVersion { get; set; } = Environment.OSVersion.ToString();
        public int EnvironmentTicks { get; set; } = Environment.TickCount;
        public int EnvironmentCurrentManagedThreadId { get; set; } = Environment.CurrentManagedThreadId;
    }
}
