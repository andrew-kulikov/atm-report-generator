﻿using System.Collections.Generic;

namespace AtmReportGenerator.Entities
{
    public class AtmReportRequest
    {
        public List<string> LogFilePaths { get; set; }
        public string DestinationFolder { get; set; }
        public string DateFormat { get; set; } = "dd.MM.yyyy HH:mm";
    }
}