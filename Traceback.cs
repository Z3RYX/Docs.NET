using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Traceback
{
    public static class Logger
    {
        static string file = "logs/traceback_" + DateTime.UtcNow.ToShortDateString() + ".txt";

        public static void Write(string log)
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            StreamWriter w = new StreamWriter(file, true);
            w.WriteLine($"[{DateTime.UtcNow.ToShortTimeString()}] {log}");
            w.Close();
        }
    }
}
