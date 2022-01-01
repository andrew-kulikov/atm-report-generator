using System;

namespace AtmReportGenerator.Entities
{
    public abstract class AtmEvent
    {
        public string AtmId { get; set; }
        public DateTime Time { get; set; }
    }
}