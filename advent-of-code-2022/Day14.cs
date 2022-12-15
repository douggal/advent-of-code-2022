using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace advent_of_code_2022
{
    public static class Day14
    {
        public static void RunDay14()
        {
            // created 14 December
            // https://adventofcode.com/2022/day/14

            Console.WriteLine("--- Day 14: Regolith Reservoir ---");

            // data file
            var df = "day14-test.txt";
            //var df = "day14-input.txt";

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

            // I'm going to represent the two-dimensional vertical slice the
            // scan produces as a sparse matrix.  Googling around to see if .NET has a
            // built-in sparse matrix type in the BCL (it doesn't) I found this on Stackoverflow.com
            // I found this code snippet which I will use.  Should reduce the problem
            // to loading of data then calling IsEmptyCell() method to test at each
            // point in the scan's grid.
            // Ref:  https://stackoverflow.com/questions/756329/best-way-to-store-a-sparse-matrix-in-net

            // 0 = empty space
            // 2 = rock
            // 1 = sand unit

            // estimate size at 1024^2 cells
            var sm = new SparseMatrix<int>(1024, 1024);
            var highx = int.MinValue;
            var highy = int.MinValue;

            // load the sparse matrix, sm
            while (input.Count > 0)
            {
                // split each line of input into a sequence of points (comma sep strings)
                // split all the points into an array of tuple2's converting x and y to int
                var li = input.Dequeue().Split(" -> ").ToList();
                var ps = li.Select(x => x.Split(','))
                    .Select((x, y) => (int.Parse(x[0]), int.Parse(x[1])))
                    .ToArray(); // end up with an array of tuple2's points (x,y)

                // start with first point, take 2 elements at a time to form
                // pairs of points { {A,B}, ... } 
                // Ref: https://stackoverflow.com/questions/1624341/getting-pair-set-using-linq?noredirect=1&lq=1
                var pairs = ps.Where((e, i) => i < ps.Count() - 1)
                        .Select((e, i) => new { A = e, B = ps[i + 1] });

                // "draw" a line of rocks between each pair of points
                foreach (var p in pairs)
                {
                    // each pair of points, p is made up of two tuple2's A (from) and B (to)
                    if (p.A.Item2 == p.B.Item2)
                    {
                        // horizontal line of rocks
                        var y = p.A.Item2;
                        if (p.A.Item1 <= p.B.Item1)
                        {
                            for (int x = p.A.Item1; x <= p.B.Item1; x++)
                            {
                                sm[x, y] = 2;
                            }
                        }
                        else
                        {
                            for (int x = p.B.Item1; x <= p.A.Item1; x++)
                            {
                                sm[x, y] = 2;
                            }
                        }
                    }
                    else  // (p.A.Item1 == p.B.Item1)
                    {
                        // vertical line of rocks
                        var x = p.A.Item1;
                        if (p.A.Item2 <= p.B.Item2)
                        {
                            for (int y = p.A.Item2; y <= p.B.Item2; y++)
                            {
                                sm[x, y] = 2;
                            }
                        }
                        else
                        {
                            for (int y = p.B.Item2; y <= p.A.Item2; y++)
                            {
                                sm[x, y] = 2;
                            }
                        }
                    }
                    if (p.A.Item1 > highx) highx = p.A.Item1;
                    if (p.B.Item1 > highx) highx = p.B.Item1;
                    if (p.A.Item2 > highy) highy = p.A.Item2;
                    if (p.B.Item2 > highy) highy = p.B.Item2;
                }
            }

            // debug: visualize the sm
            // Assert:  known cell has value of 2
            //Console.WriteLine($"Assert: (498,5) == 2 {sm[498, 5] == 2}");
            //Console.WriteLine($"Assert: (500,9) == 2 {sm[500, 9] == 2}");
            //PrintSM(sm);


            // run simulation and find answer
            var answer = 0;

            // Drop a sand unit
            // Let it fall thru the system
            // Until it either falls towards infinity or stops moving
            var range = Enumerable.Range(0, 10000000);
            //foreach (var d in range)
            //{
            //    // drop sand unit
            //    bool end = DropSandUnit(ref sm, 500, 0, 1024);

            //    // if is infinity then done?
            //    // I think so, there's no randomness so once
            //    // one grain falls to infinity all after will do same.

            //    if (end)
            //    {
            //        answer = d;   // How many units BEFORE runs to infinity?
            //        break;
            //    }

            //}

            //PrintSM(sm);

            Console.WriteLine("Day 14 Part 1");
            Console.WriteLine("Using your scan, simulate the falling sand.");
            Console.WriteLine("How many units of sand come to rest before sand starts flowing into the abyss below?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two

            // add the floor, a horizontal line of rocks
            for (int x = 0; x <=sm.Width; x++)
            {
                sm[x, highy+2] = 2;
            }



            var answer2 = 0;
            var done = false;
            var ctr = 0;
            do
            {
                // drop sand unit
                ctr += 1;
                bool end = DropSandUnit(ref sm, 500, 0);

                // if is infinity then done?
                // I think so, there's no randomness so once
                // one grain falls to infinity all after will do same.

                if (end)
                {
                    answer2 = ctr-1;
                    break;
                }
            } while (!done);

            //PrintSM(sm);


            Console.WriteLine("Day 14 Part 2");
            Console.WriteLine("Using your scan, simulate the falling sand until the source of the sand becomes blocked.");
            Console.WriteLine("How many units of sand come to rest?");
            Console.WriteLine($"{answer2}");




            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        private static void PrintSM(SparseMatrix<int> sm)
        {
            // print sparse matrix
            foreach (var y in Enumerable.Range(0, sm.Height))
            {
                foreach (var x in Enumerable.Range(0, 1024))
                {
                    if (x == 500 && y == 0) Console.Write("+");
                    else Console.Write(sm[x, y] == 0 ? "." : sm[x, y].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static bool DropSandUnit(ref SparseMatrix<int> sm, int x, int y)
        {
            // TODO: needs clean up - could be recursive?  tail recursive?

            if (y >= sm.Height) 
            {
                // infinity
                return true;
            }
            else if (!sm.IsCellEmpty(x, y))
            {
                // full
                return true;
            }

            var newx = x;
            var newy = y;

            var done = false;
            do
            {
                if (newx >= sm.Width || newx < 0 || newy >= sm.Height)
                {
                    // infinity - off the grid on or more directions
                    return true;
                }
                else if (sm.IsCellEmpty(newx, newy + 1)) // can go down 1? keep falling
                {
                    newy += 1;
                }
                else if (sm.IsCellEmpty(newx - 1, newy + 1)) // can go down 1 and left 1? keep falling
                {
                    newx -= 1;
                    newy += 1;
                }
                else if (sm.IsCellEmpty(newx + 1, newy + 1))
                {
                    newx += 1;
                    newy += 1;
                }
                else
                    done = true;

            } while (!done);

            sm[newx, newy] = 1; // sand unit is at rest
            //PrintSM(sm);
            return false;
        }
    }

    /// <summary>
    /// Simple Sparse Matrix class
    /// Original code borrowed from Stackoverflow.com reply by "Erich Mirabal"
    /// https://stackoverflow.com/users/79294/erich-mirabal
    /// Ref:  https://stackoverflow.com/questions/756329/best-way-to-store-a-sparse-matrix-in-net
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SparseMatrix<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public long Size { get; private set; }

        private Dictionary<long, T> _cells = new Dictionary<long, T>();

        public SparseMatrix(int w, int h)
        {
            this.Width = w;
            this.Height = h;
            this.Size = w * h;
        }

        public bool IsCellEmpty(int row, int col)
        {
            long index = row * Width + col;
            return !_cells.ContainsKey(index);
        }

        public T this[int row, int col]
        {
            get
            {
                long index = row * Width + col;
                T result;
                _cells.TryGetValue(index, out result);
                return result;
            }
            set
            {
                long index = row * Width + col;
                _cells[index] = value;
            }
        }
    }

}

// P1 897
