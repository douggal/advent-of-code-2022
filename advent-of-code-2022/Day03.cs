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
            // li = a line of input

            // pr = priority which is each char's index + 1
            // TODO: what about encoding? UTF-8?
            var pr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();

            var sp = 0;  // sum of the priorities
            var ctr = 0; // a counter for debugging output
            foreach (var li in input)  // don't deque need queue for part 2
            {
                ctr += 1;
                // ll = line length; hll = one-half the line length
                var ll = li.Length;
                var hll = li.Length / 2;
                HashSet<Char> a = new HashSet<char>(li.Substring(0,hll));
                HashSet<Char> i = new HashSet<char>(li.Substring(hll));
                i.IntersectWith(a);

                // assert i should have only 1 item
                //Console.WriteLine($"Line {ctr} Assert {i.Count} (should be 1 char), Char is {String.Join("",i)}");

                // find priority
                if (i.Count > 0)
                {
                    var c = i.First();
                    var p = pr.IndexOf(c) + 1;
                    sp += p;
                }
                else
                {
                    Console.WriteLine($"Error no item found for line {ctr} (should be one).");
                }

            };


            Console.WriteLine("Day 3 Part 1");
            Console.WriteLine("Find the item type that appears in both compartments of each rucksack.");
            Console.WriteLine("What is the sum of the priorities of those item types?");
            Console.WriteLine($"{sp}\n\n");


            // Part Two
            var sp2 = 0;  // sum of priorities
            ctr = 0;
            while (input.Count > 0)
            {
                ctr += 1;
                HashSet<Char> a = new HashSet<char>(input.Dequeue());
                HashSet<Char> b = new HashSet<char>(input.Dequeue());
                HashSet<Char> c = new HashSet<char>(input.Dequeue());

                IEnumerable<char> i = a.Intersect(b).Intersect(c);
                // assert i should have only 1 item
                //Console.WriteLine($"Line {ctr} Assert {i.Count()} (should be 1 char), Char is {String.Join("",i)}");

                // find priority
                if (i.Count() == 1)
                {
                    var p = pr.IndexOf(i.First()) + 1;
                    sp2 += p;
                }
                else
                {
                    Console.WriteLine($"Error no or more than one item found for line {ctr} (should be one).");
                }
            }

            // Assert:  there should be 100 groups ctr = 100;
            if (ctr != 100) Console.WriteLine("Error groups are wrong.");

            Console.WriteLine("Day 3 Part 2");
            Console.WriteLine("Find the item type that corresponds to the badges of each three-Elf group.");
            Console.WriteLine("What is the sum of the priorities of those item types?");
            Console.WriteLine($"{sp2}\n\n");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
// 266800 is too high
