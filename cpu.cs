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

    //Block 0
    private int Nop()
    {
        return 4;
    }

    private void JpA16()
    {
        ushort addr = FetchWord();
        PC = addr;
    }


    //Block 1 - 8-bit register-to-register loads

    private byte ld8(byte a, byte value) 
    {
        //TODO: Implement the exception for ld[hl],[hl]
        byte r = value;//(s=r,n,(HL) || d=r,(HL))

        return r;
    }

  
    //Block 3
    //imm8
    private byte FetchByte() 
    {
        byte value = bus.ReadByte(PC);
        PC++;
        return value;
    }

    //like above but returns 2 bytes added together (ex 0x34 + 0x12 = 0x1234)
    private ushort FetchWord()
    {
        byte low = FetchByte();
        byte high = FetchByte();
        return (ushort)(low | (high << 8));
    }


        /*
    TODO:
    private int ExecuteOpcode(byte opcode);
    private int ExecuteCBOpcode(byte opcode);
    private void PushWord(ushort value);
    private ushort PopWord();
    */
}