public class Timers
{
    //https://github.com/Ashiepaws/GBEDG/blob/master/timers/index.md
    //https://gbdev.io/pandocs/Timer_and_Divider_Registers.html
    //mostly gonna use the gbdev

    private readonly InterruptManager intManager;

    public Timers(InterruptManager intManager)
    {
        this.intManager = intManager;
        //speedmode when gbc
    }

    private static readonly int[] FreqToBit = [9, 3, 5, 7];


    public byte DIV { get; set; }   // FF04
    public byte TIMA { get; set; }          // FF05
    public byte TMA          // FF06
    {
        get => TIMA;
        set
        {
            if (ticksSinceOverflow < 5)
            {
                TIMA = value;
                overflow = false;
                ticksSinceOverflow = 0;
            }
        }
    }

    public byte TAC { get; set; }           // FF07

    private bool overflow;
    private int ticksSinceOverflow;

    private int divCounter; //divider register
    private int timaCounter;
    //Note: The divider is affected by CGB double speed mode, and will increment at 32768Hz in double speed.

    private bool previousBit;

    public byte Read(ushort address)
    {
        //return the type of timer it is
        if      (address == 0xFF04) return DIV;
        else if (address == 0xFF05) return TIMA;
        else if (address == 0xFF06) return TMA;
        else if (address == 0xFF07) return TAC;
        else return 0xFF;
    }

    public void Write(ushort address, byte value)
    {
        //depending on address write the value to said timer
        switch (address)
        {
            case 0xFF04:
                DIV = 0;
                divCounter = 0;
                break;
            case 0xFF05:
                TIMA = value;
                break;
            case 0xFF06:
                TMA = value;
                break;
            case 0xFF07:
                TAC = (byte)(value & 0x07);
                break;
        }
    }

    public void Tick()
    {
        UpdateDiv((divCounter + 1) & 0xFFFF);
        if (overflow == false) return;

        ticksSinceOverflow++;
        if (ticksSinceOverflow == 4)
        {
            intManager.RequestInterruption(2); //2 = timer
        }

        if (ticksSinceOverflow == 5)
        {
            TIMA = TMA;
        }
        if (ticksSinceOverflow == 6)
        {
            TIMA = TMA;
            overflow = false;
            ticksSinceOverflow = 0;
        }
    }


    private void UpdateDiv(int newDivCount)
    {
        divCounter = newDivCount;
       
        int bitPos = FreqToBit[TAC & 0b11];
        //CBG addition bitPos <<= speedmode.

        bool bit = (DIV & (1 << bitPos)) != 0;
        bit &= (TAC & (1 << 2)) != 0;

        if (!bit && previousBit)
        {
            IncrementTima();
        }
        previousBit = bit;
    }

    public void TimerStep(int cycles)
    {
        divCounter+= cycles;
        while (divCounter >= 256)
        {
            divCounter -= 256;
            DIV++;
        }

        if (!TimerEnabled()) return;

        timaCounter += cycles;
        int threshold = GetTimaClockCycles();

        while (timaCounter >= threshold)
        {
            timaCounter -= threshold;
            IncrementTima();
        }
    }

    public void IncrementTima()
    {

        TIMA++;
        int timaChanged = TIMA % 256;
        if (timaChanged == 0)
        {
            TIMA = 0;
            overflow = true;
            ticksSinceOverflow = 0;
        }


    }

    //https://gbdev.io/pandocs/Timer_and_Divider_Registers.html#ff07--tac-timer-control
    //
    public int GetTimaClockCycles()
    {
        if ((TAC & 0b11) == 0b00)        return 256 * 4;
        else if ((TAC & 0b11) == 0b01)   return 4 * 4;
        else if ((TAC & 0b11) == 0b10)   return 16 * 4;
        else if ((TAC & 0b11) == 0b11)   return 64 * 4;
        else return 256 * 4; //assumed default
    }

    private bool TimerEnabled()
    {
        return (TAC & 0b100) != 0;
    }
}

