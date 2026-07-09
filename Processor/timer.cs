public class Timers
{
    //https://github.com/Ashiepaws/GBEDG/blob/master/timers/index.md
    //https://gbdev.io/pandocs/Timer_and_Divider_Registers.html
    //mostly gonna use the gbdev

    public byte DIV { get; private set; }   // FF04
    public byte TIMA { get; set; }          // FF05
    public byte TMA { get; set; }           // FF06
    public byte TAC { get; set; }           // FF07
    
    private int divCounter; //divider register
    private int timaCounter;
    //Note: The divider is affected by CGB double speed mode, and will increment at 32768Hz in double speed.

    public byte Read(ushort Address)
    {
        //return the type of timer it is
    }

    public void Write(ushort address, byte value)
    {
        //depending on address write the value to said timer
    }

    public void Step(int cycles)
    {

    }

    public void IncrementTima()
    {

        //logic for overflow
        if (TIMA >= 0xFF)
        {
            TIMA = TMA;
            //request interrupt
            RequestInterruption(int 2); //2 = timer, maybe not correct? to dix when i change in the int. file
        }
        else TIMA++;
    }

    //https://gbdev.io/pandocs/Timer_and_Divider_Registers.html#ff07--tac-timer-control
    public int GetTimaClockSelect()
    {
        if (TAC & 0b11) == 0b00)        return 4096;
        else if (TAC & 0b11) == 0b01)   return 262144;
        else if (TAC & 0b11) == 0b10)   return 65536;
        else if (TAC & 0b11) == 0b11)   return 16384;
    }

    private bool TimerEnabled()
    {
        return (TAC & 0b100) != 0;
    }
}

