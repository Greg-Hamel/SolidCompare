using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

namespace SolidCompare
{
    class Reporter
    {
        private static string folderpath, reportPath;
        static List<string> delay = new List<string>();

        public Reporter(string path)
        {
            
            Logger.Info(path);
            folderpath = path;
            Initialize();
            
        }

        private static void Initialize()
        {
            reportPath = folderpath + @"\ComparisonReport.txt";
            using (StreamWriter file = new StreamWriter(reportPath))
            {
                file.WriteLine("SolidCompare Comparison Report - " + DateTime.Now.ToLongDateString() + " @ " + DateTime.Now.ToShortTimeString());
            }
        }

        public void AddComparison(string documentTitle1, string documentTitle2)
        {
            PrintContoured("Comparison between " + documentTitle1 + " & " + documentTitle2);
        }

        public void AddSection(string documentTitle1, string documentTitle2)
        {
            PrintHighlight("Comparison between " + documentTitle1 + " & " + documentTitle2);
        }

        public void AddSection(string text)
        {
            PrintHighlight(text);
        }

        public void AddSubSection(string text)
        {
            PrintSub(text);
        }

        public void AddLine(string text)
        {
            if (text.Contains("\n"))
            {
                string[] lines;
                lines = CutWhitespace(text);
                foreach (string line in lines)
                {
                    Print(line);
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    Print(text);
                }
            }
        }

        public void AddDelayedSection(string text)
        {
            PrintHighlight(text, true);
        }

        public void AddDelayedSubSection(string text)
        {
            PrintSub(text, true);
        }

        public void AddDelayedLine(string text)
        {
            if (text.Contains("\n"))
            {
                string[] lines;
                lines = CutWhitespace(text);
                foreach (string line in lines)
                {
                    Print(line, true);
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    Print(text, true);
                }
            }
        }

        private void PrintContoured(string text, bool delayed=false)
        {
            string contourer;
            string contour = "";

            contourer = "#";

            for (int i = 0; i < (text.Length + 4); i++)
            {
                contour += contourer;
            }

            if (delayed)
            {
                delay.Add("");
                delay.Add(contour);
                delay.Add("# " + text + " #");
                delay.Add(contour);
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    file.WriteLine("");
                    file.WriteLine(contour);
                    file.WriteLine("# " + text + " #");
                    file.WriteLine(contour);
                }
            }
        }
            

        private void PrintHighlight(string text, bool delayed=false)
        {
            string highlighter;
            string highlight = "";

            highlighter = "-";

            for (int i = 0; i < (text.Length + 2); i++)
            {
                highlight += highlighter;
            }

            if (delayed)
            {
                delay.Add("");
                delay.Add("|" + highlight);
                delay.Add("| " + text);
                delay.Add("|" + highlight);
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    file.WriteLine("");
                    file.WriteLine("|" + highlight);
                    file.WriteLine("| " + text);
                    file.WriteLine("|" + highlight);
                }
            }
        }

        private void PrintSub(string text, bool delayed = false)
        {
            string underliner;
            string underline = "";

            underliner = "-";

            for (int i = 0; i < (text.Length + 2); i++)
            {
                underline += underliner;
            }

            if (delayed)
            {
                delay.Add("|");
                delay.Add("|+ " + text);
                delay.Add("|+" + underline);
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    file.WriteLine("|");
                    file.WriteLine("|+ " + text);
                    file.WriteLine("|+" + underline);
                }
            }
        }

        private void Print(string text, bool delayed = false)
        {
            if (delayed)
            {
                delay.Add("|+ " + text);
            }
            else
            {
                using (StreamWriter file = new StreamWriter(reportPath, true))
                {
                    file.WriteLine("|+ " + text);
                }
            }
        }

        public void PrintDelayed()
        {
            using (StreamWriter file = new StreamWriter(reportPath, true))
            {
                foreach (string line in delay)
                {
                    file.WriteLine(line);
                }
            }
        }

        private string[] CutWhitespace(string text)
        {
            string[] lines;

            lines = text.Split('\n');
            return lines;
        }
    }
}
