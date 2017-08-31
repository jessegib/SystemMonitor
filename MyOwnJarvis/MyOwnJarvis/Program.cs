using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;


namespace MyOwnJarvis
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            List<string> cpuMaxedOutMessages = new List<string>();
            cpuMaxedOutMessages.Add("Hey your CPU usage is");
            cpuMaxedOutMessages.Add("Did you know your CPU usage is");
            cpuMaxedOutMessages.Add("Oh that's cool, your CPU usage is ");
            
            Random rand = new Random();

            #region My Performance counters
            //Welcomes user to the program once opening the app
            
            Speak("Welcome to Windows Performance Monitor", VoiceGender.Female, 3);
            
            //This pulls current CPU usage in percents
            PerformanceCounter perfCpuCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

            //This is pulling current memory usage
            PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");

            //This let's us know how long the computer has been on in seconds
            PerformanceCounter perfUptimeCounter = new PerformanceCounter("System", "System Up Time");

            //This is pulling current network traffic
            PerformanceCounter perfNetworkCounter = new PerformanceCounter("Network Adapter", "Bytes Total/sec", "Realtek 8821AE Wireless LAN 802.11ac PCI-E NIC _2");

            #endregion

            int speechSpeed = 1;

            //Infinite While Loop
            while (true)
            {
                int currentCpuPercentage = (int)perfCpuCounter.NextValue();
                int currentAvailableMemory = (int)perfMemCounter.NextValue();
                int currentSystemUptime = (int)perfUptimeCounter.NextValue();
                int currentNetworkTraffic = (int)perfNetworkCounter.NextValue();

                //Converts the time from seconds into days, hours, minutes, and seconds
                TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptimeCounter.NextValue());

                //Every second prints system usage
                Console.WriteLine("CPU Usage is:                 {0}%", currentCpuPercentage);
                Console.WriteLine("Memory Available is:          {0} GB", currentAvailableMemory / 1024);
                Console.WriteLine("Network Traffic is:           {0} kilobytes/sec", currentNetworkTraffic / 1024);
                Console.WriteLine("Computer has been on for      {0} days {1} hours {2} minutes {3} seconds\n",
                    uptimeSpan.Days,
                    uptimeSpan.Hours,
                    uptimeSpan.Minutes,
                    uptimeSpan.Seconds, currentNetworkTraffic / 1024);




                //Speak to user the current usage data
                string cpuUsageVocal = cpuMaxedOutMessages[rand.Next(3)];
                string cpuUsageVocalPercent = String.Format("{0} percent", currentCpuPercentage);
                Speak(cpuUsageVocal, VoiceGender.Female, 1);
                Speak(cpuUsageVocalPercent, VoiceGender.Female, 1);
                
                #region Logic
                if (currentAvailableMemory <= 5)
                {
                    if (currentAvailableMemory < 1)
                    {
                        string memUsageVocal = String.Format("Woah! Something is using a lot of memory");
                        Speak(memUsageVocal, VoiceGender.Male);
                    }
                    else
                    {
                        string memUsageVocal = String.Format("Memory available is {0} Gigabytes", currentAvailableMemory / 1024);
                        Speak(memUsageVocal, VoiceGender.Female);
                    }
                }

                if (speechSpeed < 5)
                {
                    {
                        speechSpeed++;
                    }
                    if (speechSpeed == 5)
                    {
                        OpenWebsite("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                    }
                    else
                    {
                        string SystemUptimeVocal = String.Format("Computer has been on for {0} days {1} hours {2} minutes {3} seconds",
                            uptimeSpan.Days,
                            uptimeSpan.Hours,
                            uptimeSpan.Minutes,
                            uptimeSpan.Seconds
                            );
                        Speak(SystemUptimeVocal, VoiceGender.Male, speechSpeed);
                    }
                }

                if (currentNetworkTraffic > 1000000)
                {
                    string networkTrafficVocal = String.Format("Network Traffic is {0} Kilobytes per second", currentNetworkTraffic / 1024);
                    Speak(networkTrafficVocal, VoiceGender.Female);
                }
                #endregion

                Thread.Sleep(2000);
                //end of loop
            }
        }
        #region Functions
        //Speaks with a select voice
        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }
        public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voiceGender);
        }
        public static void OpenWebsite(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }
        #endregion
    }
}
