public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/



    //Impletemention of the registers and flags.
    //Followed by adding the operations based on http://gameboy.mongenel.com/dmg/opcodes.html
    // and https://github.com/BotRandomness/CODE-DMG/blob/master/src/CPU.cs#L2235



    // 8-bit registers
    public byte A, B, C, D, E, H, L, F;

    //Declaring the flags location in the F register
    private const byte FLAG_Z = 0x80; // 1000 0000
    private const byte FLAG_N = 0x40; // 0100 0000
    private const byte FLAG_H = 0x20; // 0010 0000
    private const byte FLAG_C = 0x10; // 0001 0000

    // 16-bit registers
    public ushort PC;   //program counter
    public ushort SP;   //stack pointer

    public ushort AF
    {
        get => (ushort)((A << 8) | F);
        set => (A, F) = ((u8)(value >> 8), (u8)value);
    }
    public ushort BC
    {
        get => (ushort)((B << 8) | C);
        set => (B, C) = ((u8)(value >> 8), (u8)value);
    }
        public ushort DE
    {
        get => (ushort)((D << 8) | E);
        set => (D, E) = ((u8)(value >> 8), (u8)value);
    }
        public ushort HL
    {
        get => (ushort)((H << 8) | L);
        set => (H, L) = ((u8)(value >> 8), (u8)value);
    }



    //
    public bool IME; //interrupt master enable flag
    private bool halted;
    // private MMU mmu; //memory-management unit object

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

    //The flag F (ZNHC0000)
    //Zero, Negative, Half carry, Carry
    public bool GetZFlag() => (F & FLAG_Z) != 0;
    public bool GetNFlag() => (F & FLAG_N) != 0;
    public bool GetHFlag() => (F & FLAG_H) != 0;
    public bool GetCFlag() => (F & FLAG_C) != 0;

    public void SetZFlag(bool value)
    {
        if (value) F |= FLAG_Z;
        else F &= unchecked((byte)~FLAG_Z);
    }

    public void SetNFlag(bool value)
    {
        if (value) F |= FLAG_N;
        else F &= unchecked((byte)~FLAG_N);
    }

    public void SetHFlag(bool value)
    {
        if (value) F |= FLAG_H;
        else F &= unchecked((byte)~FLAG_H);
    }

    public void SetCFlag(bool value)
    {
        if (value) F |= FLAG_C;
        else F &= unchecked((byte)~FLAG_C);
    }


    public int Step()
    {
        //Next thing to do!
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
    private void HandleInterrupts();
    private void SetAF(ushort value);
    private void SetBC(ushort value);
    private void SetDE(ushort value);
    private void SetHL(ushort value);
    */
}