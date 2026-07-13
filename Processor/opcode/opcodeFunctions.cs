public partial class CPU
{

    //imm8
    private byte Fetch8() 
    {
        byte value = bus.ReadByte(PC);
        PC++;
        return value;
    }

    //like above but returns 2 bytes added together (ex 0x34 + 0x12 = 0x1234)
    private ushort Fetch16()
    {
        byte lo = Fetch8();
        byte hi = Fetch8();
        return (ushort)(lo | (hi << 8));
    }

    public void Push16(ushort value)
    {
        SP--;
        bus.WriteByte(SP, (byte)(value >> 8));
        SP--;
        bus.WriteByte(SP, (byte)(value & 0x00FF));
    }

    public ushort Pop16()
    {
        byte lo = bus.ReadByte(SP);
        SP++;
        byte hi = bus.ReadByte(SP);
        SP++;
        return (ushort)(lo | (hi << 8));
    }

    public void PushAF() //i do not like you F
    {
        SP--;
        bus.WriteByte(SP, A);
        SP--;
        bus.WriteByte(SP, (byte)(F & 0xF0));
    }

    public ushort PopAF() //stupid F
    {
        byte lo = bus.ReadByte(SP);
        SP++;
        byte hi = bus.ReadByte(SP);
        SP++;
        return (ushort)((lo & 0xF0) | (hi << 8));

    }

    private void Stop()
    {
        Console.Write("Stop called, not implemented yet");
    }

    private void Halt()
    {
        if (!intManager.IME && intManager.InterruptPending())
        {
            haltBug = true;
            state = InstructionState.Fetch;
        }
        else
        {
            state = InstructionState.Halted;
        }
    }

    //executes all cb opcode functions
    private int ExecuteCBOpcode(byte opcode)
    {
        int target = opcode & 0b111;
        int y = (opcode >> 3) & 0b111;
        int group = (opcode >> 6) & 0b11;

        byte value = ReadR8(target);
        bool isHL = (target == 6);

        switch (group)
        {
            case 0:
                value = y switch
                {
                    0 => Rlc(value),
                    1 => Rrc(value),
                    2 => Rl(value),
                    3 => Rr(value),
                    4 => Sla(value),
                    5 => Sra(value),
                    6 => Swap(value),
                    7 => Srl(value),
                    _ => throw new InvalidOperationException($"Invalid CB opcode 0x{opcode:X2}")
                };
                WriteR8(target, value);
                return isHL ? 16 : 8;

            case 1:
                TestBit(y, value);
                return isHL ? 12 : 8;

            case 2:
                value = (byte)(value & ~(1 << y));
                WriteR8(target, value);
                return isHL ? 16 : 8;

            case 3:
                value = (byte)(value | (1 << y));
                WriteR8(target, value);
                return isHL ? 16 : 8;

            default:
            throw new InvalidOperationException($"Invalid CB opcode 0x{opcode:X2}");
        }
    }
    
    //Decimal adjust after addition, makes it a number from 0 - 9
    //https://forums.nesdev.org/viewtopic.php?t=15944
    private void Daa()
    {   
        bool n = GetNFlag();
        bool h = GetHFlag();
        bool c = GetCFlag();

        if (!n)
        {
            if (A >= 0x9F)
                c = true;

            if ((A & 0x0F) >= 0x0A)
                h = true;
        }

        int adjustment = 0;
        if (h) adjustment |= 0x06;
        if (c) adjustment |= 0x60;

        A = (byte)(n ? A - adjustment : A + adjustment);

        SetZFlag(A == 0);
        SetHFlag(false);
        SetCFlag(c);
    }
}