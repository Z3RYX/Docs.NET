using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Traceback
{
    public static class Logger
    {
        static string file = "logs/traceback_" + DateTime.UtcNow.Day + "_" + DateTime.UtcNow.Month + "_" + DateTime.UtcNow.Year + ".txt";

        public static void Write(string log)
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (!File.Exists(file))
                File.Create(file);
            else
                Console.WriteLine("Traceback exists");
            StreamWriter w = new StreamWriter(file, true);
            w.WriteLine($"[{DateTime.UtcNow.ToShortTimeString()}] {log}");
            Console.WriteLine($"[{DateTime.UtcNow.ToShortTimeString()}] {log}");
            w.Close();
        }
    }
}
