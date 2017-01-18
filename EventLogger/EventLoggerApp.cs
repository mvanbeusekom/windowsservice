using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EventLogger
{
    public class EventLoggerApp
    {
        #region Member fields
        // Private fields
        private Thread _thread;
        private EventLog _log;
        #endregion Member fields

        #region Private methods
        private void Execute()
        {
            // Loop until the thread gets aborted
            try
            {
                while (true)
                {
                    // Write the current time to the eventlog
                    _log.WriteEntry(string.Format("INFO (EventLoggerApp.Execute): Current time is: {0}.", DateTime.Now.ToString("HH:mm:ss")));

                    // Sleep for one minute 
                    Thread.Sleep(60000);
                }
            }
            catch (ThreadAbortException)
            {
                _log.WriteEntry("INFO (EventLoggerApp.Execute): Thread aborted.");
            }
        }
        #endregion Private methods

        #region Public methods
        public void Start()
        { 
            // Check if the EventLoggerService Event Log Source exists, when not
            // create it
            if (!EventLog.SourceExists("EventLoggerSource"))
                EventLog.CreateEventSource("EventLoggerSource", "Event Logger");

            _log = new EventLog();
			_log.Source = "EventLoggerSource";


            _thread = new Thread(new ThreadStart(Execute));
			_thread.Start();
        }

        public void Stop()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread.Join();
            }
        }
        #endregion Public methods
    }
}
