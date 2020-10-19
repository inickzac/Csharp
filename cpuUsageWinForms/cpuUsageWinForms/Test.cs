using LiveCharts.Geared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cpuUsageWinForms
{
    class Test
    {

        [DllImport(@"cpuUsege.dll", SetLastError = true)]
        static extern double get_cpu_usege();
        public event Action<double> cpuUsage;
         bool IsReading { get; set; }
        public GearedValues<double> Values { get; set; }
        public double Count { get; set; }
        public double CurrentLecture { get; set; }
        public bool IsHot { get; set; }
        private double _trend;
        public bool threadIsWork { get; private set; } = false;

        public Test()
        {
            Values = new GearedValues<double>().WithQuality(Quality.High);
        }

        public void Read()
        {
            if (IsReading) return;
            const int keepRecords = 100;
            IsReading = true;

            Action readFromTread = () =>
            {
                threadIsWork = true;
                while (IsReading)
                {
                    Thread.Sleep(1);
                    var r = new Random();
                    _trend = get_cpu_usege();
                    if (Marshal.GetLastWin32Error() != 0)
                    {
                        Exception exception = new Exception(Marshal.GetLastWin32Error().ToString()); 
                    }
                    cpuUsage?.Invoke(_trend);
                    var first = Values.DefaultIfEmpty(0).FirstOrDefault();
                    if (Values.Count > keepRecords - 1) Values.Remove(first);
                    if (Values.Count < keepRecords) Values.Add(_trend);
                    IsHot = _trend > 0;
                    Count = Values.Count;
                    CurrentLecture = _trend;
                }
                threadIsWork = false;    
            };

            Task.Factory.StartNew(readFromTread);
        }

        public void close()
        {
            cpuUsage = null;
            IsReading = false;         
        }
    }
}
