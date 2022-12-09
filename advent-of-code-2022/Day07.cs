using System;
using System.Text;
using System.Diagnostics;

namespace advent_of_code_2022
{
    public static class Day07
    {
        public static void RunDay07()
        {
            // created 7 December 2022
            // https://adventofcode.com/2022/day/7

            Console.WriteLine("--- Day 07: No Space Left On Device ---");

            // data file
            //var df = "day07-test.txt";
            var df = "day07-input.txt";

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
            Console.WriteLine($"Start timestamp {utcDateStart.ToString("O")}");

            // create and start a Stopwatch instance
            // https://stackoverflow.com/questions/16376191/measuring-code-execution-time
            Stopwatch stopwatch = Stopwatch.StartNew();


            // Part One

            // Model the folder structure as a dictionary of path strings
            // one entry for each folder/directory
            // fs = filesystem: each key is the path
            // and value is sum of file sizes for that directory only
            Dictionary<string, int> fs = new() { { "/", 0 } };

            // wd = working directory - current location
            var wd = String.Empty;
            var ctr = 0;  //debug
            while (input.Count > 0)
            {
                ctr += 1;
                var li = input.Dequeue().Trim().Split(' ');

                // try out new C# version 11 list pattern matching feature

                if (li is ["$", "cd", "/"])  // move to root
                {
                    wd = "/";
                }
                else if (li is ["$", "cd", ".."])
                {
                    // change directory cmd - check and add to fs
                    // cd = going in to a directory (or up one)
                    // remove last dir from end of wd
                    var i = wd.LastIndexOf('/');
                    if (i > 0) wd = wd.Substring(0, i);  // below root, chop off last dir
                    else wd = "/"; // move to root
                    if (!fs.ContainsKey(wd)) fs[wd] = 0;  // shouldn't happen?
                }
                else if (li is ["$", "cd", var f])
                {
                    // add folder f to wd
                    // if in root then add folder to wd; else add "/" directory name
                    if (wd.CompareTo("/") == 0) wd = String.Concat(wd, f);
                    else wd = String.Concat(wd, "/", f);
                    // if filesystem doesn't already contain this folder add it
                    if (!fs.ContainsKey(wd)) fs[wd] = 0;
                }
                else if (li is ["$", "ls"])
                {
                    // listing follows - need to do anything with "ls"?

                }
                else if (li is ["dir", var dn])  // dn = directory name
                {
                    // ls output, found a directory
                    var newDir = (wd.Last().CompareTo('/') != 0) ? String.Concat(wd, "/", dn) : String.Concat(wd, dn);
                    if (!fs.ContainsKey(newDir)) fs[newDir] = 0;
                }
                else if (li is [var fsize, var fname])
                {
                    // file with size and name
                    // for part 1 don't care about fname
                    fs[wd] += int.Parse(fsize);
                }
                else
                {
                    Console.WriteLine($"Houston, we have a problem. Input line {ctr}: {String.Join(' ', li)}");
                }
            }

            //debug: print the collected paths and size, look for mistakes
            //printPaths(fs);

            // compute answer
            // sum of folder if less than 1e5 + total size of any subfolders
            var answer = Sum100k(fs, 100000);

            Console.WriteLine("Day 7 Part 1");
            Console.WriteLine("Find all of the directories with a total size of at most 100000.");
            Console.WriteLine("What is the sum of the total sizes of those directories?");
            Console.WriteLine($"{answer}\n\n");


            // Part Two
            var totalSpace = 70000000;
            var updateSpace = 30000000;
            var freeSpace = totalSpace - (fs["/"] + sumSubFolders(fs, "/"));
            var neededSpace = updateSpace - freeSpace;
            Console.WriteLine($"Free {freeSpace}");
            Console.WriteLine($"Needed {neededSpace}");
            Console.WriteLine($"In use {fs["/"] + sumSubFolders(fs, "/")}");

            // Therefore, the update still requires a directory with total size of at least
            // 27 573 755 to be deleted before it can run.

            // find size of all directories
            // in C# the LINQ Select() method is a map operation
            List<int> candidates = fs.Select(x => x.Value + sumSubFolders(fs,x.Key)).ToList();

            // What is the size of the smallest that would free up neededSpace?
            var answerp2 = candidates.Where(x => x >= neededSpace).Min();

            Console.WriteLine("Day 7 Part 2");
            Console.WriteLine("Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update.");
            Console.WriteLine($"What is the total size of that directory?");
            Console.WriteLine($"{answerp2}\n\n");



            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }

        private static void printPaths(Dictionary<string, int> fs)
        {
            foreach (var d in fs)
            {
                Console.WriteLine($"{d.Key} : {d.Value,-15}");
            }
        }

        private static int Sum100k(Dictionary<string, int> fs, int limit)
        {
            // sum of folder if less than 1e5 + total size of any subfolders
            var answer = 0;
            foreach (var d in fs)
            {
                // this folder
                var sum = d.Value;

                // sfs = its subfolders
                // note don't add same folder back in again
                var sumOfSubfolders = sumSubFolders(fs, d.Key);

                var total = sum + sumOfSubfolders;

                if (total <= limit) answer += total;
            }

            return answer;
        }

        private static int sumSubFolders(Dictionary<string, int> fs, string d)
        {
            var s = 0;
            if (d.CompareTo("/") == 0)
            {
                // root sum up everything
                s = fs.Where(x => x.Key.CompareTo("/") != 0)
                    .Sum(x => x.Value);
            }
            else
            {
                var sfs = fs.Where(x => x.Key.StartsWith(d + "/") && x.Key.CompareTo(d) != 0)
                            .Select(x => x.Key).ToList();

                // sumOfSubfolders = sum of sizes of each subfolder
                s = fs.Where(x => sfs.Contains(x.Key))
                                        .Sum(x => x.Value);
                // debug:
                //Console.WriteLine($"\n[{d.Key}]  Sum of folders: {total}");
                //foreach (var q in sfs)
                //{
                //Console.WriteLine($"{q}");
                //}
            }

            return s;
        }
    }
}

// 2190855 too high
// 2031851 star!
//  37134888  p2  51ms too high
// 2654809  49ms  still too high
// 2568781  47ms  star!
