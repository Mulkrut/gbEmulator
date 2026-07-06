public class Bus
{
    private readonly Cartridge cartridge;

    private readonly byte[] vram = new byte[0x2000]; // 8000-9FFF
    private readonly byte[] wram = new byte[0x2000]; // C000-DFFF
    private readonly byte[] oam  = new byte[0x00A0]; // FE00-FE9F
    private readonly byte[] io   = new byte[0x0080]; // FF00-FF7F
    private readonly byte[] hram = new byte[0x007F]; // FF80-FFFE

    public Bus(Cartridge cartridge)
    {
        this.cartridge = cartridge;
    }

    public byte ReadByte(ushort addr)
    {
        //STUFF
        return 0xFF; //temp fix until i add the functionality
    }

    public void WriteByte(ushort addr, byte value)
    {
        //stuff
    }
}