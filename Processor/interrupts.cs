public class InterruptManager
{

    public bool IME; //interrupt master enable flag

    public bool IME_EnableScheduled = false;
    public bool IME_DisableScheduled = false;

    public bool imeEnablePending;
    private int imeEnableDelay = 0;

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
        if (withDelay) imeEnableDelay = 1;
        else IME = true;
    }

    public void DisableInterrupts()
    {
        imeEnableDelay = 0;
        IME = false;
    }

    public void OnInstructionFinished()
    {
        if (imeEnableDelay > 0)
        {
            imeEnableDelay--;
            if (imeEnableDelay == 0)
                IME = true;
        }
    }

    public void RequestInterruption(byte interruption)
    {
        IF |= (byte)(1 << interruption);
    }

    //getting cast error here
    public void ClearInterruption(int interruption)
    {
      IF = (byte)(IF & ~(1 << interruption));
    }

    public bool InterruptPending()
    {
        return (IE & IF & 0x1F) != 0;
    }
    public int GetInterruptType()
    {
        byte pending = (byte)(IE & IF & 0x1F);

        for (int i = 0; i < 5; i++)
        {
            if ((pending & (1 << i)) != 0)
                return i;
        }

        return -1;
    }




    public ushort GetInterruptVector(int activeInterrupt)
    {
        switch (activeInterrupt)
        {
            case 0: return 0x0040;
            case 1: return 0x0048;
            case 2: return 0x0050;
            case 3: return 0x0058;
            case 4: return 0x0060;
            default: return 0x0000;
        }
    }


  //beyond the scope of what i want to learn, taken from the FrozenBoy
    public bool IsHaltBug()
    {
      return !IME && InterruptPending();
    }

}
