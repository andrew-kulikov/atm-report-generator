using System;
using System.Collections.Generic;
using System.Linq;

namespace AtmReportGenerator.Entities
{
    public class AtmDailyReport
    {
        public string AtmId { get; set; }
        public DateTime Date { get; set; }
        public AtmWorkingDayStartReport WorkingDayStartReport { get; set; }
        public List<AtmEvent> AtmEvents { get; set; }

        public double CashLoad => AtmEvents.Where(e => e is AtmCashLoadEvent).Sum(e => ((AtmCashLoadEvent)e).Amount);
        public double CashUnload => AtmEvents.Where(e => e is AtmCashUnloadEvent).Sum(e => ((AtmCashUnloadEvent)e).Amount);
    }
}