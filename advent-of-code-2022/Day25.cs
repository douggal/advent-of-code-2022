using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day25
    {
        public static void RunDay25()
        {
            // created 25 December 2022
            // https://adventofcode.com/2022/day/25

            Console.WriteLine("--- Day 25: Full of Hot Air ---");

            // data file
            var df = "day25-test.txt";
            //var df = "day25-input.txt";

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

            // Bob needs to know the total amount of fuel that will be processed
            // ahead of time so it can correctly calibrate heat output and flow rate.
            // This amount is simply the sum of the fuel requirements of all of the
            // hot air balloons

            // "SNAFU works the same way, except it uses powers of five instead of ten.
            // Starting from the right, you have a ones place, a fives place, a
            // twenty-fives place, a one-hundred-and-twenty-fives place, and so on."

            // "the digits are 2, 1, 0, minus (written -), and double-minus (written =).
            // Minus is worth -1, and double-minus is worth -2."

            // Plan: split each input string on chars and do the math?

            var digits = new Dictionary<char, int>() {
            { '0', 0},
            { '1', 1},
            { '2', 2},
            { '-', -1},
            { '=', -2}
            };

            var snafuAnswer = 0;
            while (input.Count > 0)
            {
                var li = input.Dequeue().Trim().ToCharArray();

                var lir = li.Reverse().ToList();

                var sum = 0;
                foreach (var c in lir.Select((v, i) => new { i, v }))
                {
                    sum += digits[c.v] * (int)Math.Pow(5d, (double)c.i);
                    //Console.Write(sum + ", ");
                }
                //Console.WriteLine( " = sum is ", sum);
                snafuAnswer += sum;
            }

            // change snafuAnswer from decimal back into SNAFU numbers

            // Ref:  https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net
            //var snafu1 = Convert.ToString(snafuAnswer,5);  doesn't work, can't use base5

            // Convert from decimal to destination base: divide the decimal with the base
            // until the quotient is 0 and calculate the remainder each time.
            // The destination base digits are the calculated remainders.
            // Ref: https://www.mathsisfun.com/base-conversion-method.html

            // Reverse a string in C# takes some work, so using reverse in lookup table
            // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string

            var sdigits = new Dictionary<int, string>() {
                {        0,              "0"},
                {        1,              "1"},
                {        2,              "2"},
                {        3,             "=1"},
                {        4,             "-1"},
                {        5,             "01"},
                {        6,             "11"},
                {        7,             "21"},
                {        8,             "=2"},
                {        9,             "-2"},
                {       10,             "02"},
                {       15,            "0=1"},
                {       20,            "0-1"},
                {     2022,          "2-11=1"},
                {    12345,        "0---0-1" },
                { 314159265,  "0=1-0111-1211"},
            };

            // Reverse string in C#
            // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string

            var answer = String.Empty;
            var q = snafuAnswer;
            var r = 0;
            do
            {
                (q, r) = int.DivRem(q, 5);
                answer += sdigits[r];
            } while (q != 0);

            var answerString = string.Empty;
            foreach (var c in answer)
            {
                answerString += c.ToString();
            }


            Console.WriteLine("Day 25 Part 1");
            Console.WriteLine("The Elves are starting to get cold. What SNAFU number");
            Console.WriteLine("do you supply to Bob's console?");
            Console.WriteLine($"{answerString}");





            // ---------------------------
            // Part Two
            // ---------------------------
            // TODO
            Console.WriteLine("Day 25 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

