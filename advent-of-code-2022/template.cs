using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class DayXX
    {
        public static void RunDayXX()
        {
            // created TODO: add date
            // https://adventofcode.com/2022/day/XX

            Console.WriteLine("--- Day XX: ... ---");

            // data file
            var df = "dayXX-test.txt";
            //var df = "dayXX-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
            var input = new Queue<string>();
            string? line;
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fn, Encoding.UTF8))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //var items = line.Trim().Split(',').ToList();
                        input.Enqueue(line);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                System.Environment.Exit(-1); // fail
            }

            // Quality control
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Data Quality Control Checks:");
            Console.WriteLine($"Input file name: {fn}");
            Console.WriteLine($"Number lines: {input.Count}");
            Console.WriteLine($"Input first line: {input.FirstOrDefault("EMPTY FILE")}");
            Console.WriteLine($"Input last line: {input.LastOrDefault("EMPTY FILE")}");
            Console.WriteLine("---End input file specs---{0}{0}", Environment.NewLine);

            // Timing
            DateTime utcDateStart = DateTime.UtcNow;
            Stopwatch stopwatch = Stopwatch.StartNew();  // start stopwatch
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");


            // ---------------------------
            // Part One
            // ---------------------------
            // TODO
            Console.WriteLine("Day XX Part 1 TBD");


            // ---------------------------
            // Part Two
            // ---------------------------
            // TODO
            Console.WriteLine("Day XX Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

