using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day18
    {
        public static void RunDay18()
        {
            // created 18 December 2022
            // https://adventofcode.com/2022/day/18

            Console.WriteLine("--- Day 18: Boiling Boulders ---");

            // data file
            var df = "day18-test.txt";
            //var df = "day18-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
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

            // a cube has 6 sides
            var nbrSides = 6;

            // Each cube is represented as a tuple3 (x, y, z)
            // Add the cubes one a time and accumulate them in a List

            List<(int, int, int)> grid = new();
            var exposedSides = 0;

            while (input.Count > 0)
            {
                // get a new cube, nc
                var li = input.Dequeue().Split(',').ToArray();
                var nc = ( int.Parse(li[0]), int.Parse(li[1]), int.Parse(li[2]) );

                // each cube adds 6 sides minus 2 sides for each cube it abuts
                var subtractThisMany = 0;
                foreach (var cube in grid)
                {
                    if ((nc.Item1 == cube.Item1 && nc.Item2 == cube.Item2)
                        || (nc.Item1 == cube.Item1 && nc.Item3 == cube.Item3)
                        || (nc.Item2 == cube.Item2 && nc.Item3 == cube.Item3))
                    {
                        subtractThisMany += 2;
                    }
                }
                grid.Add(nc);
                exposedSides += (nbrSides - subtractThisMany);
            }

            Console.WriteLine("Day 18 Part 1");
            System.Console.WriteLine("What is the surface area of your scanned lava droplet?");
            System.Console.WriteLine($"{exposedSides}\n\n");


            // Part Two
            // TODO
            Console.WriteLine("Day 18 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

