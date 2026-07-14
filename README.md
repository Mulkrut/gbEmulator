# gbEmulator
A project for learning the fundamentals of how a gameboy works.


My main goal for starting this project was to become a better programmer.
During the process my use of AI was kept to a minimal.

While i am not expecting a perfect emulator that can compare to the ones already available i set myself the goal of being able to
run simpler games that like tetris and passing most if not all of Blargg's Gameboy hardware test ROMs.

The project is split up in the following parts in order of my process:

CPU
    
    [X]     Registers and flags
    [X]     Functions to read and execute OPCodes
    [ ]     Interrupts
    [ ]     Timers
    [ ]     Bus for reading the cartride 
    [ ]     Cartride loading and header reading

GPU,
PPU,
Joystick IO control,
Sound.


Sources for learning:
https://cturt.github.io/cinoop.html
http://gameboy.mongenel.com/dmg/opcodes.html
https://gbdev.io/pandocs/
https://jeremybanks.github.io/0dmg/2018/05/23/getting-started.html
http://marc.rawer.de/Gameboy/Docs/GBCPUman.pdf
https://imrannazar.com/series/gameboy-emulation-in-javascript/input
https://gekkio.fi/files/gb-docs/gbctr.pdf
https://github.com/rvaccarim/FrozenBoy - Frozenboy C# Emulator
https://github.com/davidwhitney/CoreBoy - Coreboy C# Emulator
https://rgbds.gbdev.io/docs/v1.0.1/gbz80.7

