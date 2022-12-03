﻿using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day03
    {
        public static void RunDay03()
        {
            // created 3 December 2022
            // https://adventofcode.com/2022/day/3

            Console.WriteLine("--- Day 03: Rucksack Reorganization ---");

            // data file
            var df = "day03-test.txt";

            // read in the data
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

            // sp = sum of the priorities
            var sp = 0;

            Console.WriteLine("Day 3 Part 1");
            Console.WriteLine("Find the item type that appears in both compartments of each rucksack.");
            Console.WriteLine("What is the sum of the priorities of those item types?");
            Console.WriteLine($"{sp}\n\n");


            // Part Two
            // TODO
            Console.WriteLine("Day 3 Part 2  [TBD]");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
