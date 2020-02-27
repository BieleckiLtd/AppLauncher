using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AppLauncher.Utils
{
    public class Logger
    {
        private static readonly string _logPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public List<Log> Logs { get; set; } = new List<Log>();
        private const string _logFileName = @"logs.log";
        private static readonly string _logFilePath = $@"{_logPath}\{_logFileName}";

        public static void Log(object o, bool isVerbose = true)
        {
            var logs = Instance.Logs;
            var log = new Log();
            if (o is string message)
            {
                log.Message = message;
            }
            else if (o is Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"{DateTime.Now} - Exception catched:");
                while (e != null)
                {
                    sb.AppendLine($"Exception name: {e.GetType().FullName}");
                    sb.AppendLine($"Message: {e.Message}");
                    sb.AppendLine($"Stack trace: {e.StackTrace}");

                    e = e.InnerException;
                }
                log.Message = sb.ToString();
            }
            logs.Add(log);
            if (isVerbose) Debug.Print($"Time: {log.DateTime.ToLocalTime()} Log: {log.Message}");
        }
        public static void DumpToFile()
        {
            var sb = new StringBuilder();

            foreach (var log in Instance.Logs)
            {
                sb.AppendLine($"-----------------------------------------------------------------------------");
                sb.AppendLine($"Date: {log.DateTime.ToLocalTime().ToString()}");
                sb.AppendLine($"EnvironmentUserName: {log.Environment.EnvironmentUserName}");
                sb.AppendLine($"EnvironmentUserDomainName: {log.Environment.EnvironmentUserDomainName}");
                sb.AppendLine($"EnvironmentOSVersion: {log.Environment.EnvironmentOSVersion}");
                sb.AppendLine($"EnvironmentMachineName: {log.Environment.EnvironmentMachineName}");
                sb.AppendLine($"EnvironmentCurrentDirectory: {log.Environment.EnvironmentCurrentDirectory}");
                sb.AppendLine($"EnvironmentCurrentManagedThreadId: {log.Environment.EnvironmentCurrentManagedThreadId}");
                sb.AppendLine($"EnvironmentTicks: {log.Environment.EnvironmentTicks}");
                sb.AppendLine($"Message: {log.Message}");
            }
            try
            {
                if (sb.Length > 0)
                {
                    using StreamWriter writer = new StreamWriter(_logFilePath, true);
                    writer.WriteLine(sb.ToString());
                    File.SetAttributes(_logFilePath, File.GetAttributes(_logFilePath) | FileAttributes.Hidden);
                }
            }
            catch
            {
                Debug.Print($"Failed to dump logger to file");
            }
        }
        public static void DumpToXmlFile()
        {
            string fileName = @"logs.xml";
            string filePath = Path.Combine(_logPath, fileName);

            using var writer = new StreamWriter(filePath);
            var serializer = new XmlSerializer(Instance.GetType());
            serializer.Serialize(writer, Instance);
            writer.Flush();

        }


        #region Singleton Implementation
        private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());
        public static Logger Instance => instance.Value;
        #endregion
    }
}
