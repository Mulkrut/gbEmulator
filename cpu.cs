public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/


    //
    public bool IME; //interrupt master enable flag
    

    private readonly Bus bus;

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


    //Operations and their blocks based on https://gbdev.io/pandocs/CPU_Instruction_Set.html



}