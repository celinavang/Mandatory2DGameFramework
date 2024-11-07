using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Models
{
    public class MyLogger
    {
        private static MyLogger? _instance;

        public MyLogger()
        {
            Trace.TraceInformation("Logger created");
        }

        public void Start()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public static MyLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MyLogger();
            }
            return _instance;
        }

        public static void TraceInformation(string message)
        {
            Trace.TraceInformation(message);
            Trace.Flush();
        }

        public static void TraceError(string message)
        {
            Trace.TraceError(message);
            Trace.Flush();
        }

        public void AddTraceLIstener(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }
        public void RemoveTraceLIstener(TraceListener listener)
        {
            Trace.Listeners.Remove(listener);
        }
    }
}
