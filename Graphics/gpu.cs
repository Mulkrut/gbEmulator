using System.Collections;

partial class GPU
{
    private byte status;
    private PPU_States mode;


    public void EnableLCD()
    {
        enableDelay = 224;
    }

    public void DisableLCD()
    {
        
    }

    public void Tick()
    {
       if (IsLCDEnabled())
        {
            if (wasDisabled)
            {
                enableDelay--;

                if (enableDelay == 0)
                {
                    wasDisabled = false;
                }
                else return;
            }
        }
        else return;


        status = STAT;
        mode = (PPU_States)(STAT & 0b00000011);

        modeTicks++;
        lineTicks++;

        if (modeTicks == 4 && mode == PPU_States.VBLANK && LY == 153)
        {
            //check for oam bitpos and do LCD interrupt
        }
        else
        {
            switch(mode)
            {
                case PPU_States.HBLANK:
                    if (modeTicks == 204)
                    {
                        modeTicks = 0;
                        lineTicks = 0;
                        LY++;

                        if (LY == 144)
                        {
                            mode = PPU_States.VBLANK;
                            //REQUEST VBLANK INTERRUPT

                        }
                        else
                        {
                            mode = PPU_States.SCAN;
                        }
                    }
                    break;

                case PPU_States.VBLANK:
                    if (modeTicks == 456)
                    {
                        modeTicks = 0;
                        lineTicks = 0;
                        LY++;

                        if (LY == 1)
                        {
                            mode = PPU_States.SCAN;
                            LY = 0;

                            //LCD INTERRUPT
                        }
                    }
                    break;

                case PPU_States.SCAN:
                    break;
                case PPU_States.DRAW:

                    break;
            }            
            //Set STAT bits
            SetCoincidenceFlag();
            
        }
    }
        
    // private void RenderLine()
    // {
        
    // }

    // private void RenderTile()
    // {
        
    // }
    
}