using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Terraform
{
    public static class Handler
    {
        public static string ExternalProcess(string cmd, string[] args, ILogger log, string input = "")
        {
            log.LogInformation($"Starting external process for {cmd} with arguments {args}");

            string output = "";
            var exitCode = 0;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = cmd;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                foreach (string arg in args) { p.StartInfo.ArgumentList.Add(arg); }
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;

                p.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);
                p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                
                p.Start();
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                var hasInput = !input.Equals("");
                Task writer = null;
                if(hasInput)
                {
                    writer = p.StandardInput.WriteAsync(input);
                }

                while(!p.StandardOutput.EndOfStream)
                {
                    var outputLine = p.StandardOutput.ReadLine();
                    output += outputLine + Environment.NewLine;
                }

                if(hasInput && writer != null)
                {
                    writer.GetAwaiter().GetResult();
                }
                
                p.WaitForExit();
                exitCode = p.ExitCode;
            }
            return output;
        }

        private static void OutputHandler(object sendingProcess,
             DataReceivedEventArgs outputLine)
        {
            if (!String.IsNullOrEmpty(outputLine.Data))
            {
            }
        }

        private static void ErrorHandler(object sendingProcess,
            DataReceivedEventArgs errLine)
        {
            if (!String.IsNullOrEmpty(errLine.Data))
            {
            }
        }
    }
}