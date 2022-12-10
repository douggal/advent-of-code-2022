using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace advent_of_code_2022
{
    public static class Day08
    {
        public static void RunDay08()
        {
            // created 9 December 2022
            // https://adventofcode.com/2022/day/8

            Console.WriteLine("--- Day 08: Treetop Tree House ---");

            // data file
            var df = "day08-test.txt";
            //var df = "day08-input.txt";

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
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");


            // Part One

            // Model the patch of trees as a single List of trees
            // An offset identifies which column a tree is in
            // A tree has a height property and a visible true/false property
            // Each tree is a instance of class, so we can pass the reference
            // around as needed.

            List<Tree> treePatch = new();
            var nItemsPerRow = input.First().Length;
            var nRows = input.Count;

            while (input.Count > 0)
            {
                var li = input.Dequeue()
                    .Trim()
                    .ToCharArray()
                    .Select(x => int.Parse(x.ToString()));
                foreach (var t in li)
                {
                    treePatch.Add(new Tree(t));
                }
            }

            #region debug1
            // debug: print out the patch of trees on the console
            //PrintTreePatch(treePatch, nItemsPerRow);
            #endregion

            /* Each tree is represented as a single digit whose value is its height,
             * where 0 is the shortest and 9 is the tallest. 
             * A tree is visible if all of the other trees between it and an edge
             * of the grid are shorter than it.
            */

            /* All trees are visible by default - no need to work the perimeter */
            /* TODO: for each tree compute if visible */
            /* skip/take - take trees one row at a time */

            /* For loop or Linq?  Answer: combine best of both Linq and C for loops
             * and choose the best tool for the job.
             * ref: https://stackoverflow.com/questions/37361331/how-to-iterate-a-loop-every-n-items 
             */

            // right and left
            for (int i = 0; i < nItemsPerRow; i += 1)
            {
                // get row
                var skip = i * nItemsPerRow;
                var row = treePatch.Skip(skip)
                    .Take(nItemsPerRow)
                    .ToList();

                row.First().Visible = true;
                row.Last().Visible = true;

                // for each tree in the row
                for (int j = 1; j < nItemsPerRow; j++)
                {
                    // Look right, for each item are all the trees shorter than this one trees?
                    // if so then set Visible property to true
                    if (row.Skip(j + 1)
                        .All(x => x.Height < row[j].Height)) row[j].Visible = true;

                    // Part2: still looking right, find scenic index
                    // e.g., looking at 5 in this row, 3,3,5,4,9
                    // get vector 4, 9, then count how many steps until find a
                    // tree as tall or taller than current (5) or hit the edge.
                    var ssVector = row.Skip(j + 1)
                        .Select(x => x)
                        .ToList();
                    var ss = 0;
                    for (int k = ssVector.Count - 1; k >= 0; k--)
                    {
                        if (ssVector[ss].Height < row[j].Height)
                        {
                            ss += 1;
                        }
                        else break;
                    }
                    row[j].ScenicScores.Add(ss);

                    // Look left, ditto looking left
                    if (row.Take(j)
                        .All(x => x.Height < row[j].Height)) row[j].Visible = true;

                    // Part2: still looking left, find scenic index
                    // e.g., looking at 5 in this row, 3,3,5,4,9
                    // get vector 3,3, then count how many steps to the left until find a
                    // tree as tall or taller than current (5) or hit the edge.
                    ssVector = row.Take(j)
                        .Select(x => x)
                        .ToList();
                    ss = 0;
                    for (int k = ssVector.Count - 1; k >= 0; k--)
                    {
                        if (ssVector[ss].Height < row[j].Height)
                        {
                            ss += 1;
                        }
                        else break;
                    }
                    row[j].ScenicScores.Add(ss);
                }
            }

            // up and down
            for (int i = 0; i < nRows; i += 1)
            {
                // get column
                // ref: https://stackoverflow.com/questions/682615/how-can-i-get-every-nth-item-from-a-listt
                var col = treePatch.Skip(i)
                    .Where((t, idx) => idx % nItemsPerRow == 0)
                    .ToList();

                col.First().Visible = true;
                col.Last().Visible = true;

                // for each tree in the row
                for (int j = 1; j < nRows; j++)
                {
                    // Part 1: Look down, for each item are all the trees shorter than this one trees?
                    // if so then set Visible property to true
                    if (col.Skip(j + 1)
                        .All(x => x.Height < col[j].Height))  col[j].Visible = true;

                    // Part2: still looking right, find scenic index
                    // e.g., looking at last 5 in this column, 3,5,3,5,3
                    // get vector 3, then count how many steps until find a
                    // tree as tall or taller than current (5) or hit the edge.
                    var ssVector = col.Skip(j + 1)
                        .Select(x => x)
                        .ToList();
                    var ss2 = 0;
                    for (int k = ssVector.Count - 1; k >= 0; k--)
                    {
                        if (ssVector[k].Height < col[j].Height)
                        {
                            ss2 += 1;
                        }
                        else break;
                    }
                    col[j].ScenicScores.Add(ss2);


                    // Part 1: Look up, ditto looking left
                    if (col.Take(j)
                        .All(x => x.Height < col[j].Height))  col[j].Visible = true;

                    // Part2: still looking up, find scenic index
                    ssVector = col.Take(j)
                        .Select(x => x)
                        .ToList();
                    ss2 = 0;
                    for (int k = ssVector.Count - 1; k >= 0; k--)
                    {
                        if (ssVector[ss2].Height < col[j].Height)
                        {
                            ss2 += 1;
                        }
                        else break;
                    }
                    col[j].ScenicScores.Add(ss2);

                }
            }

            //PrintTreePatch(treePatch, nItemsPerRow);

            var answer = treePatch.Where(t => t.Visible).Count();

            Console.WriteLine("Day 8 Part 1");
            System.Console.WriteLine("Consider your map; how many trees are visible from outside the grid?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two

            // best possible scenic score is max value found in the list of product of each tree's ss
            var answerp2 = treePatch
                .Select(t => t.ScenicScores.Aggregate(1, (a, b) => a * b))
                .Max();

            Console.WriteLine("Day 8 Part 2");
            Console.WriteLine($"Consider each tree on your map. What is the highest scenic score possible for any tree?");
            Console.WriteLine($"{answerp2}");

            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            Console.ReadKey();
        }

        private static void PrintTreePatch(List<Tree> treePatch, int n)
        {
            // n = nbr trees per row
            // Ref: https://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq
            // Note: Linq features lazy evaluation - doesn't do anything until ToList etc is
            // called forcing an evaluation. Hence I first split up the treePatch,
            var tmp = treePatch.Select((value, index) => new { Index = index, Value = value })
                            .GroupBy(i => i.Index / n)
                            .Select(i => i.Select(i2 => i2.Value));
            // and then interate over the IEnumerable<IEnumerable<Tree>> calling ToList to force
            // evaluation of each row in order for whole list (row) to be sent to String.Join.
            foreach (var t in tmp)
            {
                Console.WriteLine(String.Join(", ", t.Select(y => String.Format("h:{0},v:{1}", y.Height, y.Visible)).ToList()));
            }
            Console.WriteLine();
        }
    }

    public class Tree
    {
        public int Height { get; set; }
        public bool Visible { get; set; }
        public List<int> ScenicScores { get; set; }

        public Tree(int h)
        {
            Height = h;
            Visible = false;  // assume it's not visible unless shown otherwise.
            ScenicScores = new();  // multiplicative identity
        }
    }
}
// 1763 go

