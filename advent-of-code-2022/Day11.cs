using System;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace advent_of_code_2022
{
    public static class Day11
    {
        public static void RunDay11()
        {
            // created 22 December
            // https://adventofcode.com/2022/day/11

            Console.WriteLine("--- Day 11: Monkey in the Middle ---");

            // data file
            var df = "day11-test.txt";
            //var df = "day11-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData" ,df);
            var input = new Queue<string>();
            string? line;
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

            // ---------------------------
            // Part One
            // ---------------------------

            // Each monkey is known by a number. Create a List<Monkey>
            List<Monkey> monkeys = new();

            while (input.Count > 0)
            {
                var li = input.Dequeue().Trim();

                if (li.Trim() != string.Empty)
                {
                    // first row monkey number:  "Monkey 0:"
                    // the input appears to be regular and in order
                    Regex mn = new Regex(@"^Monkey (\d+):$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var m = mn.Matches(li).ToArray();
                    var n = int.Parse(m[0].Groups[1].Value);
                    monkeys.Add(new Monkey(n));

                    // second row starting items:  "  Starting items: 79, 98"
                    li = input.Dequeue().Trim();
                    var si = li.Skip(18).ToString();
                    var s = si.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                    monkeys[n].Items = s;

                    // third row operation:  "  Operation: new = old * 19"
                    li = input.Dequeue().Trim();
                    Regex op = new Regex(@"^\s+Operation: new = old ([\+\*]) (\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var o = op.Matches(li).ToArray();
                    var operation = m[0].Groups[1].Value.Trim();
                    var val = int.Parse(m[0].Groups[2].Value);
                    monkeys[n].Operation = Tuple.Create(operation, val);

                    // fourth row test:  "  Test: divisible by 23"
                    li = input.Dequeue().Trim();
                    var tstr = li.Skip(21).ToString();
                    monkeys[n].TestValue = int.Parse(tstr);

                    // fifth row test if true:  "    If true: throw to monkey 1"
                    li = input.Dequeue().Trim();
                    var ttt = li.Skip(29).ToString();
                    monkeys[n].TestIsTrueMonkey = int.Parse(ttt);

                    // sixth row test if false:  "    If false: throw to monkey 3"
                    li = input.Dequeue().Trim();
                    var ttf = li.Skip(30).ToString();
                    monkeys[n].TestIsTrueMonkey = int.Parse(ttf);
                }
            }

            /*
             * Monkey inspects an item with a worry level of 79.
                Worry level is multiplied by 19 to 1501.
                Monkey gets bored with item. Worry level is divided by 3 to 500.
                Current worry level is not divisible by 23.
                Item with worry level 500 is thrown to monkey 3.
            */

            // Play 20 rounds of Keep Away
            foreach (var rnd in Enumerable.Range(0,20))
            {
                foreach (var mmonkey in monkeys)
                {
                    foreach (var item in mmonkey.Items)
                    {
                       // var newWorryLevel = item * mmonkey.Operation;
                    }
                }

            }

            var answer = 0;
            Console.WriteLine("Day 11 Part 1");
            Console.WriteLine("What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?");
            Console.WriteLine($"{answer}");


            // ---------------------------
            // Part Two
            // ---------------------------
            // TODO
            Console.WriteLine("Day 11 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        private class Monkey
        {
            public string Name { get; set; }
            public List<int> Items { get; set; }
            public int TestValue { get; set; }
            public Tuple<string, int> Operation { get; set; }
            public int TestIsTrueMonkey { get; set; }
            public int TestIsFalseMonkey { get; set; }

            public Monkey(int m)
            {
                // todo
                Name = string.Format("Monkey {0}",m);
                Items = new List<int>();
        }

            public int Test()
            {

                //Test: divisible by 17
                //If true: throw to monkey 0
                //If false: throw to monkey 1
                return 0;
            }

        }
    }



}

