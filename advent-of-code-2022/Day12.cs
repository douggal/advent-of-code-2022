using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using QuikGraph;
using QuikGraph.Algorithms;
using System.Diagnostics.Metrics;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;

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
            //var df = "day12-test.txt";
            var df = "day12-input.txt";

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

            // letters in order of height a = lowest and z = highest
            // rather than rely on comparing ASCII or Unicode points
            // comparisions will reference ordering in this list
            var letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray()
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

            // ID of each vertex in the graph numbered from
            // top left to bottom right
            var id = 0;

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    id++;
                    heightmap[id] = new List<int>();

                    var here = temp[i, j];
                    labels[id] = here;
                    if (string.Compare(here, "S") == 0)
                    {
                        startVertexID = id;
                        here = "a";  // start equiv a
                    }
                    if (string.Compare(here, "E") == 0)
                    {
                        endVertexID = id;
                        here = "z";  // end equiv z
                    }

                    // right
                    if (j < nCols - 1)
                    {
                        var right = CheckLetter(temp[i, j + 1]);
                        var rightVertexID = id + 1;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char next in order/higher
                        var idx = letters.IndexOf(here);
                        var next = letters.IndexOf(right);
                        if (String.Compare(right, here) <= 0)
                            heightmap[id].Add(rightVertexID);
                        else if (next - idx == 1)
                            heightmap[id].Add(rightVertexID);
                        else;   // do nothing
                    }

                    // down
                    if (i < nRows - 1)
                    {
                        var down = CheckLetter(temp[i + 1, j]);
                        var downVertexID = id + nCols;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(here);
                        var next = letters.IndexOf(down);
                        if (String.Compare(down, here) <= 0)
                            heightmap[id].Add(downVertexID);
                        else if (next - idx == 1)
                            heightmap[id].Add(downVertexID);
                        else;   // do nothing
                    }

                    // left
                    if (j > 0)
                    {
                        var left = CheckLetter(temp[i, j - 1]);
                        var leftVertexID = id - 1;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(here);
                        var next = letters.IndexOf(left);
                        if (String.Compare(left, here) <= 0)
                            heightmap[id].Add(leftVertexID);
                        else if (next - idx == 1)
                            heightmap[id].Add(leftVertexID);
                        else;   // do nothing
                    }

                    // up
                    if (i > 0)
                    {
                        var up = CheckLetter(temp[i - 1, j]);
                        var upVertexID = id - nCols;

                        // if right equals current or
                        // or it is any amount lower
                        // or right is no more than 1 char higher
                        var idx = letters.IndexOf(here);
                        var next = letters.IndexOf(up);
                        if (String.Compare(up, here) <= 0)
                            heightmap[id].Add(upVertexID);
                        else if (next - idx == 1)
                            heightmap[id].Add(upVertexID);
                        else;   // do nothing
                    }
                }
            }

            // debug: print tree
            //foreach (var kv in heightmap.OrderBy(k => k.Key))
            //{
            //    Console.WriteLine($"{kv.Key}: {String.Join(',', kv.Value)}");
            //}

            // Workaround: could not find a "ConvertAll" method for List<T>
            // So, copy to new Dictionary but turn the values from List<int> to int[]
            Dictionary<int, int[]> heightmap2 = new();
            foreach (var kv in heightmap)
            {
                heightmap2[kv.Key] = heightmap[kv.Key].ToArray();
            }

            // Bring in QuikGraph: create a graph
            // Ref: https://github.com/KeRNeLith/QuikGraph/wiki/Creating-Graphs
            var graph = heightmap2.ToDelegateVertexAndEdgeListGraph(
                kv => Array.ConvertAll(kv.Value, v => new Edge<int>(kv.Key, v)));

            // Following example in tutorial here...
            // https://github.com/KeRNeLith/QuikGraph/wiki/Shortest-Path
            Func<Edge<int>, double> cityDistances = edge => 1; // A delegate that gives the distance between cities
            int sourceCity = startVertexID; // Starting city
            int targetCity = endVertexID; // Ending city

            // Creating the algorithm instance
            var dijkstra = new DijkstraShortestPathAlgorithm<int, Edge<int>>(graph, cityDistances);

            // Creating the observer
            var vis = new VertexPredecessorRecorderObserver<int, Edge<int>>();

            // Compute and record shortest paths
            using (vis.Attach(dijkstra))
            {
                dijkstra.Compute(sourceCity);
            }

            // what answer did it find?
            var answer = 0;

            // vis can create all the shortest path in the graph
            if (vis.TryGetPath(targetCity, out IEnumerable<Edge<int>> path))
            {
                answer = path.Count();
                foreach (Edge<int> edge in path)
                {
                    Console.WriteLine(edge);
                }
            }

            Console.WriteLine("Day 12 Part 1");
            Console.WriteLine($"What is the fewest steps required to move from your current position to");
            Console.WriteLine("the location that should get the best signal?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two

            // which vertices have a "a" elevation?
            var a = labels.Where(x => string.Compare(x.Value, "a") == 0)
                .Select(x => x.Key)
                .ToList();

            Console.WriteLine($"Number of vertices with 'a' elevation is {a.Count}");

            // for each vertex at elevation "a", find shortest path to E
            var stepsInEachPath = new List<int>();
            foreach (var v in a)
            {

                // Creating the observer
                var o = new VertexPredecessorRecorderObserver<int, Edge<int>>();

                // Compute and record shortest paths
                using (o.Attach(dijkstra))
                {
                    dijkstra.Compute(v);
                }

                // what answer did it find?  Add to list.
                // vis can create all the shortest path in the graph
                if (o.TryGetPath(targetCity, out IEnumerable<Edge<int>> pth))
                {
                    stepsInEachPath.Add(pth.Count());
                    Console.WriteLine($"Computed vertex ID {v} value {pth.Count()}");
                    //foreach (Edge<int> edge in pth)
                    //{
                    //    Console.WriteLine(edge);
                    //}
                }
            }

            var answer2 = stepsInEachPath.Min();

            Console.WriteLine("Day 12 Part 2");
            Console.WriteLine("What is the fewest steps required to move starting from any square with elevation a");
            Console.WriteLine("to the location that should get the best signal?");
            Console.WriteLine($"{answer2}\n\n");


            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        private static string CheckLetter(string letter)
        {
            var ret = letter;
            if (string.Compare(letter, "S") == 0) ret = "a";
            if (string.Compare(letter, "E") == 0) ret = "z";

            return ret;
        }
    }
}

// P1 394

