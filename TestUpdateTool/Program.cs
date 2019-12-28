using System;
using System.Diagnostics;
using System.IO;

namespace TestUpdateTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var adbExe = Path.Combine(Environment.CurrentDirectory, "platform-tools", "adb.exe");
            var updater = new Updater(new ConsoleLogger(), adbExe);

            //updater.LaunchExternalExecutable(adbExe, "devices");
            // adb shell 'pm list packages -f'
            updater.ListConnectedDevices();

            if (updater.CheckRideOnInstalled())
            {
                Console.WriteLine("found rideon version");
                updater.UninstallRideOn();
                Console.WriteLine("uninstalled");
            }

            Console.WriteLine("install new");
            updater.InstallRideOn();

            Console.WriteLine("installed!!!");
            Console.ReadKey();
        }
    }

    public interface ILogger
    {
        void LogText(string message);
    }

    internal class ConsoleLogger:ILogger
    {
        public void LogText(string message)
        {
            Console.WriteLine(message);
        }
    }

    internal class Updater
    {
        private ILogger Logger;
        public string AdbExePath { get; private set; }

        public Updater(ILogger _logger, string p_adbExePath)
        {
            AdbExePath = p_adbExePath;
            Logger = _logger;
        }

        public bool InstallRideOn()
        {
            var res = ExecuteAdbCommand("install " + Path.Combine(Environment.CurrentDirectory, "rideon.apk"));
            Logger.LogText(res);
            return res.Contains("Success");
        }

        public bool UninstallRideOn()
        {
            var res = ExecuteAdbCommand("uninstall com.aroslab.rideon");
            Logger.LogText(res);
            return res.Contains("Success");
        }

        public string ListConnectedDevices()
        {
            var res = ExecuteAdbCommand("devices");
            Logger.LogText(res);
            return res;
        }

        public bool CheckRideOnInstalled()
        {
            // adb shell 'pm list packages -f'
            var res = ExecuteAdbCommand("shell pm list packages -f");
            Logger.LogText(res);
            return res.Contains("rideon");

        }

        private string ExecuteAdbCommand(String arguments)
        {
            if (string.IsNullOrWhiteSpace(AdbExePath) == true)
            {
                var errorMessage = string.Format(" Path is not valid. LaunchExternalExecutable called with invalid argument executablePath was empty.");
                Logger.LogText(errorMessage);
                throw new ArgumentNullException(errorMessage);
            }

            String processOutput = "";

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = AdbExePath,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = arguments,
                RedirectStandardOutput = true
            };

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    processOutput = exeProcess.StandardOutput.ReadToEnd();
                }
            }
            catch (SystemException exception)
            {
                String errorMessage = String.Format("LaunchExternalExecutable - " +
                                                    "Device Failed to launch a tool with executable path" +
                                                    " { 0}. { 1} ", AdbExePath, exception.ToString());
                Logger.LogText(errorMessage);
                throw new Exception(errorMessage);
            }

            //Strip off extra characters - spaces, carriage returns, end of line etc
            processOutput = processOutput.Trim();
            processOutput = processOutput.TrimEnd(System.Environment.NewLine.ToCharArray());

            //Without this next change any text that contains
            //{X} will crash the String.Format inside the logger
            processOutput = processOutput.Replace('{', '[');
            processOutput = processOutput.Replace('}', ']');

            Logger.LogText("LaunchExternalExecutable called. Output from tool : " + processOutput);
            return processOutput;
        }
    }
}
