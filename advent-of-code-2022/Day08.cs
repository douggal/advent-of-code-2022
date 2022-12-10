using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace advent_of_code_2022
{
    public static class Day08
    {
        public static void RunDay08()
        {
            // created 9 December 2022
            // https://adventofcode.com/2022/day/8

            Console.WriteLine("--- Day 08: Treetop Tree House ---");

            // data file
            var df = "day08-test.txt";
            //var df = "day08-input.txt";

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
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");


            // Part One

            // Model the patch of trees as a single List of trees
            // An offset, o, identifies which column a tree is in
            // A tree has a height property and a visible true/false property

            List<Tree> treePatch = new();
            var offset = input.First().Length;

            while (input.Count > 0)
            {
                var li = input.Dequeue()
                    .Trim()
                    .ToCharArray()
                    .Select(x => int.Parse(x.ToString()));
                foreach (var t in li)
                {
                    treePatch.Add(new Tree(t));
                }
            }

            // debug: print out the patch of trees on the console
            // Note: Linq features lazy evaluation - doesn't do anything until ToList etc is
            // called forcing an evaluation. Hence I first split up the treePatch,
            var tmp = treePatch.Select((value, index) => new { Index = index, Value = value })
                            .GroupBy(i => i.Index / offset)
                            .Select(i => i.Select(i2 => i2.Value));
            // and then interate over the IEnumerable<IEnumerable<Tree>> calling ToList to force
            // evaluation of each row in order for whole list to be sent to String.Join.
            foreach (var t in tmp)
            {
                Console.WriteLine(String.Join(", ",t.Select(y => y.Height).ToList()));
            }
            Console.WriteLine();



            var answer = 0; 
            Console.WriteLine("Day 8 Part 1");
            System.Console.WriteLine("Consider your map; how many trees are visible from outside the grid?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two
            // TODO
            Console.WriteLine("Day 8 Part 2");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }

    public class Tree
    {
        public int Height { get; set; }
        public bool? Visible { get; set; }
        public Tree(int h)
        {
            Height = h;
            Visible = null;
        }
    }
}

