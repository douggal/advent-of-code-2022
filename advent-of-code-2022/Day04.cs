using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day04
    {
        public static void RunDay04()
        {
            // created 4 December 2022
            // https://adventofcode.com/2022/day/4

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- Day 04: Camp Cleanup ---");

            // data file
            var df = "day04-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData", df);
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
            if (input.Count == 0)
            {
                Console.WriteLine("The input file is empty");
            }
            Console.WriteLine($"Input first line: {input.FirstOrDefault("NOT FOUND")}");
            Console.WriteLine($"Input last line: {input.LastOrDefault("NOT FOUND")}");
            Console.WriteLine("---End input file specs---{0}{0}", Environment.NewLine);

            // Timing
            DateTime utcDateStart = DateTime.UtcNow;
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");

            // create and start a Stopwatch instance
            // https://stackoverflow.com/questions/16376191/measuring-code-execution-time
            Stopwatch stopwatch = Stopwatch.StartNew();


            // Part One
            // my first thought is treat each elves section ID list as
            // a sorted set, then use built-in to see if one is a subset of the other
            // could also compare endpoints and see if they fall within range of the other
            var ctr = 0;  // counter for debugging
            var ans1 = 0;  // answer P1 - count of how many sections completely overlap another
            var ans2 = 0;  // answer P1 - count of how many sections completely overlap another
            foreach (var li in input)
            {
                ctr += 1;
                var pair = li.Split(',');
                var e1 = pair[0].Split('-');
                // assuming section IDs are always in order low-high
                var elf1 = (int.Parse(e1[0]), int.Parse(e1[1]));  // tuple (start, end)
                var e2 = pair[1].Split('-');
                var elf2 = (int.Parse(e2[0]), int.Parse(e2[1]));  // tuple (start, end)

                // are elf1's sections completely within elf2's sections?
                // could check endpoints... maybe like this
                // if (elf1.Item1 >= elf2.Item1 && elf1.Item2 <= elf2.Item2) ans += 1;
                // if (elf2.Item1 >= elf1.Item1 && elf2.Item2 <= elf1.Item2) ans += 1;

                // looping over a range c# - for loop or use a Range?
                // https://stackoverflow.com/questions/915745/thoughts-on-foreach-with-enumerable-range-vs-traditional-for-loop
                // in C# a for loop may the better choice for looping over a contiguous range.
                SortedSet<int> a = new();
                var r1 = elf1.Item2 - elf1.Item1 + 1;  // count between starting section and its end
                foreach (int i in Enumerable.Range(elf1.Item1, r1)) a.Add(i);

                SortedSet<int> b = new();
                var r2 = elf2.Item2 - elf2.Item1 + 1;  // count between starting section and its end
                foreach (int i in Enumerable.Range(elf2.Item1, r2)) b.Add(i);

                // Part 1 does either range fall completely within the other?
                if (a.IsSubsetOf(b) || b.IsSubsetOf(a))
                {
                    ans1 += 1;
                    System.Console.WriteLine($"Line {ctr} a sub b {a.IsSubsetOf(b)} or b sub a {b.IsSubsetOf(a)}");
                }

                // Part 2 do either section ranges overlap at all?
                if (a.Overlaps(b) || b.Overlaps(a))
                {
                    ans2 += 1;
                    System.Console.WriteLine($"Line {ctr} a overlaps b {a.IsSubsetOf(b)} or b overlaps a {b.IsSubsetOf(a)}");
                }
            }

            Console.Write('\u2460');
            Console.WriteLine("Day 4 Part 1");
            Console.WriteLine("In how many assignment pairs does one range fully contain the other?");
            Console.WriteLine($"{ans1}\n\n");

            // Part Two
            Console.Write('\u2461');
            Console.WriteLine("Day 4 Part 2");
            Console.WriteLine("In how many assignment pairs does either range overlap the other?");
            Console.WriteLine($"{ans2}\n\n");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
// p1 580 go!
// p2 895 go!
