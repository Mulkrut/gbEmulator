public class CPU
{

    //Impletemention of the registers and flags.
    //Followed by adding the operations based on http://gameboy.mongenel.com/dmg/opcodes.html
    // and https://github.com/BotRandomness/CODE-DMG/blob/master/src/CPU.cs#L2235

    // 8-bit registers
    public byte A, B, C, D, E, H, L, F;

    // 16-bit registers
    public ushort PC;   //program counter
    public ushort SP;   //stack pointer


    //Idk yet
    public bool IME; //interrupt master enable flag
    private bool halted;
    private MMU mmu; //memory-management unit object


    //Init
    public CPU(MMU mmu)
    {
        A = B = C = D = E = H = L = F = 0;
        PC = 0x0000;
        SP = 0x0000; //Boot Rom will set this to 0xFFFE
        IME = false;

        this.mmu = mmu;

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


    //Operations and their blocks based on https://gbdev.io/pandocs/CPU_Instruction_Set.html

    //Block 0


    //Block 1 - 8-bit register-to-register loads

    private byte ld8(byte a, byte value) 
    {
        //TODO: Implement the exception for ld[hl],[hl]
        byte r = //(s=r,n,(HL) || d=r,(HL))

        SetZFlag(false);
        SetNFlag(true);

        return r;
    }

    //Block 2 - 8-bit arithmetic

    private byte Add8(byte a, byte b)
    {
        int result = a + b
        byte r = (byte)result; //converting from int to byte

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(((a & 0x0F) + (b & 0x0F)) > 0x0F); //0x0F = 00001111
        SetCFlag(result > 0xFF);

        return r;
    }
    
    private byte Adc8(byte a, byte b)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int result = a + b + carryIn;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(((a & 0x0F) + (b & 0x0F) + carryIn) > 0x0F);
        SetCFlag(result > 0xFF);

        return r;
    }
    
    private byte Sub8(byte a, byte b)
    {
        int result = a - b
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < (b & 0x0F));
        SetCFlag(a < b);

        return r;
    }

    private byte Sbc8(byte a, byte b)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int result = a - b - carryIn;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < ((b & 0x0F) + carryIn));
        SetCFlag(a < b + carryIn);

        return r;
    }

    private byte And8(byte a, byte b)
    {
        int result = a & b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(true);
        SetCFlag(false);

        return r;
    }

    private byte Or8(byte a, byte b)
    {
        int result = a | b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(false);

        return r;
    }

    private byte Xor8(byte a, byte b)
    {
        int result = a ^ b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(false);
        return r;
    }

    //Uses to compare, like subtract except you dont return a
    private void Cp8(byte a, byte b)
    {
        int result = a - b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < (b & 0x0F));
        SetCFlag(a < b);
    }

    private byte Inc8(byte a) 
    {
        int result = a++;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag((a & 0x0F) == 0x0F);

        return r;
    }

    private byte Dec8(byte a) 
    {
        int result = a--;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag((a & 0x0F) == 0x0F);

        return r;
    }

    //Block 3


    //imm8
    private byte FetchByte() 
    {
        byte value = _bus.ReadByte(PC);
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
}