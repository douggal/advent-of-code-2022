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

            // first thought is to model the folder structure as a dictionary of paths
            // one entry for each folder/directory
            // fs = filesystem
            Dictionary<string, int> fs = new() { { "/", 0 } };  // init to root folder

            // wd = working directory - current location
            var wd = String.Empty;

            while (input.Count > 0)
            {
                var li = input.Dequeue().Trim().Split(' ');

                // try out new C# version 11 list pattern matching feature
                if (li is ["$", "cd", var f])  // f = folder name
                {
                    // change directory cmd - check and add to fs
                    // cd = going in to a directory (or up one)
                    if (f == "..")
                    {
                        // remove last dir from end of wd
                        var i = wd.LastIndexOf('/');
                        if (i > 0) wd = wd.Substring(0, i-1);  // below root, move up
                        else wd = "/"; // root
                    }
                    else
                    {
                        // add folder f to wd
                        if (f.CompareTo("/") == 0) wd = "/";  // to root
                        else if (wd.CompareTo("/") == 0) wd = String.Concat(wd, f); // in root down 1
                        else wd = String.Concat(wd, "/", f);  // not root, add /folder

                        if (!fs.ContainsKey(wd)) fs[wd] = 0;
                    }
                }
                else if (li is ["$", "ls"])
                {
                    // listing - need to do anything?

                }
                else if (li is ["dir", var dn])  // dn = directory name
                {
                    // have a directory
                    var newDir = (wd.Last().CompareTo('/') != 0) ? String.Concat(wd,"/",dn) : String.Concat("/",dn);
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
                    Console.WriteLine("Houston, we have a problem.");
                }
            }

            // print folder tree
            foreach (var d in fs)
            {
                Console.WriteLine($"{d.Value,-15} : {d.Key}");
            }

            // compute answer
            // sum of folder if less than 1e5 + total size of any subfolders
            // note to self - don't add same folder back in again
            // no go: var answer = fs.Sum(x => x.Value <= 1e5 ? x.Value + fs.Sum(y => y.Key.Contains(x.Key) ? y.Value : 0) : 0);
            var answer = 0;
            foreach (var d in fs)
            {
                var sum = d.Value +
                    fs.Sum(x => (x.Key.StartsWith(d.Key + "/")
                        && (x.Key != d.Key) // not same folder
                        && (x.Key.Length > d.Key.Length)) // and is longer, i.e., sub-folder
                        ? x.Value : 0);
                if (sum <= 1e5) answer += sum;
            }

            Console.WriteLine("Day 07 Part 1");
            Console.WriteLine("Find all of the directories with a total size of at most 100000.");
            Console.WriteLine("What is the sum of the total sizes of those directories?");
            Console.WriteLine($"{answer}");


            // Part Two
            // TODO
            Console.WriteLine("Day 07 Part 2  [TBD]");



            // Display run time and exit
            stopwatch.Stop();
            Console.WriteLine("\nDone.");
            Console.WriteLine("Time elapsed: {0:0.0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"End timestamp {DateTime.UtcNow.ToString("O")}");
            //Console.ReadKey();
        }
    }
}

// 2190855 too high
