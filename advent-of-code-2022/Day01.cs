using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day01
    {
        public static void RunDay01()
        {
            // created 1 Dec 2022
            // https://adventofcode.com/2022/day/1

            Console.WriteLine("--- Day 1: Calorie Counting ---");

            // read data file
            var df = "day01-input.txt";
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
            var input = new Queue<String>();
            String? line;
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fn))
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
            if (input.Count > 0)
            {
                Console.WriteLine($"Input first line: {input.FirstOrDefault()}");
                Console.WriteLine($"Input last line: {input.LastOrDefault()}");
            }
            else
            {
                Console.WriteLine("The input file is empty");
            }
            Console.WriteLine("---End input file specs---{0}{0}", Environment.NewLine);

            // Timing
            DateTime utcDateStart = DateTime.UtcNow;
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");
        
            // create and start a Stopwatch instance
            // https://stackoverflow.com/questions/16376191/measuring-code-execution-time
            Stopwatch stopwatch = Stopwatch.StartNew();


            // Part One
            // build iv, a input vector of int of total Calories carried by each elf
            var iv = new List<int>();
            var t = 0;
            while(input.Count > 0)
            {
                var s = input.Dequeue();
                if (string.IsNullOrEmpty(s))
                {
                    // done adding Calories given elf
                    iv.Add(t);
                    t = 0;
                }
                else
                {
                    // add to total for this elf;
                    t += int.Parse(s.Trim());
                }
                // don't forget to include the last line if not blank!
                if (input.Count == 0 && !string.IsNullOrEmpty(s)) iv.Add(t);
            }

            var most = iv.Max();
            Console.WriteLine($"Day 1 Part 1 Most Calories carried by any one elf is {most}.");


            // Part Two
            iv.Sort((x, y) => y.CompareTo(x));  // descending
            var hi3 = iv.Take(3).Sum();

            Console.WriteLine("Day 1 Part 2 Find the top three Elves carrying the most Calories.");
            Console.WriteLine($"How many Calories are those Elves carrying in total?  {hi3}");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            Console.ReadKey();
        }
    }
}

