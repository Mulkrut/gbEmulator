public partial class CPU
{
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
        set => (A, F) = ((byte)(value >> 8), (byte)(value & 0x0F));
    }
    public ushort BC
    {
        get => (ushort)((B << 8) | C);
        set => (B, C) = ((byte)(value >> 8), (byte)value);
    }
        public ushort DE
    {
        get => (ushort)((D << 8) | E);
        set => (D, E) = ((byte)(value >> 8), (byte)value);
    }
        public ushort HL
    {
        get => (ushort)((H << 8) | L);
        set => (H, L) = ((byte)(value >> 8), (byte)value);
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
}