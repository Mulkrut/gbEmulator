using System;

public partial class CPU
{
    //Main document to learn how the cpu works:
    //https://robertovaccari.com/blog/2020_09_26_gameboy/

    public BUS bus;
    public Timer timer;
    public InterruptManager intManager;
    public GPU gpu;

    public enum InstructionState {
        Fetch, FetchPrefix, WorkPending,
        Interrupt_IF, Interrupt_IE, Interrupt_Push1, Interrupt_Push2, Interrupt_Jump,
        Halted, Stopped
    }

    public InstructionState state;
    private int executeTimer = 0;
    public bool haltBug;

    //Init
    public CPU(BUS bus)
    {
        Console.WriteLine("CPU init");

        A = B = C = D = E = H = L = F = 0;
        PC = 0x0000;
        SP = 0x0000; //Boot Rom will set this to 0xFFFE
        IME = false;

        this.bus = bus;
        this.timer = timer;
        this.intManager = intManager;
        this.gpu = gpu;
        CPUResetRegisters();
        
        this.intManager.IME = false;
        state = InstructionState.Fetch();

    }

    public void CpuStep()
    {

    }

    //Main loop
    public void executeNext()
    {
        // Only want to execute very 4th tick
        executeTimer++;
        if (executeTimer < 4) return;
        else 
        {
            executeTimer = 0;
        }

        //Interrupt logic here
        if (state == InstructionState.Fetch || state == InstructionState.Halted || InstructionState.Stopped)
        {
            //check for interrupts and set state to InstructionState.IF if true

            return;
        }

        switch (state)
        {
            case InstructionState.Interrupt_IF:
            case InstructionState.Interrupt_IE:
            case InstructionState.Interrupt_Push1:
            case InstructionState.Interrupt_Push2:
            case InstructionState.Interrupt_Jump:
                //handle interrupt
                return;
            case InstructionState.Halted when intManager.interruptRequested():
                state = InstructionState.Fetch;
                break;
        }

        if (state == InstructionState.Halted || state == InstructionState.Stopped)
        {
            return;
        }


        //main progressor for going through the opcode
        switch (state)
        {
            case InstructionState.Fetch:

                //checks for haltbug and moves the pointer
                if (intManager.IsHaltBug() == true) haltBug = false;
                else PC++;

                //maybe return
                break;



        }

    }


    //todo:
    private void HandleInterrupt()
    {
        switch (state)
        {
            case InstructionState.Interrupt_IF:

                break;
            case InstructionState.Interrupt_IE:
                break;
            case InstructionState.Interrupt_Push1:
                break;
            case InstructionState.Interrupt_Push2:
                break;
            case InstructionState.Interrupt_Jump:
                break;

        }
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

}