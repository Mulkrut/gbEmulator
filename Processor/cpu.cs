using System;

public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/

    //todo: Consistency accrost T and M cycles

    public BUS bus;
    public Timers timer;
    public InterruptManager intManager;
    public GPU gpu;

    public enum InstructionState {
        Fetch, FetchPrefix, WorkPending,
        Interrupt_IF, Interrupt_IE, Interrupt_Push1, Interrupt_Push2, Interrupt_Jump,
        Halted, Stopped
    }

    public InstructionState state;
    private int executeTimer = 0;
    private int activeInterrupt = -1;
    public bool haltBug;

   

    //Init
    public CPU(BUS bus, Timers timer, InterruptManager intManager, GPU gpu)
    {
        Console.WriteLine("CPU init");

        this.bus = bus;
        this.timer = timer;
        this.intManager = intManager;
        this.gpu = gpu;

        A = B = C = D = E = H = L = F = 0;
        PC = 0x0000;
        SP = 0x0000; //Boot Rom will set this to 0xFFFE
        
        CPUResetRegisters();
        
        this.intManager.IME = false;
        state = InstructionState.Fetch;

    }

    public void CpuStep()
    {

        timer.TimerStep(1);

        // Only want to execute very 4th tick
        executeTimer++;
        if (executeTimer < 4) return;
        else 
        {
            executeTimer = 0;
            executeNext();
        }

    }

    //Main loop
    public void executeNext()
    {


        //Interrupt logic here
        if (state == InstructionState.Fetch)
        {
            if (intManager.IME && intManager.InterruptPending())
            {
                state = InstructionState.Interrupt_IF;
            }
        }
        else if (state == InstructionState.Halted)
        {
            if (intManager.InterruptPending())
            {
                state = intManager.IME ? InstructionState.Interrupt_IF : InstructionState.Fetch;
            }
        }
        else if (state == InstructionState.Stopped)
        {
            return;
        }

        switch (state)
        {
            case InstructionState.Interrupt_IF:
            case InstructionState.Interrupt_IE:
            case InstructionState.Interrupt_Push1:
            case InstructionState.Interrupt_Push2:
            case InstructionState.Interrupt_Jump:
                int cyclesForHandling = HandleInterrupt(); //long var name cause stupid compiler
                timer.TimerStep(cyclesForHandling);//mCycles therefor i may have to *4, compared to T cycles
                return;
            case InstructionState.Halted when intManager.InterruptPending():
                state = InstructionState.Fetch;
                break;
        }

        if (state == InstructionState.Halted || state == InstructionState.Stopped)
        {
            return;
        }

        TraceOpcode();
        byte opcode;

        //main progressor for going through the opcode
        if (haltBug) 
        {
            opcode = bus.ReadByte(PC);
            haltBug = false;
        }
        else
        {
            opcode = bus.ReadByte(PC++);
        }

        int cycles = ExecuteBaseOpcode(opcode);

        timer.TimerStep(cycles);
        intManager.OnInstructionFinished();

    }


    //returns the cycles each state takes.
    public int HandleInterrupt()
    {
        switch (state)
        {
            case InstructionState.Interrupt_IF:
                activeInterrupt = intManager.getInterruptType();

                if (activeInterrupt == -1)
                {
                    state = InstructionState.Fetch;
                    return 0;
                }
                state = InstructionState.Interrupt_IE;
                return 1;

            case InstructionState.Interrupt_IE:
                intManager.IME = false;
                state = InstructionState.Interrupt_Push1;
                return 1;

            case InstructionState.Interrupt_Push1:
                SP--;
                bus.WriteByte(SP, (byte)(PC >> 8));
                state = InstructionState.Interrupt_Push2;
                return 1;

            case InstructionState.Interrupt_Push2:
                SP--;
                bus.WriteByte(SP, (byte)(PC & 0x00FF));

                intManager.ClearInterruption((byte)activeInterrupt);
                state = InstructionState.Interrupt_Jump;
                return 1;

            case InstructionState.Interrupt_Jump:
                PC = (ushort)intManager.GetInterruptVector(activeInterrupt);
                activeInterrupt = -1;
                state = InstructionState.Fetch;
                return 1;

        }
        return 0;
    }

    //move to registers file?
    public void CPUResetRegisters()
    {
        AF = 0x01B0;
        BC = 0x0013;
        DE = 0x00D8;
        HL = 0x014d;
        PC = 0x100;
        SP = 0xFFFE;
    }



     //used to bugfix
    private bool traceEnabled = false;
    private string? lastTraceLine = null;
    
    private void TraceOpcode()
    {
        if (!traceEnabled) return;

        byte b0 = bus.ReadByte(PC);
        byte b1 = bus.ReadByte((ushort)(PC + 1));
        byte b2 = bus.ReadByte((ushort)(PC + 2));
        byte b3 = bus.ReadByte((ushort)(PC + 3));

        string line =
            $"A:{A:X2} F:{F:X2} B:{B:X2} C:{C:X2} D:{D:X2} E:{E:X2} H:{H:X2} L:{L:X2} " +
            $"SP:{SP:X4} PC:{PC:X4} PCMEM:{b0:X2},{b1:X2},{b2:X2},{b3:X2}";

        if (line != lastTraceLine)
        {
            Console.WriteLine(line);
            lastTraceLine = line;
        }
    }


}