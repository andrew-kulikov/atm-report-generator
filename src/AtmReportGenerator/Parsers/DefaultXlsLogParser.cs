using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using AtmReportGenerator.Entities;
using ExcelDataReader;

namespace AtmReportGenerator.Parsers
{
    public class DefaultXlsLogParser : ILogParser
    {
        public AtmLog ParseLog(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();

                    // TODO: Add if not found
                    var sheet = dataSet.Tables["Sheet1"];

                    // TODO: Throw if not found - pass row
                    var atmInfo = (string)sheet.Rows[1].ItemArray[0];
                    var atmId = atmInfo.Split(' ')[0];

                    var data = ParseRows(sheet);

                    return new AtmLog
                    {
                        AtmInfo = atmInfo,
                        AtmId = atmId,
                        Logs = data
                    };
                }
            }
        }

        private static List<AtmLogRecord> ParseRows(DataTable sheet)
        {
            var data = new List<AtmLogRecord>();

            for (var i = 5; i < sheet.Rows.Count; i++)
            {
                var row = sheet.Rows[i];
                var logRecord = new AtmLogRecord
                {
                    // TODO: Throw if invalid format and pass format as optional parameter
                    Time = DateTime.ParseExact((string)row[0], "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                    RemainingCash = (double)row[1],
                    WithdrawAmount = (double)row[2],
                    ExpectedWithdrawAmount = (double)row[3],
                    ExpectedRemaining = (double)row[4]
                };

                data.Add(logRecord);
            }

            return data.OrderBy(d => d.Time).ToList();
        }
    }
}