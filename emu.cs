using System;

public class Emu
{
    public byte[]? GameRom;
    public byte[]? BootRom;

    public void Run(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: gbEmulator <rom file>");
            return;
        }

        while (true)
        {
            break;
        }
    }

    /**
    TODO:
    private Cartridge LoadCartridge(string path);
    private void Initialize(string romPath);
    private void MainLoop();
    **/
}