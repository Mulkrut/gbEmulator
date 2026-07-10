public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/

    public Bus bus;
    public Timer timer;
    public InterruptManager intManager;
    public GPU gpu;

    private int executeTimer = 0;

    //Init
    public CPU(Bus bus)
    {
        Console.WriteLine("CPU init");

        A = B = C = D = E = H = L = F = 0;
        PC = 0x0000;
        SP = 0x0000; //Boot Rom will set this to 0xFFFE
        IME = false;

        this.bus = bus;
        this.timer = timer;
        this.intManager = intManager;
        this.gpu = gpu;
        CPUResetRegisters();
        
        this.intManager.IME = false;

    }

    public void Step()
    {

    }

    //Main loop
    public void executeNext()
    {
        // Only want to execute very 4th tick
        executeTimer++;
        if (executeTimer < 4) return;
        else 
        {
            executeTimer = 0;
        }

        //Interrupt logic here




    }

    //move to registers file?
    public void CPUResetRegisters()
    {
        AF = 0x01B0;
        BC = 0x0013;
        DE = 0x00D8;
        HL = 0x014d;
        PC = 0x100;
        SP = 0xFFFE;
    }

}