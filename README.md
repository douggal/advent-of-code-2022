# Advent of Code 2022

My solutions to the Advent of Code programming contest December 2022.

## What is this project about?

Advent of Code website:  [Advent of Code](https://adventofcode.com/2022)

Solutions are in C# and cross platform .NET 7 framework unless otherwise noted.

My goals for this year's AoC are to have fun, and apply
the functional programming techniques in C# 11.  I'll make
full use of .NET's built-in data type collections, LINQ extension methods to operate on them,
and for graph and digraph traversals the [QuikGraph](https://github.com/KeRNeLith/QuikGraph) graph algorithmm library.

1. Day  1:  Calorie Counting
2. Day  2:  Rock Paper Scissors
3. Day  3:  Rucksack Reorganization
4. Day  4:  Camp Cleanup
5. Day  5:  Supply Stacks
6. Day  6:  Tuning Trouble
7. Day  7:  No Space Left On Device
8. Day  8:  Treetop Tree House
9. Day  9:
10. Day 10:
11. Day 11:
12. Day 12:  Hill Climbing Algorithm
13. Day 13:
14. Day 14:


## Notes

### Day 1

Don't forget the last line of input if not blank!

### Day 2

Took me quite a while to realize a draw is each player
throwing the same item but must account for differnt values
 they have in each column of the data.  In C# .NET tuples
 can be either reference values (new Tuple<...>) or a value
 type, e.g., "('A',1)".  Need value types for Dictionary
 keys.

### Day 3

String splitting and slicing is straightforward, but it pays to read the docs and put in some assertions
to catch errors.  Ditto for the System.Collections types.  
Note that Intersection is a LINQ method while IntersectWith
is [HashSet\<T>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0) method.  
Making good use of C# .NET System.Collections, but I'm using mutable implementations.

### Day 4

[Looping over a range c# - for loop or use a IEnumerable.Range?](https://stackoverflow.com/questions/915745/thoughts-on-foreach-with-enumerable-range-vs-traditional-for-loop).
As the post comments point out in C# a for loop may the better choice for looping over a contiguous range of integers.  The clarity comes from long time use in C.

### Day 5

I underestimated this one.  Sounded straightforward until I started parsing the picture part of the data needed to initialize the stacks.  Result is messy.  Pays to think about it for while.  Should have just hard coded the input picture data to save time intead of trying
parse it in a generic manner.

### Day 6

Straightforward.  Trying to use FP but finding for loops and mutable variables often seems
efficient and easier to read.  Maybe it's the language, and C# is not really geared for FP.  To loop over
a array and have the index of each character available a for loop might be clearer than 
```csharp
foreach (var e in ins.Skip(3).Select((x, i) => new { Value = x, Index = i }))
```

### Day 7

Couldn't solve this one in same day. The idea worked, but it was buggy and not easy to find problems.

I modelled the folder structure as a dictionary of paths,
one entry for each folder/directory and each entry contains the sum of file sizes
for that directory (not including its subfolders).  Once that is built, then it should
be just a matter of adding up each folder's size along with sum of its subfolders sizes.  

Tried out new to C# version 11 [list pattern matching feature](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching).
and [IAmTimCorey](https://www.youtube.com/watch?v=SztvGBv8uVM).  Very nice. 

Surprised the linting tool/features didn't catch this mistake: I had following an if stmt `String.Concat(wd, f);`
and should have been an assignment stmt, `wd = String.Concat(wd, f);`.
Just silence on it (Visual Studio 2022 for Mac).

### Day 8

Finally completed both parts on the 10th.  Had a serious logic error, and several smaller skip/take endnpoint errors.
I did not see the logic error for a long time.

To make all the overhead Linq introduces performant, Linq features lazy evaluation.  This can
take some getting used to.  In sending string items from a Linq query for output
to the screen using String.Join for concatenation I had to force an evaluation before
I could get the expected output.

[When does Linq shine over a for loop?](https://stackoverflow.com/questions/37361331/how-to-iterate-a-loop-every-n-items)
Answer seems to be for C# to use both and choose according to situation.
Both worked together well for this problem.


### Day 12

Model the data as a digraph in a Dictionary<int, int[]> and use QuikGraph
to solve for shortest path.  Almost had a quick win.  I had errors in building the
graph and took a while to find them all and fix.

