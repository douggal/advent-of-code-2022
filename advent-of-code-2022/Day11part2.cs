using System;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Numerics;


// Day 11 Part 2 - try re-scaling the numbers

// as of 23 Dec - no go

namespace advent_of_code_2022
{
    public static class Day11part2
    {
        public static void RunDay11part2()
        {
            // created 23 December
            // https://adventofcode.com/2022/day/11

            Console.WriteLine("--- Day 11: Monkey in the Middle ---");

            // data file
            var df = "day11-test.txt";
            //var df = "day11-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData", df);
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
                    var si = li.Substring("Starting items: ".Length - 1);
                    var s1 = si.Split(',');
                    var s2 = s1.Select(x => int.Parse(x.Trim())).ToArray();
                    monkeys[n].Items = new Queue<BigInteger>();
                    foreach (var s in s2)
                        monkeys[n].Items.Enqueue((long)s);

                    // third row operation:  "  Operation: new = old * 19"
                    var li3 = input.Dequeue().Trim().Split(' ');
                    if (li3 is [.., "Operation:", "new", "=", "old", "*", "old"])
                    {
                        monkeys[n].Operation = Tuple.Create("~", 0);
                    }
                    else if (li3 is [.., "Operation:", "new", "=", "old", var o, var v])
                    {
                        monkeys[n].Operation = Tuple.Create(o, int.Parse(v));
                    }

                    // fourth row test:  "  Test: divisible by 23"
                    li = input.Dequeue().Trim();
                    var tstr = li.Substring("Test: divisible by ".Length - 1);
                    monkeys[n].TestValue = (long)int.Parse(tstr);

                    // fifth row test if true:  "    If true: throw to monkey 1"
                    li = input.Dequeue().Trim();
                    var ttt = li.Substring("If true: throw to monkey ".Length - 1).Trim();
                    monkeys[n].TestIsTrueMonkey = int.Parse(ttt);

                    // sixth row test if false:  "    If false: throw to monkey 3"
                    li = input.Dequeue().Trim();
                    var ttf = li.Substring("If false: throw to monkey ".Length - 1).Trim();
                    monkeys[n].TestIsFalseMonkey = int.Parse(ttf);
                }
            }

            // Play 20 rounds of Keep Away with worry level divisor of 3
            //PlayKeepAway(monkeys, 20, 3);

            // Play 10000 rounds of Keep Away with worry level divisor of 1
            PlayKeepAway(monkeys, 10000, 1);


            /* answer
             * focus on the two most active monkeys if you want any hope of getting 
             * your stuff back. Count the total number of times each monkey inspects 
             * items over 20 rounds.
             * the two most active monkeys inspected items 101 and 105 times. 
             * The level of monkey business in this situation can be found by multiplying these together
             */

            var mostActive = monkeys.Select((v, i) => new { index = i, value = v.InspectedItemsCount }).ToList();
            mostActive.Sort((x, y) => y.value.CompareTo(x.value));  // descending!

            var answer = monkeys[mostActive[0].index].InspectedItemsCount * monkeys[mostActive[1].index].InspectedItemsCount;

            //Console.WriteLine("Day 11 Part 1");
            //Console.WriteLine("What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?");
            //Console.WriteLine($"{answer}\n\n");


            // ---------------------------
            // Part Two
            // ---------------------------
            Console.WriteLine("Day 11 Part 2");
            Console.WriteLine("Worry levels are no longer divided by three after each item is");
            Console.WriteLine("inspected; you'll need to find another way to keep your worry levels");
            Console.WriteLine("manageable. Starting again from the initial state in your puzzle input,");
            Console.WriteLine("what is the level of monkey business after 10000 rounds?");
            Console.WriteLine($"{answer}\n\n");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        private static void PlayKeepAway(List<Monkey> monkeys, int rounds, long worryLevelDivisor)
        {
            // rounds = rounds to play
            // wld = worry level divisor
            BigInteger wld2 = worryLevelDivisor;

            foreach (var rnd in Enumerable.Range(0, rounds))
            {
                Console.WriteLine($"Round {rnd}");
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        var item = monkey.Items.Dequeue();

                        // monkey inspects
                        BigInteger newWorryLevel = monkey.DoOperation(item);
                        monkey.InspectedItemsCount += 1L;

                        // Part 1:  monkey gets bored
                        // floored integer division
                        // Ref: https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger.divrem?view=net-7.0
                        BigInteger remainder;
                        var nextwWorryLevel = BigInteger.DivRem(newWorryLevel, wld2, out remainder);

                        // Part 2:  monkey does not get bored
                        // Note: had to comment out the division done for Part 1
                        // Using a divisor of 1 did not help.
                        // Division did not work as expected with really big numbers
                        //var nextwWorryLevel = newWorryLevel;

                        // monkey throws away
                        var next = monkey.Test(nextwWorryLevel);
                        monkeys[next].Items.Enqueue(nextwWorryLevel);
                    }
                }

                // rescale worry level numbers for each monkey
                Console.WriteLine($"Rescaling worry levels {rnd}");
                foreach (var monkey in monkeys)
                {
                    if (monkey.Items.Count > 0)
                        monkey.rescale(57,98);
                }
            }
        }

        private class Monkey
        {
            public string Name { get; set; }
            public Queue<BigInteger> Items { get; set; }
            public long TestValue { get; set; }
            public Tuple<string, int> Operation { get; set; }
            public int TestIsTrueMonkey { get; set; }
            public int TestIsFalseMonkey { get; set; }
            public BigInteger InspectedItemsCount { get; set; }

            public Monkey(int m)
            {
                Name = string.Format("Monkey {0}", m);
                Items = new Queue<BigInteger>();
                InspectedItemsCount = 0L;
            }

            public BigInteger DoOperation(BigInteger worryLevel)
            {
                BigInteger ret = 0;
                switch (Operation.Item1)
                {
                    case "+":
                        ret = BigInteger.Add(worryLevel, Operation.Item2);
                        break;
                    case "*":
                        ret = BigInteger.Multiply(worryLevel, Operation.Item2);
                        break;
                    case "~":
                        ret = BigInteger.Multiply(worryLevel, worryLevel);
                        break;
                    default:
                        break;
                }
                return ret;
            }

            public int Test(BigInteger worryLevel)
            {

                //Test: divisible by 17
                //If true: throw to monkey 0
                //If false: throw to monkey 1

                BigInteger remainder;
                var nextwWorryLevel = BigInteger.DivRem(worryLevel, TestValue, out remainder);
                if (remainder == 0)
                    return TestIsTrueMonkey;
                else
                    return TestIsFalseMonkey;
            }

            internal void rescale(int newmin, int newmax)
            {
                // Rescale:
                // https://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
                // https://stackoverflow.com/questions/5294955/how-to-scale-down-a-range-of-numbers-with-a-known-min-and-max-value

                var tmp = Items.Select(x => x).ToList();
                var oldmin = tmp.Min();
                var oldmax = tmp.Max();
                Items.Clear();

                BigInteger newvalue = 0;
                var oldrange = (oldmax - oldmin);
                var newrange = newmax - newmin;
                foreach (var oldvalue in tmp)
                {
                    if (oldrange == 0)
                    {
                        newvalue = newmin;
                    }
                    else
                    {
                        newvalue = BigInteger.Divide(((oldvalue - oldmin) * newrange), oldrange) + newmin;
                    }
                    Items.Enqueue(newvalue);

                }
            }
        }
    }
}


// 56120 go!
// 2 713 310 158 test data
// 2 670 460 610 no
