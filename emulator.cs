using System;

public class Emulator
{
    public byte[]? GameRom;
    public byte[]? BootRom;
    public const int ClockSpeed = 4194304;

    public CPU cpu;
    public GPU gpu;
    public BUS bus;
    public DMA dma;
    public InterruptManager intManager;
    public Timer timer;
    
    //taken from the frozenboy, insert other variables to the functions
    public Emulator()
    {
        intManager = new InterruptManager();
        timer = new Timer(intManager);
        gpu = new GPU(intManager, gbOptions.Palette);
        // joypad = new Joypad(intManager);
        // dma = new Dma();
        // serial = new SerialLink(intManager);
        bus = new BUS(timer, intManager, gpu, joypad, dma, serial);
        cpu = new CPU(bus, timer, intManager, gpu);
    }


    //delete?
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


    public int Step()
    {
        timer.tick();
        cpu.executeNext();
        

        //todo:
        // dma.tick();
        // serial.tick();
        // gpu.tick();
        return 1;
    }

    /*
    TODO:
    private Cartridge LoadCartridge(string path);
    private void Initialize(string romPath);
    private void MainLoop();
    */
}