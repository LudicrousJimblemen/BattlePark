# Battle Park

This is a silly game about amusement parks, explosives, and the sale of ambiguously ethical "food." It's not done, and likely never will be, but it's still a fun project for now.

# How to Play

To play a game, you must join a server and everyone in the server should be ready, at which point the game will start. From there, you can do essentially nothing useful or entertaining. A large majority of the effort in this project has gone into creating network code rather than focusing on the game itself, so the gameplay is rather lacking.

## Camera Controls

- <kbd>W</kbd> - moves the camera forward
- <kbd>S</kbd> - moves the camera backwards
- <kbd>A</kbd> - moves the camera left
- <kbd>D</kbd> - moves the camera right
- <kbd>space</kbd> - moves the camera up
- <kbd>shift</kbd> - moves the camera down
- <kbd>Q</kbd> - rotates the camera anti/counter-clockwise
- <kbd>E</kbd> - rotates the camera clockwise

## Object Controls

- <kbd>1</kbd> - summon a new path to place down
- <kbd>2</kbd> - summon a new sculpture to place down
- <kbd>Z</kbd> - rotates the currect object anti/counter-clockwise
- <kbd>X</kbd> - rotates the current object clockwise
- <kbd>ctrl</kbd> - allows the object to be placed in the air while held down
- <kbd>esc</kbd> - cancels placement of the current object

# Starting a server

To start a server, one can simply run the server executable. By default, it hosts on port `6666`, and supports two players. By passing `-port` and `-usercount` as command-line arguments, you can change these values, like so:

```bash
C:\Users\SimulacrumGuy\Desktop\BattlePark\Server>BattlePark.Server.exe -port 9999 -usercount 4
```

- `-port` must be in between 0 and 65535.
- `-usercount` must be 1, 2, or 4.
