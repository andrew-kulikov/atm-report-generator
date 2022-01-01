using System.Collections.Generic;

namespace AtmReportGenerator.Entities
{
    public class AtmLog
    {
        public string AtmInfo { get; set; }
        public string AtmId { get; set; }
        public List<AtmLogRecord> Logs { get; set; }
    }
}