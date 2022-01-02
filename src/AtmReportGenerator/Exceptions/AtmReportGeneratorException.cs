using System;

namespace AtmReportGenerator.Exceptions
{
    public class AtmReportGeneratorException : Exception
    {
        public AtmReportGeneratorException(string message) : base(message)
        {
        }
    }
}