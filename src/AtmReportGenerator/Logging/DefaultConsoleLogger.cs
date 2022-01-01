using System;
using System.Text;

namespace AtmReportGenerator.Logging
{
    public class DefaultConsoleLogger : ILogger
    {
        public DefaultConsoleLogger()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void LogInformation(string message)
        {
            Console.WriteLine(message);
        }
    }
}