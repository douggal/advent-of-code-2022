using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day09
    {
        public static void RunDay09()
        {
            // created 10 December 2022
            // https://adventofcode.com/2022/day/9

            Console.WriteLine("--- Day 09: Rope Bridge ---");

            // data file
            var df = "day09-test.txt";
            //var df = "day09-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData", df);
            var input = new Queue<String>();
            String? line;
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


            // Part One

            // Read in series of motions and store them in a List of value tuples
            List<(string, int)> motions = new();

            while (input.Count > 0)
            {
                var li = input.Dequeue().Trim().Split(' ');
                motions.Add(  (li[0], int.Parse(li[1]))  );
            }
            //foreach (var t in motions) Console.WriteLine(String.Join(',',t));

            var answer = 0;

            Console.WriteLine("Day 9 Part 1");
            Console.WriteLine("Simulate your complete hypothetical series of motions");
            Console.WriteLine("How many positions does the tail of the rope visit at least once?");
            Console.WriteLine($"{answer}");


            // Part Two
            // TODO
            Console.WriteLine("Day 9 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

