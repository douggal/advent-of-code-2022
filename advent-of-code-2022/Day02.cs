using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day02
    {
        public static void RunDay02()
        {
            // created 2 December 2022
            // https://adventofcode.com/2022/day/2

            Console.WriteLine("--- Day 02: Rock Paper Scissors ---");

            // read data file
            var df = "Day02-test.txt";
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
            // represent rounds as two parallel lists of char
            // first = List<char> representing 1st column
            // sec = List<char> representing 2nd column
            // l = a line of input
            var a = new List<Char>();
            var b = new List<Char>();
            String? l;
            while(input.Count > 0)
            {
                l = input.Dequeue();
                var s = l.Split(' ');
                a.Add(s[0].Trim().ToCharArray()[0]);
                b.Add(s[1].Trim().ToCharArray()[0]);
            }

            // shape points  A and X = Rock, B and Y = Paper, C and Z = Sissors
            var spts = new Dictionary<Char, int>
            {
                { 'A', 1 }, {'B', 2}, {'C', 3},
                { 'X', 1 }, {'Y', 2}, {'Z', 3}
            };
            // round outcome points  Win, Loss, Draw
            var opts = new Dictionary<Char, int>
            {
                { 'W', 6 }, {'L', 0}, {'D', 3}
            };
            // outcome - who wins?
            // note: value tuple not reference tuple https://stackoverflow.com/questions/955982/tuples-or-arrays-as-dictionary-keys-in-c-sharp
            var oc = new Dictionary<(Char, Char), Char>
            {
                // a = opponent and b = me opposites
                {('X', 'C'), 'W' }, // rock defeats sissors
                {('Z', 'B'), 'W' }, // sissors defeats paper
                {('Y', 'A'), 'W' }  // paper defeats rock
                // same = Draw
                // all else = loss
            };

            var score1 = 0;
            // foreach loop with index https://stackoverflow.com/questions/521687/foreach-with-index
            foreach (var it in b.Select((x, i) => new { Value = x, Index = i }))
            {
                // round score
                var rs = 0;
                //if (it.Index > SomeNumber) //
                // is it a win?
                if (oc.ContainsKey((it.Value, b[it.Index])))
                {
                    // score for shape selected + outcome score
                    rs = spts[it.Value] + opts['W'];
                }
                else if (it.Value == b[it.Index])  // is it a draw?
                {
                    rs = spts[it.Value] + opts['D'];
                }
                else // loss
                {
                    rs = spts[it.Value] + opts['L'];
                }
                score1 += rs;
            }

            Console.WriteLine("Day 2 Part 1 total score be if everything goes exactly according to your strategy guide?");
            Console.WriteLine($"{score1}");

            // Part Two
            // TODO
            Console.WriteLine("Day 02 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

