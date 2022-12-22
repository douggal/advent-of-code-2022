using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day20
    {
        public static void RunDay20()
        {
            // created 20 December 2022
            // https://adventofcode.com/2022/day/20

            Console.WriteLine("--- Day 20: Grove Positioning System ---");

            // data file
            var df = "day20-test.txt";
            //var df = "day20-input.txt";

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

            // Considered the LinkedList<T> type but discarded in favor of List<T>
            // and its InsertAt() method.

            // original = original input
            // cypher - working copy
            List<int> original = new();
            List<int> cypher = new();

            while (input.Count > 0)
            {
                var aLine = input.Dequeue().Trim();

                // one number per line
                original.Add(int.Parse(aLine));
                cypher.Add(int.Parse(aLine));
            }

            // Find the grove coords by running the mixing process

            // List is circular - items moved from head or tail appear at other end

            foreach (var n in original)
            {
                // Ugly, but I could not get the backwards count to work right
                // any other way
                if (n < 0)
                    cypher.Reverse();

                // move the n-th item in cypher n units ahead or back
                var oldIndex = cypher.FindIndex(x => x == n);

                // don't do anything if item is a zero
                if (n != 0)
                {
                    // calculate new index (once the old item
                    // is removed the newIndex will have correct insertion point)
                    var newIndex = (oldIndex + int.Abs(n)) % cypher.Count;

                    // Drop from old position
                    cypher.RemoveAt(oldIndex);

                    // insert n-th item at newIndex
                    // the Insert() method pushes items out of the way
                    // to make room for the insert.
                    if (newIndex == 0)
                        // insert before the head of the list, ie., at end of the list
                        cypher.Add(n);
                    else if (newIndex == cypher.Count)
                        // insert at beginning of the list
                        cypher.Insert(0, n);
                    else if (oldIndex + int.Abs(n) > cypher.Count)
                        // account for zero based List
                        cypher.Insert(newIndex + 1, n);
                    else
                        cypher.Insert(newIndex, n);
                }

                if (n < 0)
                    cypher.Reverse();
            }

            // the grove coordinates can be found by looking at the 1000th, 2000th,
            // and 3000th numbers after the value 0, wrapping around the list as necessary

            var indexofZero = cypher.FindIndex(x => x == 0);
            var a = cypher[(1000 % original.Count + cypher.FindIndex(x => x == 0)) % cypher.Count];
            var b = cypher[(2000 % original.Count + cypher.FindIndex(x => x == 0)) % cypher.Count];
            var c = cypher[(3000 % original.Count + cypher.FindIndex(x => x == 0)) % cypher.Count];

            var answer = a + b + c;

            Console.WriteLine("Day 20 Part 1");
            Console.WriteLine("Mix your encrypted file exactly once.");
            Console.WriteLine("What is the sum of the three numbers that form the grove coordinates?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two
            // TODO
            Console.WriteLine("Day 20 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
// -188 no
// 17490 too high

