using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        StartTrace();

    }

    public void CpuStep()
    {

        // Only want to execute very 4th tick
        executeTimer++;

        if (executeTimer < 4) return;

        executeTimer = 0;
        executeNext();


    }

    //Main loop
    public void executeNext()
    {
        int cycles = 0;

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
            else
            {
              return;
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
                cycles = HandleInterrupt() * 4;
                return;
        }

        if (state == InstructionState.Halted || state == InstructionState.Stopped)
        {
            return;
        }

        //creates the trace log for gameboy doctor
        LogState();

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

        cycles = ExecuteBaseOpcode(opcode);

        //timer.TimerStep(cycles); - pushed to emulator main loop
        intManager.OnInstructionFinished();

    }


    //returns the cycles each state takes.
    public int HandleInterrupt()
    {
        switch (state)
        {
            case InstructionState.Interrupt_IF:
                activeInterrupt = intManager.GetInterruptType();

                if (activeInterrupt == -1)
                {
                    state = InstructionState.Fetch;
                    return 0;
                }

                intManager.IME = false;
                state = InstructionState.Interrupt_IE;
                return 1; // first wait cycle

            case InstructionState.Interrupt_IE:
                state = InstructionState.Interrupt_Push1;
                return 1; // second wait cycle

            case InstructionState.Interrupt_Push1:
                SP--;
                bus.WriteByte(SP, (byte)(PC >> 8));   // high byte first
                state = InstructionState.Interrupt_Push2;
                return 1;

            case InstructionState.Interrupt_Push2:
                SP--;
                bus.WriteByte(SP, (byte)(PC & 0xFF)); // low byte second
                intManager.ClearInterruption(activeInterrupt);
                state = InstructionState.Interrupt_Jump;
                return 1;

            case InstructionState.Interrupt_Jump:
                PC = intManager.GetInterruptVector(activeInterrupt);
                activeInterrupt = -1;
                state = InstructionState.Fetch;
                return 1;
        }

        return 0;
    }

    //move to registers file?
    //https://github.com/robert/gameboy-doctor
    public void CPUResetRegisters()
    {
        A = 0x01;
        F = 0xB0;
        B = 0x00;
        C = 0x13;
        D = 0x00;
        E = 0xD8;
        H = 0x01;
        L = 0x4F;
        

        AF = 0x01B0;
        BC = 0x0013;
        DE = 0x00D8;
        HL = 0x014d;

        PC = 0x100;
        SP = 0xFFFE;
    }


    //creates logfiles for gameboy doctor
    private StreamWriter? traceWriter;

    public void StartTrace(string fileName = "cpu_trace.log")
      {
        // Combines the folder path of the executable with your filename
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        traceWriter = new StreamWriter(fullPath, false);
        traceWriter.AutoFlush = true;
      }

    public void StopTrace()
    {
        traceWriter?.Flush();
        traceWriter?.Close();
        traceWriter = null;
    }

    private void LogState()
    {
        if (traceWriter == null) return;

        byte pc0 = bus.ReadByte(PC);
        byte pc1 = bus.ReadByte((ushort)(PC + 1));
        byte pc2 = bus.ReadByte((ushort)(PC + 2));
        byte pc3 = bus.ReadByte((ushort)(PC + 3));

        traceWriter.WriteLine(
            $"A:{A:X2} F:{F:X2} B:{B:X2} C:{C:X2} D:{D:X2} E:{E:X2} H:{H:X2} L:{L:X2} " +
            $"SP:{SP:X4} PC:{PC:X4} PCMEM:{pc0:X2},{pc1:X2},{pc2:X2},{pc3:X2}"
        );
    }
}