using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;

namespace EventLogger
{
	static class Program
	{
		[DllImport("kernel32.dll")]
		public static extern Boolean AllocConsole();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if (args.Length > 0 && args[0].ToLower() == "/console")
			{
				AllocConsole();

				EventLoggerApp app = new EventLoggerApp();
				app.Start();

				string input = string.Empty;
				Console.Write("Eventlogger Console started. Type 'exit' to stop the application: ");

				// Wait for the user to exit the application                
				while (input.ToLower() != "exit") input = Console.ReadLine();

				// Stop the application.
				app.Stop();
			}
			else
			{
				// Initialize and run the service
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] { new EventLoggerService() };
				ServiceBase.Run(ServicesToRun);
			}
		}
	}
}