# Conway's Game of Life

A simple CLI implementation of Conway's Game of Life in .NET.

## Console Options

### -f, --file

*Required*

A path to an input text file to seed the game.
See [here](src/ConwaysGameOfLife/TestData/Initialization.txt)
for an example format.

`Life.exe -f path/to/file.txt`

### -c, --count

*Optional*

The maximum number of generations to run. Defaults to unlimited.

`Life.exe -f path/to/file.txt -c 50`

### -l, --length

*Optional*

The length of each generation, in milliseconds. Defaults to 500 (half a second).

`Life.exe -f path/to/file.txt -l 250`