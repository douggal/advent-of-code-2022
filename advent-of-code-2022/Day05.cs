﻿using System;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace advent_of_code_2022
{
    public static class Day05
    {
        public static void RunDay05()
        {
            // created 5 December 2022
            // https://adventofcode.com/2022/day/5

            Console.WriteLine("--- Day 05: Supply Stacks ---");

            // data file
            var df = "day05-test.txt";
            //var df = "day05-input.txt";

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
            // 1st thought is Stacks!  
            // Picture:  push all the input up to the blank line onto a stack
            // p = picture data
            Stack<string> p = new();
            var li = String.Empty;
            do 
            {
                li = input.Dequeue();
                p.Push(li.Trim());
            } while (input.Count > 0 && li != String.Empty);
            p.Pop();  // discard blank line

            // find out how many stacks there are
            // and create a Dictionary which represents a ship with stacks of containers
            var stacksStr = p.Pop();
            Regex rx = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            stacksStr = rx.Replace(stacksStr," ");
            Dictionary<int,Stack<string>> ship = new();
            foreach (string s in (stacksStr.Split(" ")))
            {
                ship.Add(int.Parse(s), new Stack<string>());
            }
            // finish intializing
            //how many stacks of containers are there on the ship?
            var nStacks = ship.Keys.ToList().Max();
            // get a row of data from picture
            var r = p.Pop(); 
            // and break it up into N sections, each will contain 0 or 1 container
            // cs = containers, and c is a single container
            List<string> cs = new();
            var columnWidth = r.Length / nStacks + 1;
            var i = 0;
            while (i < r.Length)
            {
                if (i + columnWidth <= r.Length) {
                    // pick up this section or chunk
                    cs.Add(String.Join(String.Empty, String.Join(String.Empty, r.Substring(i, columnWidth).ToList())));
                }
                else if (i < r.Length) {
                    // near end of the string, pick up last section/chunk
                    cs.Add(String.Join(String.Empty, String.Join(String.Empty, r.Substring(i).ToList())));
    
                }
                i+=columnWidth;
            }
            // then parse each section to find 0 or 1 container 
            // and add it to the container stack if one is found
            Regex cx = new Regex(@"\[(\w+)]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // foreach loop with index https://stackoverflow.com/questions/521687/foreach-with-index
            foreach (var c in cs.Select((x, i) => new { Value = x, Index = i }))
            {
                var m = cx.Matches(c.Value).ToList();
                ship[c.Index+1].Push(m[0].Groups[1].Value);
            }
var y=1;

            var ansP1 = "ABCD";
            Console.WriteLine("Day 5 Part 1 TBD");
            Console.WriteLine("After the rearrangement procedure completes, what crate ends up on top of each stack?");
            Console.WriteLine($"{ansP1}\n\n");

            // Part Two
            // TODO
            Console.WriteLine("Day 5 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

