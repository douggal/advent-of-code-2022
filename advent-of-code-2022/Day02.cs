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

            // data file
            var df = "Day02-test.txt";

            // read in data file
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
                if (!String.IsNullOrWhiteSpace(l)) {
                    var s = l.Split(' ');
                    a.Add(s[0].Trim().ToCharArray()[0]);
                    b.Add(s[1].Trim().ToCharArray()[0]);
                };
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
            // for each value i throw (list b) compare with my opponents value in list a
            // foreach loop with index https://stackoverflow.com/questions/521687/foreach-with-index
            foreach (var it in b.Select((x, i) => new { Value = x, Index = i }))
            {
                // round score
                var rs = 0;
                //if (it.Index > SomeNumber) //
                // is it a win?
                if (oc.ContainsKey((it.Value, a[it.Index])))
                {
                    // score for shape selected + outcome score
                    rs = spts[it.Value] + opts['W'];
                }
                else if (spts[it.Value] == spts[a[it.Index]])  // is it a draw? i.e., both same item = compare point values
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
            // 2nd column indicates how the round needs to end
            // X = I need to Lose, Y = I need to Draw, Z = I need to Win
            // not very much different, in part 1 I had my throw and could comapre with my opponent
            // in Part 2 I know the outocme and my opponent's throw and need to find my throw to compute the round's score
            // my = my throw given outcome and my opponents throw
            var my = new Dictionary<(Char, Char), Char>
            {
                // a = opponent and b = me opposites
                {('A','Y'),'X'},
                {('A','X'),'Z'},
                {('A','Z'),'Y'},
                {('B','Y'),'Y'},
                {('B','X'),'X'},
                {('B','Z'),'Z'},
                {('C','Y'),'Z'},
                {('C','X'),'Y'},
                {('C','Z'),'X'}
            };
            var score2 = 0;
            foreach (var it in a.Select((x, i) => new { Value = x, Index = i }))
            {
                // round score
                var rs = 0;

                // what do I need to throw?
                var myThrow = my[(it.Value, b[it.Index])];
                // win?
                if (oc.ContainsKey((myThrow, a[it.Index])))
                {
                    // score for shape selected + outcome score
                    rs = spts[myThrow] + opts['W'];
                }
                else if (spts[myThrow] == spts[a[it.Index]])  // is it a draw? i.e., both same item = compare point values
                {
                    rs = spts[myThrow] + opts['D'];
                }
                else // loss
                {
                    rs = spts[myThrow] + opts['L'];
                }
                score2 += rs;
            }
            Console.WriteLine("Day 2 Part 2  what would your total score be if everything goes exactly according to your strategy guide?");
            Console.WriteLine($"{score2}");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
// 12153 is too low
// 12645