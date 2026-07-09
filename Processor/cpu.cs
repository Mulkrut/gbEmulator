public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/
    private readonly Bus bus;

    private int executeTimer = 0;

    //Init
    public CPU(Bus bus)
    {
        A = B = C = D = E = H = L = F = 0;
        PC = 0x0000;
        SP = 0x0000; //Boot Rom will set this to 0xFFFE
        IME = false;

        this.bus = bus;

        Console.WriteLine("CPU init");
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



}