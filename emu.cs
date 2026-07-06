

class EMU 
{

    public byte[] gameRom;
    public byte[] bootRom;


    public void EmuRun()
    {
        //checks if a rom file is inputted
        if (argc < 2) return -1;
        //loads in the rom, error if failure
        if (!cart_load(argv[1])) return -2;
   

        //init

        //Main loop
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
        }


    }
}