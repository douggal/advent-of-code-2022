using System;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace advent_of_code_2022
{
    public static class Day06
    {
        public static void RunDay06()
        {
            // created 6 December 2022
            // https://adventofcode.com/2022/day/6

            Console.WriteLine("--- Day 06: Tuning Trouble ---");

            // data file
            //var df = "day06-test.txt";
            var df = "day06-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
            var input = new List<String>();
            String? line;
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fn, Encoding.UTF8))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //var items = line.Trim().Split(',').ToList();
                        input.Add(line);
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
            // first thought is implement a sliding window
            var bs = 3;  // start-of-packet marker size in chars - 1

            // ins = input data stream
            var ins = input.First().ToCharArray();

            var a = FindStartOfPacket(ins, bs);

            Console.WriteLine("Day 6 Part 1");
            Console.WriteLine("How many characters need to be processed before the first start-of-packet marker is detected?");
            Console.WriteLine($"{a}\n\n");

            // Part Two
            bs = 13;  // start-of-packet marker size in chars - 1

            var a2 = FindStartOfPacket(ins, bs);

            Console.WriteLine("Day 6 Part 2");
            Console.WriteLine("How many characters need to be processed before the first start-of-message marker is detected?");
            Console.WriteLine($"{a2}\n\n");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        public static int FindStartOfPacket(char[] ins, int bs)
        {
            var a = 0;
            var buff = new Queue<char>(ins.Take(bs).ToList());

            // start sliding along begining at buff size (smallest chunk possible)
            // until 4 unique chars are found
            // Skip(buff size) is fine, but I need the index of each char too
            // a for loop might be clearer.
            foreach (var e in ins.Skip(bs).Select((x, i) => new { Value = x, Index = i }))
            {
                // add character
                buff.Enqueue(e.Value);
                // helper to check if all chars differ
                HashSet<char> c = new HashSet<char>(buff);

                bool check = buff.Count == c.Count ? true : false;
                if (check)
                {
                    a = e.Index + bs + 1; // account for 0 based indexing and buff size
                    break;
                }
                else
                {
                    // not start of packet/message, discard head and try again
                    buff.Dequeue();
                }
            }

            return a;
        }
    }
}
// 1140
// 3495
