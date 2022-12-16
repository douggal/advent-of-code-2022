﻿using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace advent_of_code_2022
{
    public static class Day15
    {
        public static void RunDay15()
        {
            // created 15 December 2022
            // https://adventofcode.com/2022/day/15

            Console.WriteLine("--- Day 15: Beacon Exclusion Zone ---");

            // data file
            var df = "day15-test.txt";
            //var df = "day15-input.txt";

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

            // Line of Interest
            var LOI = 10;

            // List of x values of ccords on Line of Interest which are covered by a sensor
            List<int> cannotContain = new();

            // List of sensors
            // Tuple (p1, p2, m) = index is sensor ID, p1 = sensor loc, p2 = beacon loc, m = distance
            var sl = new List<((int, int), (int, int), int)>();

            // split string on regex
            // Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            string pat = @".*(Sensor at x=)([-]{0,1}\d+), y=([-]{0,1}\d+): closest beacon is at x=([-]{0,1}\d+), y=([-]{0,1}\d+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            while (input.Count > 0)
            {
                //Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                var li = input.Dequeue().Trim();

                // Match the regular expression pattern against a text string.
                Match m = r.Match(li);

                // find manhattan distance for each coord.
                var p1 = (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
                var p2 = (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

                var man = int.Abs(p1.Item1 - p2.Item1) + int.Abs(p1.Item2 - p2.Item2);
                sl.Add((p1, p2, man));
            }

            // Eliminate any points whose Manhattan dist to beacon puts them
            // completely above (Y axis) or below the Line of Interest
            // Check only those sensors that overlap LOI
            var slToCheck = new List<int>();
            foreach (var s in sl.Select((value, i) => new {i, value}))
            {
                var bottomY = s.value.Item1.Item2 + s.value.Item3;  // Y + manhattan
                var topY = s.value.Item1.Item2 - s.value.Item3;   // Y - manhattan

                if (bottomY >= LOI && topY <= LOI)
                {
                    slToCheck.Add(s.i);
                }
            }

            // Find all the points in each sensor-beacon pairing that
            // overlap / touch the LOI.
            foreach (var i in slToCheck)
            {
                var s = sl[i];
                var bottomY = s.Item1.Item2 + s.Item3;  // Y + manhattan (highest Y value)
                var topY = s.Item1.Item2 - s.Item3;   // Y - manhattan (lowest Y value)

                // TODO: sweep line down from top to bottom (low to high Y)
                // if coord(s) fall on LOI, save them in List
                for (int j = topY; j <= bottomY; j++)
                {
                    if (j == LOI)
                    {
                        // count points overlapping LOI

                    }
                }
            }
            var z = 0;


            var answer = 0;
            Console.WriteLine("Day 15 Part 1");
            Console.WriteLine("Consult the report from the sensors you just deployed.");
            Console.WriteLine("In the row where y=2000000, how many positions cannot contain a beacon?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two
            // TODO
            Console.WriteLine("Day 15 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

