using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace advent_of_code_2022
{
    public static class Day15
    {
        public static void RunDay15()
        {
            // created 15 December 2022
            // https://adventofcode.com/2022/day/15

            Console.WriteLine("--- Day 15: Beacon Exclusion Zone ---");

            // data file
            //var df = "day15-test.txt";
            var df = "day15-input.txt";

            // Line of Interest
            //var LOI = 10;
            var LOI = 2000000;

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
            if (input.Count == 0)
            {
                Console.WriteLine("The input file is empty");
            }
            Console.WriteLine($"Input first line: {input.FirstOrDefault("NOT FOUND")}");
            Console.WriteLine($"Input last line: {input.LastOrDefault("NOT FOUND")}");
            Console.WriteLine("---End input file specs---{0}{0}", Environment.NewLine);

            // Timing
            DateTime utcDateStart = DateTime.UtcNow;
            Stopwatch stopwatch = Stopwatch.StartNew();  // start stopwatch
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");


            // Part One

            // List of x values of ccords on Line of Interest which are covered by a sensor
            HashSet<int> cannotContain = new();

            // List of sensors
            // Tuple (p1, p2, m) = index is sensor ID, p1 = sensor loc, p2 = beacon loc, m = distance
            var sl = new List<((int, int), (int, int), int)>();

            // List of beacons, the "b"s for short
            // Tuple (p2)
            var bs = new List<(int, int)>();

            // split string on regex and use capture groups to pull out numbers
            // Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            string pat = @".*(Sensor at x=)([-]{0,1}\d+), y=([-]{0,1}\d+): closest beacon is at x=([-]{0,1}\d+), y=([-]{0,1}\d+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            while (input.Count > 0)
            {
                //Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                var li = input.Dequeue().Trim();

                // Match the regular expression pattern against a text string.
                Match m = r.Match(li);

                // find manhattan distance for each coord.
                var p1 = (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
                var p2 = (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

                var man = int.Abs(p1.Item1 - p2.Item1) + int.Abs(p1.Item2 - p2.Item2);
                sl.Add((p1, p2, man));
                bs.Add(p2);
            }

            // Eliminate any points whose Manhattan dist to beacon puts them
            // completely above (Y axis) or below the Line of Interest
            // Check only those sensors that overlap LOI
            var slToCheck = new List<int>();
            foreach (var s in sl.Select((value, i) => new {i, value}))
            {
                var bottomY = s.value.Item1.Item2 + s.value.Item3;  // Y + manhattan
                var topY = s.value.Item1.Item2 - s.value.Item3;   // Y - manhattan

                if (bottomY >= LOI && topY <= LOI)
                {
                    slToCheck.Add(s.i);
                }
            }

            // Find all the points in each sensor-beacon pairing that
            // overlap / touch the LOI.
            foreach (var i in slToCheck)
            {
                var s = sl[i];

                // count points overlapping LOI
                var top = s.Item1.Item2 - s.Item3;  // Y - m top of the diamond
                var bot = s.Item1.Item2 + s.Item3;  // Y + m bottom of the diamond

                // N of squares to count off
                // on either side of where the Y coord crosses the area
                // covered by this sensor-beacon pairiing
                // diamond shape, take Abs values
                var N = int.Abs(s.Item3 - int.Abs(LOI -s.Item1.Item2));

                // add X coords to cannot contain list:
                // the X coord of sensor beacon + the X coords on either side 
                // the line the LOI cuts thru the sensor-beacon covered area:
                // AND this spot doesn't have a beacon in it.
                for (int k = 0; k <= N; k++)
                {
                    var x1 = s.Item1.Item1 - k;
                    if (!bs.Contains((x1, LOI)))
                        cannotContain.Add(x1);

                    var x2 = s.Item1.Item1 + k;
                    if (!bs.Contains((x2, LOI)))
                        cannotContain.Add(x2);
                }
            }

            // number of X coords cover by some sensor-beacon pair
            // debug:
            //cannotContain.ToList().Sort();
            //Console.WriteLine(String.Join(',',cannotContain.ToArray()));


            var answer = cannotContain.Count;



            Console.WriteLine("Day 15 Part 1");
            Console.WriteLine("Consult the report from the sensors you just deployed.");
            Console.WriteLine("In the row where y=2000000, how many positions cannot contain a beacon?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two
            var lo = 0;
            var hi = 20;
            // var hi = 4000000;

            // tuning frequency, which can be found by multiplying its x coordinate by 4000000
            // and then adding its y coordinate
            var tf = 0;

            Console.WriteLine("Day 15 Part 2");
            Console.WriteLine("Find the only possible position for the distress beacon.");
            Console.WriteLine("What is its tuning frequency?");
            Console.WriteLine($"{tf}");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}
// 2682985 too low!
// 5367037 p1

