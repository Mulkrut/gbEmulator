public class InterruptManager
{

    public bool IME; //interrupt master enable flag

    public bool IME_EnableScheduled = false;
    public bool IME_DisableScheduled = false;

    public bool imeEnablePending;

    //just doing as my references, might change to bools
    public int pendingEnable = -1;
    public int pendingDisable = -1;

    public byte IE { get; set; }
    public byte IF { get; set; }

    // IE = granular interrupt enabler. When bits are set, the corresponding interrupt can be triggered
    // IF = When bits are set, an interrupt has happened
    // their bit structure:
    // Bit position
    // 0   Vblank 
    // 1   LCD
    // 2   Timer 
    // 3   Serial Link 
    // 4   Joypad 

    //ISR addresses: (not used atm, just kept as a reference)
    public List<ushort> ISR_Address = new List<ushort>
    {
        { 0x0040 },    // Vblank
        { 0x0048 },    // LCD Status
        { 0x0050 },    // TimerOverflow
        { 0x0058 },    // SerialLink
        { 0x0060 }    // JoypadPress
    };


    public void EnableInterrupts(bool withDelay)
    {
        if (withDelay) imeEnablePending = true;
        else IME = true;
    }

    public void DisableInterrupts()
    {
        imeEnablePending = false;
        IME = false;
    }

    public void OnInstructionFinished()
    {
        if (imeEnablePending)
        {
            IME = true;
            imeEnablePending = false;
        }
    }

    public void RequestInterruption(byte interruption)
    {
        IF |= (byte)(1 << interruption);
    }

    //getting cast error here
    public void ClearInterruption(byte interruption)
    {
        IF &= (byte)~((0x01 << interruption));
    }

    public int getInterruptType()
    {
        int interruptType = -1;
        byte pending = (byte)(IE & IF & 0x1F);

        //goes through bit 0 - 4 of IF to find the type
        for (byte i = 0; i < 5; i++)
        {
            if ((pending & (1 << i)) == 0x01)
            {
                interruptType = i;
                break;
            }
        }
        return interruptType;
    }

    public bool InterruptPending()
    {
        return (IE & IF & 0x1F) != 0;
    }


    public ushort GetInterruptVector(int activeInterrupt)
    {
        if (activeInterrupt == 0)      return 0x0040;
        else if (activeInterrupt == 1) return 0x0048;
        else if (activeInterrupt == 2) return 0x0050;
        else if (activeInterrupt == 3) return 0x0058;
        else if (activeInterrupt == 4) return 0x0060;
        else return 0x0000;
    }


    //beyond the scope of what i want to learn, taken from the FrozenBoy
    public bool IsHaltBug() => (IE & IF & 0x1f) != 0 && !IME;
}
