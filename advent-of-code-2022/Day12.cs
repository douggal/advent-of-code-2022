using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using QuikGraph;
using System.Diagnostics.Metrics;

namespace advent_of_code_2022
{
    public static class Day12
    {
        public static void RunDay12()
        {
            // created 12 December 2022
            // https://adventofcode.com/2022/day/12

            Console.WriteLine("--- Day 12: Hill Climbing Algorithm ---");

            // data file
            var df = "day12-test.txt";
            //var df = "day12-input.txt";

            // read in data
            var fn = Path.Combine(Directory.GetCurrentDirectory(), "inputData", df);
            var input = new Queue<String>();
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

            //Model the data as a graph and use QuikGraph to solve for shortest path.

            // First, create a dictionary of vertices (keys) and edges (values)
            // from the input data.  Use a 2x array as step inbetween
            // because it's easier to find the edges of each vertex that way
            var nCols = input.First().Length;
            var nRows = input.Count;
            var letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray()
                .Select(x => x.ToString())
                .ToList();
            var lettersNext = "bcdefghijklmnopqrstuvwxyzz".ToCharArray()
                .Select(x => x.ToString())
                .ToList();

            string[,] temp = new string[nRows, nCols];

            var col = 0;
            while (input.Count > 0)
            {
                var li = input.Dequeue()
                    .Trim()
                    .ToCharArray()
                    .Select(x => x.ToString())
                    .ToList();
                foreach (var v in li.Select((v, i) => new { vertex = v, idx = i }))
                {
                    temp[col, v.idx] = v.vertex;
                }
                col++;
            }

            Dictionary<int, List<int>> heightmap = new();
            Dictionary<int, string> labels = new();
            var startVertexID = -1;
            var endVertexID = -1;
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    var id = (i + 1) * (j + 1);
                    var thisHeight = temp[i, j];
                    labels[id] = thisHeight;
                    if (string.Compare(thisHeight, "S") == 0)
                    {
                        startVertexID = id;
                        thisHeight = "a";  // start equiv a
                    }
                    if (string.Compare(thisHeight, "E") == 0)
                    {
                        endVertexID = id;
                        thisHeight = "z";  // end equiv z
                    }
                    if (!heightmap.ContainsKey(id))
                        heightmap[id] = new List<int>();

                    // right
                    if (j < nCols - 2)
                    {
                        var right = temp[i, j + 1];
                        var rightVertexID = id + 1;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(thisHeight);
                        var next = lettersNext[idx];
                        if (String.Compare(right, thisHeight) <= 0)
                            heightmap[id].Add(rightVertexID);
                        else if (letters.IndexOf(next) - idx == 1)
                            heightmap[id].Add(rightVertexID);
                        else;   // do nothing
                    }

                    // down
                    if (i < nRows - 2)
                    {
                        var down = temp[i + 1, j];
                        var downVertexID = id + nCols;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(thisHeight);
                        var next = lettersNext[idx];
                        if (String.Compare(down, thisHeight) <= 0)
                            heightmap[id].Add(downVertexID);
                        else if (letters.IndexOf(next) - idx == 1)
                            heightmap[id].Add(downVertexID);
                        else;   // do nothing
                    }

                    // left
                    if (j > 0)
                    {
                        var left = temp[i, j-1];
                        var leftVertexID = id - 1;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(thisHeight);
                        var next = lettersNext[idx];
                        if (String.Compare(left, thisHeight) <= 0)
                            heightmap[id].Add(leftVertexID);
                        else if (letters.IndexOf(next) - idx == 1)
                            heightmap[id].Add(leftVertexID);
                        else;   // do nothing
                    }

                    // up
                    if (i > 0)
                    {
                        var up = temp[i-1, j];
                        var upVertexID = id - nCols;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(thisHeight);
                        var next = lettersNext[idx];
                        if (String.Compare(up, thisHeight) <= 0)
                            heightmap[id].Add(upVertexID);
                        else if (letters.IndexOf(next) - idx == 1)
                            heightmap[id].Add(upVertexID);
                        else;   // do nothing
                    }
                }
            }

            // QuikGraph: create a graph
            // Ref: https://github.com/KeRNeLith/QuikGraph/wiki/Creating-Graphs
            var graph = heightmap.ToDelegateVertexAndEdgeListGraph(
                kv => Array.ConvertAll(kv.Value, v => new Edge<int>(kv.Key, v)));

            var xyz = 0;

            Console.WriteLine("Day 12 Part 1");


            // Part Two
            // TODO
            Console.WriteLine("Day 12 Part 2  [TBD]");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

