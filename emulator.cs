using System;
using System.IO;

public class Emulator
{
    public byte[]? GameRom;
    public byte[]? BootRom;
    public const int ClockSpeed = 4194304;

    public Cartridge cartridge;
    public CPU cpu;
    public GPU gpu;
    public BUS bus;
    public DMA dma;
    public InterruptManager intManager;
    public Timers timer;

    
    //taken from the frozenboy, insert other variables to the functions
    // public Emulator()
    // {
    //     intManager = new InterruptManager();
    //     timer = new Timers(intManager);
    //     cartridge = new Cartridge(romPath);
    //     // gpu = new GPU(intManager);
    //     // joypad = new Joypad(intManager);
    //     // serial = new SerialLink(intManager);
    //     bus = new BUS(cartridge, timer, intManager, gpu, dma); //add serial and joypad
    //     cpu = new CPU(bus, timer, intManager, gpu);
    //     dma = new DMA();
    //     dma.SetBus(bus);
    // }


    //method to find the rom and boots up the emulator
    public void Run(string[] args)
    {
        string romPath = null;

        //load rom from dragging on .exe file
        if (args.Length > 0 && File.Exists(args[0]))
        {
            romPath = args[0];
        }
        else //load from roms folder (only have 1 in for now)
        {
            string romFolder = Path.Combine(AppContext.BaseDirectory, "roms");
            if (File.Exists(romFolder)) romPath = romFolder;
        }

        if (romPath == null) 
        {
            Console.WriteLine("No ROM found.");
            return;
        }

        Console.WriteLine($"Loading ROM: {romPath}");

        Initialize(romPath);

        //main loop, maybe add a exit condition later
        while (true)
        {
            Step();
        }


    }

    public int Step()
    {
        //timer.TimerStep(); //idk what to do about this one
        cpu.CpuStep();
        
        //todo:
        // dma.tick();
        // serial.tick();
        // gpu.tick();
        return 1;
    }

    private void Initialize(string romPath)
    {
        intManager = new InterruptManager();
        timer = new Timers(intManager);
        cartridge = new Cartridge(romPath);
        bus = new BUS(cartridge, timer, intManager, gpu, dma);

        gpu = new GPU();
        dma = new DMA();
        dma.SetBUS(bus);

        cpu = new CPU(bus, timer, intManager, gpu);
    }
}