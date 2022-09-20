# GTA AFK

This is a simple script that will allow you to AFK in GTA Online.

## How to use

1. Download the latest release from the [releases page](https://github.com/dablenparty/gta-afk/releases).
2. Extract the zip file.
3. Run the `GtaAfk.exe` file.

## How it works

The script will randomly choose 1-2 keys from `W, A, S, D, Shift` and hold them for 1-4 seconds. It will never hold more
than 2 keys at a time, hold `Shift` on its own, or press conflicting keys (e.g. `W` and `S`) together.

## Use with other games

This script is designed to work with GTA Online, but it should work with any game that uses the same key bindings. If you
want to use it with another game, you can launch the program from the command line and pass the name of the games window
that you want to use (e.g., `GtaAfk.exe "Red Dead Redemption II"` would look for the RDR2 window).
