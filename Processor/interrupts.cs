public partial class CPU
{


    public bool IME; //interrupt master enable flag

    public bool IME_EnableScheduled = false;
    public bool IME_DisableScheduled = false;

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
        if (withDelay == true)
        {
            if (pendingEnable == -1) pendingEnable = 1;
        }
        else
        {
            pendingEnable = -1;
            IME = true;
        }
    
    }

    public void DisableInterrupts()
    {
        pendingEnable = -1;
        pendingDisable = -1;
        IME = false;
    }

    public void RequestInterruption(byte interruption)
    {
        IF |= ((byte)(1 << interruption));
    }

    public void ClearInterruption(byte interruption)
    {
        IF &= ~((byte)(1 << interruption));
    }

    public byte getInterruptType()
    {
        byte interruptType = 0x00;

        //goes through bit 0 - 4 of IF to find the type
        for (byte i = 0; i < 5; i++)
        {
            if (((IF >> i) & 0x01) == 0x01)
            {
                interruptType = i;
                break;
            }
        }
        return interruptType;
    }

    private void interruptStep()
    {
        //IF |= (1 << 3);

        byte interruptType = getInterruptType();


        //endgoal:
        IME = true;
        Push16(PC);

        //figure out a smarter way to do this with the list?
        if (interruptType == 0) PC = 0x0040;
        else if (interruptType == 1) PC = 0x0048;
        else if (interruptType == 2) PC = 0x0050;
        else if (interruptType == 3) PC = 0x0058;
        else if (interruptType == 4) PC = 0x0060;
        //else stop(); //Error

        //todo:
        //doHandleRoutine(interruptType);

        Pop16();
        DisableInterrupts();
    }

    //beyond the scope of what i want to learn, taken from the FrozenBoy
    public bool IsHaltBug() => (IE & IF & 0x1f) != 0 && !IME;
}
