using System;
using System.Diagnostics;
using System.Timers;

namespace SharpProcesses
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            String targetProcess = args[0];
            Double maxLife = Double.Parse(args[1]); // in minutes
            Double monitorFreq = Double.Parse(args[2]) * 60 * 1000; // received in minutes, transformed to miliseconds

            Timer scheduler = new Timer(monitorFreq);
            scheduler.AutoReset = true;
            scheduler.Elapsed += (sender, e) => monitorProcess(sender, e, targetProcess, maxLife);
            scheduler.Enabled = true;

            Console.WriteLine("\nMonitoring processes. Start time: " + DateTime.Now.ToString());
            Console.WriteLine("Press Enter to stop monitoring...");
            Console.ReadLine();

            scheduler.Stop();
            scheduler.Dispose();
        }

        static void monitorProcess(Object source, ElapsedEventArgs e, String targetProcess, Double maxLife)
        {
            Console.WriteLine("Monitoring event triggered at " + DateTime.Now.ToString());

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == targetProcess && (DateTime.Now - process.StartTime).TotalMinutes > maxLife)
                {
                    Console.WriteLine(process.ProcessName + " lasted " + (DateTime.Now - process.StartTime).TotalMinutes + "minutes, process was killed.");
                    process.Kill();
                }
            }
        }
    }
}
