public class Cartridge
{
    private readonly byte[] rom;
    private readonly byte[] eram = new byte[0x2000];


    public Cartridge(byte[] rom)
    {
        this.rom = rom;
    }
}