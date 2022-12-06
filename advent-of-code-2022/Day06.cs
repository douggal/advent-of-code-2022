using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day06
    {
        public static void RunDay06()
        {
            // created 6 December 2022
            // https://adventofcode.com/2022/day/6

            Console.WriteLine("--- Day 05: Tuning Trouble ---");

            // data file
            //var df = "day06-test.txt";
            var df = "day06-input.txt";

            // read in data
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
            // a = answer;
            var a =0;

            // b = buffer
            var b = input.Dequeue().ToCharArray();
            // create and inialize a check buffer
            var b4 = new Queue<char>( b.Take(3).ToList() );
            // start sliding along until 4 unique chars are found
            for (int i = 3; i < b.Length; i++)
            {
                b4.Enqueue(b[i]);
                // check if all 4 chars differ
                HashSet<char> c = new HashSet<char>(b4.ToList());
                bool check = (b4.Count == c.Count ? true : false); 
                if (check) 
                {
                    a = i + 1; // account for 0 based indexing
                    break;
                }
                else
                {
                    b4.Dequeue();
                }
            }

            Console.WriteLine("Day 6 Part 1");
            Console.WriteLine("How many characters need to be processed before the first start-of-packet marker is detected?");
            Console.WriteLine($"{a}\n\n");

            // Part Two
            // TODO
            Console.WriteLine("Day 6 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

