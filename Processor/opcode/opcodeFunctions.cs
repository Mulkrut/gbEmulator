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

    private void Stop()
    {
        Fetch8();
        timer.DIV = 0;
        state = InstructionState.Stopped;
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

    private byte ExecuteCBOpcode(byte valye)
    {
        return 0x01; //temp
    }

    //Decimal adjust after addition, makes it a number from 0 - 9
    private void Daa()
    {   
        int adjustment = 0;
        bool carry = GetCFlag();

        if (!GetNFlag()) // after addition
        {
            if (GetHFlag() || (A & 0x0F) > 0x09)
            {
                adjustment |= 0x06;
            }

            if (carry || A > 0x99)
            {
                adjustment |= 0x60;
                carry = true;
            }

            A = (byte)(A + adjustment);
        }
        else // after subtraction
        {
            if (GetHFlag())
            {
                adjustment |= 0x06;
            }

            if (carry)
            {
                adjustment |= 0x60;
            }

            A = (byte)(A - adjustment);
        }

        SetZFlag(A == 0);
        SetHFlag(false);
        SetCFlag(carry);
    }

}