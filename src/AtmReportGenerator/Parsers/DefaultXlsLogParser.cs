using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using AtmReportGenerator.Entities;
using AtmReportGenerator.Exceptions;
using ExcelDataReader;

namespace AtmReportGenerator.Parsers
{
    public class DefaultXlsLogParser : ILogParser
    {
        private readonly ReportGeneratorOptions _generatorOptions;

        public DefaultXlsLogParser(ReportGeneratorOptions generatorOptions)
        {
            _generatorOptions = generatorOptions;
        }

        public AtmLog ParseLog(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();
                    
                    var sheet = dataSet.Tables["Sheet1"];
                    if (sheet == null) throw new AtmLogParseException("Table 'Sheet1' not found");
                    
                    var atmInfo = (string)sheet.Rows[1].ItemArray[0];
                    if (string.IsNullOrEmpty(atmInfo)) throw new AtmLogParseException("Cannot find atm info header");

                    var atmId = atmInfo.Split(' ')[0];
                    if (string.IsNullOrEmpty(atmInfo)) throw new AtmLogParseException($"Cannot atm id from header {atmInfo}");

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

        private List<AtmLogRecord> ParseRows(DataTable sheet)
        {
            var data = new List<AtmLogRecord>();

            for (var i = 5; i < sheet.Rows.Count; i++)
            {
                var row = sheet.Rows[i];
                var logRecord = new AtmLogRecord
                {
                    Time = DateTime.ParseExact((string)row[0], _generatorOptions.DateFormat, CultureInfo.InvariantCulture),
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