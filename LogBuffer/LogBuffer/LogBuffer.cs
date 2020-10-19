using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LogBuffer
{
    class LogBuffer
    {
        public event Action<string> MessageEvent;
        public event Action<List<string>> ShowLogList;
        public event Action<int> ShowQuantityitemsInBuffer;
        string fileName;
        int flushLimit;
        int flushPeriodInMilliseconds;
        string beginSplit = "<$";
        string endSplit = "$>";
        List<string> buffer = new List<string>();
        bool fileBusy = false;

        public int FlushLimit { get => flushLimit; set => flushLimit = value; }
        public int FlushPeriodInMilliseconds { get => flushPeriodInMilliseconds; set => flushPeriodInMilliseconds = value; }
        public string FileName { get => fileName; set { if (!File.Exists(value)) { File.Create(value); fileName = value; } } }

        public LogBuffer(
             string fileName, // имя файла на диске
             int flushLimit, // размер буфера в сообщениях
             int flushPeriodInMilliseconds // интервал сохранения накопленных сообщений
             )
        {
            this.fileName = fileName;
            this.flushLimit = flushLimit;
            this.flushPeriodInMilliseconds = flushPeriodInMilliseconds;
            ShowLogList?.Invoke(readAllLog());
            Action WorkBuffer = () =>
            {
                while (true)
                {
                    Thread.Sleep(flushPeriodInMilliseconds);
                    if (buffer.Count > 0)
                    {
                        fileBusy = true;
                        WriteBufferToFile();
                        buffer.Clear();
                        ShowQuantityitemsInBuffer?.Invoke(buffer.Count);
                        fileBusy = false; 
                    }
                } };
            Task.Factory.StartNew(WorkBuffer);
        }
        public bool Add(string item)
        {           
            if(buffer.Count>=flushLimit)
            {
                while (fileBusy) { Thread.Sleep(100); };
                if (buffer.Count >= flushLimit) {
                    WriteBufferToFile(); }
                buffer.Clear();
                ShowQuantityitemsInBuffer?.Invoke(buffer.Count);
            }
            buffer.Add(item);
            ShowQuantityitemsInBuffer?.Invoke(buffer.Count);
            return true;
        }

        private void WriteBufferToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.Default))
                {
                    foreach (var item in buffer)
                    {
                        sw.WriteLine("{0} {1} {2}", beginSplit, item, endSplit);
                    }
                }
               
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke(e.Message);              
            }
        }

        public List<string> readAllLog()
        {
            List<string> items = new List<string>();
            try
            {
                Regex regex = new Regex(@"<\$([^\$>]+)\$>");              
                while (fileBusy) { Thread.Sleep(100); }
                using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                {
                    string itemsFull = sr.ReadToEnd();
                    MatchCollection regImems = regex.Matches(itemsFull);
                    foreach (Match item in regImems)
                    {
                        items.Add(item.Groups[1].Value);
                    }
                }
                items.AddRange(buffer);
            }
            catch (Exception e)
            {
                MessageEvent?.Invoke(e.Message);              
            }
            return items;
        }

        public void Close()
        {
            WriteBufferToFile();
        }
    }
}
