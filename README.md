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
7. Day  7:  

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
Try out C# version 11 [list pattern matching feature](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching).
and [IAmTimCorey](https://www.youtube.com/watch?v=SztvGBv8uVM).

