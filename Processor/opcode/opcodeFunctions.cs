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
        byte f = (bus.ReadByte(SP));
        SP++;
        A = bus.ReadByte(SP);
        SP++;

        F = (byte)(f & 0xF0);
        return (ushort)((A << 8) | F);
    }

    private void Stop()
    {
        //Console.Write("Stop called");
        Fetch8();
        //state = InstructionState.Stopped;
    }

    private void Halt()
    {
        if (intManager.IsHaltBug())
        {
            haltBug = true;
            state = InstructionState.Fetch;
        }
        else
        {
            state = InstructionState.Halted;
        }
    }


    private byte BitRes(byte byteToChange, byte value)
    {
        byte r = (byte)(byteToChange & ~(1 << value));

        return r;
    }
    
    private byte BitSet(byte byteToChange, byte value)
    {
        byte r = (byte)(byteToChange | (1 << value));

        return r;
    }

    //Decimal adjust after addition, makes it a number from 0 - 9
    //https://forums.nesdev.org/viewtopic.php?t=15944
    private void Daa()
    {   
        bool n = GetNFlag();
        bool h = GetHFlag();
        bool c = GetCFlag();

        int adjustment = 0;

        if (n) // After subtraction
        {
            if (c)
            {
                adjustment |= 0x60;
                c = true;
            }
            if (h)
            {
                adjustment |= 0x06;
                c = true;
            }
        }
        else // After addition
        {
            if (c || A > 0x99)
            {
                adjustment |= 0x60;
                c = true;
            }
            if (h || (A & 0xF) > 0x9)
            {
                adjustment |= 0x06;
            }
        }

        A = (byte)(n ? A - adjustment : A + adjustment);

        SetZFlag(A == 0);
        SetHFlag(false);
        SetCFlag(c);
    }
}