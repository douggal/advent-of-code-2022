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
                var aLine =  input.Dequeue().Trim();

                // one number per line
                original.Add(int.Parse(aLine));
                cypher.Add(int.Parse(aLine));
            }

            // Find the grove coords by running the mixing process

            // List is circular - items moved from head or tail appear at other end
            var N = original.Count;

            foreach (var n in original)
            {
                // move the n-th item in cypher n units ahead or back
                var currIndex = cypher.FindIndex(x => x == n) + 1;
                var newIndex = (currIndex + n) % N;

                // insert n-th item at newIndex
                // the Insert() method pushes items out of the way
                // to make room for the insert.
                cypher.Insert(newIndex, n);

                // clean up - have to take item back out of the list
                // if new insert was before old item, then the old item
                // now 1 unit ahead of where it was.
                if (newIndex < currIndex)
                    cypher.RemoveAt(currIndex-1);
                else
                    cypher.RemoveAt(currIndex);
            }

            // the grove coordinates can be found by looking at the 1000th, 2000th,
            // and 3000th numbers after the value 0, wrapping around the list as necessary

            var indexofZero = cypher.FindIndex(x => x == 0);
            var a = 1000 % N + indexofZero;
            var b = 2000 % N + indexofZero;
            var c = 3000 % N + indexofZero;

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

