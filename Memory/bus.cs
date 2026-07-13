public class BUS
{
    //Bus or mmu, both is the same

    private Cartridge cartridge;
    private readonly Timers timer;
    private readonly InterruptManager intManager;
    public DMA dma { get; }
    
    private byte[] rom = Array.Empty<byte>();
    private byte[] vram = new byte[0x2000];   // 8000-9FFF
    private byte[] eram = new byte[0x2000];   // A000-BFFF
    private byte[] wram = new byte[0x2000];   // C000-DFFF
    private byte[] oam  = new byte[0x00A0];   // FE00-FE9F
    private byte[] hram = new byte[0x007F];   // FF80-FFFE
    private byte[] io = new byte[0x80];       // fallback
    //private byte ieRegister;                  // FFFF   unused as id rather use IE from intmanager

    //private byte interruptFlags;        // FF0F
    //moved to interrupts IF

    //used for the testrom output
    private byte sb; // FF01
    private byte sc; // FF02


    //maybe dma and hpu is not needed here, just inserted for consistency and to get it to compile
    public BUS(Cartridge cartridge, Timers timer, InterruptManager intManager, GPU gpu, DMA dma)
    {
        this.cartridge = cartridge;
        this.rom = cartridge.rom;
        this.timer = timer; //fix
        this.intManager = intManager;
        this.dma = dma;
    }

    public void LoadData(string romName)
    {
        cartridge = new Cartridge(romName);
        rom = cartridge.Rom;
    }
    /*
    Read, write & fetch


    https://github.com/rvaccarim/FrozenBoy/blob/master/FrozenBoyCore/Memory/MMU.cs

    http://gameboy.mongenel.com/dmg/asmmemmap.html
    0000-3FFF: fixed ROM bank
    4000-7FFF: switchable ROM bank
    8000-9FFF: VRAM
    A000-BFFF: cartridge RAM
    C000-DFFF: WRAM
    E000-FDFF: echo of WRAM
    FE00-FE9F: OAM (object attribute memory) (sprites)
    FF00-FF7F: I/O registers
    FF80-FFFE: HRAM
    FFFF: interrupt enable register*/

    public byte ReadByte(ushort address)
    {
        if (address <= 0x3FFF)
        {
            return cartridge.rom[address];
        }
        else if (address <= 0x7FFF)
        {
            return ReadRomBanked(address);
        }
        else if (address <= 0x9FFF)
        {
            return vram[address - 0x8000];
        }
        else if (address <= 0xBFFF)
        {
            return ReadExternalRam(address);
        }
        else if (address <= 0xDFFF)
        {
            return wram[address - 0xC000];
        }
        else if (address <= 0xFDFF)
        {
            return wram[address - 0xE000]; // echo RAM
        }
        else if (address <= 0xFE9F)
        {
            return oam[address - 0xFE00];
        }
        else if (address <= 0xFEFF)
        {
            return 0xFF;
        }
        else if (address <= 0xFF7F)
        {
            return ReadIO(address);
        }
        else if (address <= 0xFFFE)
        {
            return hram[address - 0xFF80];
        }
        else
        {
            return intManager.IE;
        }
    }

    public void WriteByte(ushort address, byte value)
    {
        if (address <= 0x3FFF)
        {
            WriteRomControl(address, value);
        }
        else if (address <= 0x7FFF)
        {
            WriteRomControl(address, value);
        }
        else if (address <= 0x9FFF)
        {
            vram[address - 0x8000] = value;
        }
        else if (address <= 0xBFFF)
        {
            WriteExternalRam(address, value);
        }
        else if (address <= 0xDFFF)
        {
            wram[address - 0xC000] = value;
        }
        else if (address <= 0xFDFF)
        {
            wram[address - 0xE000] = value;
        }
        else if (address <= 0xFE9F)
        {
            oam[address - 0xFE00] = value;
        }
        else if (address <= 0xFEFF)
        {
            //nothing
        }
        else if (address <= 0xFF7F)
        {
            WriteIO(address, value);
        }
        else if (address <= 0xFFFE)
        {
            hram[address - 0xFF80] = value;
        }
        else
        {
            intManager.IE = value;
        }
    }

    
    //TODO:
    private byte ReadIO(ushort address)
    {
        switch (address)
        {
            //just for console output from testrom
            case 0xFF01: return sb;
            case 0xFF02: return sc;

            //actual ReadIO
            case 0xFF04: return timer.DIV;
            case 0xFF05: return timer.TIMA;
            case 0xFF06: return timer.TMA;
            case 0xFF07: return timer.TAC;
            case 0xFF0F: return intManager.IF;
            default:     return io[address - 0xFF00];
        }
    }

    private void WriteIO(ushort address, byte value)
    {
        switch (address)
        {
            //just for console output from testrom
            case 0xFF01:
            sb = value;
            break;

            case 0xFF02:
                sc = value;

                if (sc == 0x81)
                {
                    Console.Write((char)sb);
                    sc = 0x00; // transfer complete for simple test ROM output
                }
                break;

            //actual Write
            case 0xFF04:
                timer.DIV = 0; // writing any value resets DIV
                break;

            case 0xFF05:
                timer.TIMA = value;
                break;

            case 0xFF06:
                timer.TMA = value;
                break;

            case 0xFF07:
                timer.TAC = (byte)(value & 0x07); // only bits 0-2 are used
                break;

            case 0xFF0F:
                intManager.IF = (byte)(value & 0x1F); // only low 5 interrupt bits
                break;

            default:
                io[address - 0xFF00] = value;
                break;
        }
    }

    private byte ReadExternalRam(ushort address)
    {
        if (!cartridge.HasRam) return 0xFF;

        int offset = address - 0xA000;
        byte[] eram = cartridge.Eram;

        if (offset < 0 || offset >= eram.Length)
            return 0xFF;

        return eram[offset];
    }

private void WriteExternalRam(ushort address, byte value)
    {
        if (!cartridge.HasRam) return;

        int offset = address - 0xA000;
        byte[] eram = cartridge.Eram;

        if (offset < 0 || offset >= eram.Length) return;

        eram[offset] = value;
    }

    private byte ReadRomBanked(ushort address)
    {
        if (address >= rom.Length) return 0xFF;
        else return rom[address];
    }

    private void WriteRomControl(ushort address, byte value)
    {
        //todo
    }

    //maybe put into interrupts
    public void RequestInterrupt(int bit)
    {
        intManager.RequestInterruption((byte)bit);
    }
}