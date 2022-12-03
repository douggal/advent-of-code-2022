using System;
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
            var df = "day03-input.txt";

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
                Console.WriteLine($"Input first line: {input.First()}");
                Console.WriteLine($"Input last line: {input.Last()}");
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
            // first thought is us a Set and take the intersection
            // text says each line is an even number of chars long
            // let set a be 1st half / 1st compartment
            // let set b be the 2nd half / 2nd compartment
            // l = a line of input
            // sp = sum of the priorities

            // pr = priority which is each char's index + 1
            var pr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

            var sp = 0;
            var l = String.Empty;
            while (input.Count > 0)
            {
                line = input.Dequeue();
                // ll line length; hll one-half the line length
                var ll = line.Length;
                var hll = line.Length / 2;
                HashSet<Char> a = new HashSet<char>(line.Substring(0,hll-1));
                HashSet<Char> i = new HashSet<char>(line.Substring(hll, hll));
                i.IntersectWith(a);

                // assert i should have only 1 item
                Console.WriteLine($"Assert {i.Count} (should be 1 char), Char is {String.Join("",i)}");

                // find priority
                var c = i.First();
                var p = pr.IndexOf(c) + 1;
                sp += p;

            };


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

