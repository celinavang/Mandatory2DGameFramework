using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApplication
{
    class TryLog
    {
        private const string logname = "MyLog";
        private static TraceSource _trace = new("MyLog");

        public TryLog()
        {
   
        }

        public void Start()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        public void AddTraceLIstener(TraceListener listener)
        {
            _trace.Listeners.Add(listener);
        }
        public void RemoveTraceLIstener(TraceListener listener)
        {
            _trace.Listeners.Remove(listener);
        }
    }
}
