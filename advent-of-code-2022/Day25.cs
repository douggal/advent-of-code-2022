using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day25
    {
        public static void RunDay25()
        {
            // created 25 December 2022
            // https://adventofcode.com/2022/day/25

            Console.WriteLine("--- Day 25: Full of Hot Air ---");

            // data file
            var df = "day25-test.txt";
            //var df = "day25-input.txt";

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






            var answer = 0;
            Console.WriteLine("Day 25 Part 1");
            Console.WriteLine("The Elves are starting to get cold. What SNAFU number");
            Console.WriteLine("do you supply to Bob's console?");
            Console.WriteLine($"{answer}");





            // ---------------------------
            // Part Two
            // ---------------------------
            // TODO
            Console.WriteLine("Day 25 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

