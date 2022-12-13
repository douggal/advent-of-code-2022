using System;
using System.Text;
using System.Diagnostics;
using QuikGraph;

namespace advent_of_code_2022
{
    public static class Day12
    {
        public static void RunDay12()
        {
            // created 12 December 2022
            // https://adventofcode.com/2022/day/12

            Console.WriteLine("--- Day 12: Hill Climbing Algorithm ---");

            // data file
            var df = "day12-test.txt";
            //var df = "day12-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
            var input = new Queue<String>();
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
            if (input.Count == 0)
            {
                Console.WriteLine("The input file is empty");
            }
            Console.WriteLine($"Input first line: {input.FirstOrDefault("NOT FOUND")}");
            Console.WriteLine($"Input last line: {input.LastOrDefault("NOT FOUND")}");
            Console.WriteLine("---End input file specs---{0}{0}", Environment.NewLine);

            // Timing
            DateTime utcDateStart = DateTime.UtcNow;
            Stopwatch stopwatch = Stopwatch.StartNew();  // start stopwatch
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");


            // Part One

            //Model the data as a graph and use QuikGraph to solve for shortest path.

            // First, create a dictionary of vertices (keys) and edges (values)
            // from the input data.  Use a 2x array as step inbetween
            // because it's easier to find the edges of each vertex that way
            var nCols = input.First().Length;
            var nRows = input.Count;

            string[,] temp = new string[nRows, nCols];

            var col = 0;
            while (input.Count > 0)
            {
                var li = input.Dequeue()
                    .Trim()
                    .ToCharArray()
                    .Select(x => x.ToString())
                    .ToList();
                foreach (var v in li.Select( (v, i) => new {vertex = v, idx = i}))
                {
                    temp[col,v.idx] = v.vertex;
                }
                col++;
            }

            var i = 0;

            Console.WriteLine("Day 12 Part 1");


            // Part Two
            // TODO
            Console.WriteLine("Day 12 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

