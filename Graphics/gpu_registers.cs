using System.ComponentModel.Design;
using System.Numerics;

partial class GPU
{

    //https://gbdev.io/pandocs/STAT.html
    //LCD status, bits are set depending on mode, selection and LYC1
    public byte STAT;

    //LCD Control
    public byte LCDC;

    public int enableDelay = 0;
    public bool wasDisabled = false;

    public int modeTicks;
    public int lineTicks;

    public byte LY;
    public byte LYC;

   
    public enum InsertState : byte
    {
        MODE_ZERO  = 3,
        MODE_ONE = 4,
        MODE_TWO = 5,
        LYCSelect = 6
    };

    public enum PPU_States : byte
    {
        HBLANK = 0,
        VBLANK = 1,
        SCAN = 2, //OAM
        DRAW = 3
    };

    public bool IsLCDEnabled()
    {
        if ((LCDC & 0b10000000) == 1) return true;
        return false;
    }

    public void SetPPUState(PPU_States state)
    {
        if (state == PPU_States.HBLANK || state == PPU_States.VBLANK)
        {
            if (IsLCDEnabled())
            {
                //Request VBLANK INTERRUPT 0x0040
            }
        }
        STAT = (byte)((STAT & 0b00000011) | ((byte)state & 0b00000011));
    }

    public PPU_States GetPPUState()
    {
        return (PPU_States)(STAT & 0b00000011);
    }
 
    public void SetInsertState(InsertState state)
    {
        STAT = (byte)(STAT | (1 << (byte)state));
    }

    public PPU_States GetSelectState()
    {
        return (PPU_States)(STAT & 0b01111000);
    }
    
    public void LycCompare()
    {
        if (LY == LYC)
        {
            STAT |= 0b00000100;
        }
        else
        {
            STAT &= 0b11111011;
        }
    }

    public void SetCoincidenceFlag()
    {
        if (LY == LYC)
        {
            STAT = (byte)(STAT | 0b00000001);
        }
            STAT = (byte)(STAT & 0b11111110);
    }    
    
    //WINDOW
    public byte WY;
    public byte WX;

    //SCROLLING
    //https://gbdev.io/pandocs/Scrolling.html

    public byte SCY;
    public byte SCX;

    private (int Bottom, int Right) BRViewportCalc()
    {
        int bottom = (SCY + 143) % 256;
        int right = (SCX + 159) % 256;


        return (bottom, right);
    }


}