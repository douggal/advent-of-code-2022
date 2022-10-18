@main
def main(): Unit = {
  println("Hello world!")

  // created ...
  // https://adventofcode.com/2022/day/1

  println(s"--- Day 1: ... ---")

  // Input Data File
  //val filename = "./input/Day1Input.txt"
  val filename = "./input/placeholder.txt"

  // TODO: read input

  val readInputData = () => {
    val source = io.Source.fromFile(filename)
    for {
      line <- source.getLines().toVector
    } yield
      line
  }

  val input = readInputData()
  for (line <- input) {
    println(line)
  }

  // Quality control
  println("------------------------------------")
  println("Data Quality Control:")
  println(s"Start Timestamp ${java.time.ZonedDateTime.now()}")
  println(s"Input file name: $filename")
  println(s"Each line is a: ${input.getClass}")
  println(s"Number lines: ${input.length}")
  println(s"Number items per line: ${input.head.count(_ => true)}")
  println(s"Input first line: ${input.head}")
  //println(s"Input last line: ${input.tail.last}")
  println("------------------------------------")

  // Part One
  println(s"Day 1 Part 1 TBD")

  // Part Two

  println(s"Day 1 Part 2  [TBD]")

  println(s"End at ${java.time.ZonedDateTime.now()}")
}