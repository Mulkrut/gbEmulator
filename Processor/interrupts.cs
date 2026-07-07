public class Interrupt
{


    public bool IME; //interrupt master enable flag

    private byte IE { get; set; };
    private byte IF { get; set; };

    // IE = granular interrupt enabler. When bits are set, the corresponding interrupt can be triggered
    // IF = When bits are set, an interrupt has happened
    // their bit structure:
    // Bit position
    // 0   Vblank 
    // 1   LCD
    // 2   Timer 
    // 3   Serial Link 
    // 4   Joypad 

    //ISR addresses:
    public List<u16> ISR_Address = new List<u16>
    {
        { 0x0040 },    // Vblank
        { 0x0048 },    // LCD Status
        { 0x0050 },    // TimerOverflow
        { 0x0058 },    // SerialLink
        { 0x0060 };  // JoypadPress,
    }

    timerInterrupt()
    {
        IF |= (1 << 3);


        //endgoal:
        IME = true;
        Push16(PC);

        switch (ISR)(
            PC = 0x0040
        )
        doHandleRoutine();
        Pop16(PC);
        IME = True;
    }

    
}
