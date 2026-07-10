public class Cartridge
{
    public readonly byte[] rom;
    private readonly byte[] eram = new byte[0x2000]; //the save

    public byte[] Rom => rom;
    public byte[] Eram => eram;

    public byte CartridgeType { get; }
    public byte RomSizeCode { get; }
    public byte RamSizeCode { get; }

    public string Title { get; }

    public bool HasRam { get; }
    public bool HasBattery { get; }

    public Cartridge(string romName)
    {
        byte[] rom = File.ReadAllBytes(romName);
        

        CartridgeType = rom[0x0147];
        RomSizeCode   = rom[0x0148];
        RamSizeCode   = rom[0x0149];

        Title = ReadTitle();
        eram = new byte[GetRamSizeBytes(RamSizeCode)];

        HasRam = CartridgeHasRam(CartridgeType);
        HasBattery = CartridgeHasBattery(CartridgeType);
    }


    public void CartrideInfoToConsole()
    {
        Console.WriteLine(Cartridge.Title);
        Console.WriteLine(Cartridge.CartridgeType);
        Console.WriteLine(GetRomSizeBytes(RomSizeCode));
        Console.WriteLine("Has ram: " + HasRam);
        Console.WriteLine(GetRamSizeBytes(RamSizeCode));
        Console.WriteLine("Has battery: " + HasBattery);

        Console.WriteLine("Passed checksum: " + CartrideChecksum());
    }

    //return a string going from 0x0134 to 0x0143, breaking if a character == 0x00.
    public string ReadTitle()
    {
        string CartrideName = "";
        
        for (int i = 0; i < 16; i++)
        {
            int charAddress = rom[0x134 + i];
            if (rom[charAddress] == 0x00) break;
            else CartrideName += (char)(rom[charAddress]);
        }
        
        return CartrideName;
    }

    public bool isRomOnly()
    {
        return CartridgeType == 0x00;
    }

    //https://gbdev.io/pandocs/The_Cartridge_Header.html used for following cases
    public int GetRomSizeBytes(byte RomSizeCode)
    {
        int kbit = 1024;
        int mbit = 1048576;

        switch (RomSizeCode)
        {
            case 0x00: return 32 * kbit;
            case 0x01: return 64 * kbit;
            case 0x02: return 128 * kbit;
            case 0x03: return 256 * kbit;
            case 0x04: return 512 * kbit;
            case 0x05: return 1 * mbit;
            case 0x06: return 2 * kbit;
            case 0x07: return 4 * mbit;
            case 0x08: return 8 * mbit;
            default: return -1; //temp, write to terminal and find a fallback
        }
    }

public int GetRamSizeBytes(byte RamSizeCode)
    {
        int kbit = 1024;
        switch (RamSizeCode)
        {
            case 0x00: return 0;
            case 0x01: return -1; //unused idk if -1 is a good idea?
            case 0x02: return 8 * kbit;
            case 0x03: return 32 * kbit;
            case 0x04: return 128 * kbit;
            case 0x05: return 64 * kbit;
            default: return -1; //temp, write to terminal and find a fallback
        }
    }


    
    private static bool CartridgeHasRam(byte cartridgeType)
    {
        switch (cartridgeType)
        {
            case 0x02: // MBC1+RAM
            case 0x03: // MBC1+RAM+BATTERY
            case 0x08: // ROM+RAM
            case 0x09: // ROM+RAM+BATTERY
            case 0x10: // MBC3+TIMER+RAM+BATTERY
            case 0x12: // MBC3+RAM
            case 0x13: // MBC3+RAM+BATTERY
            case 0x1A: // MBC5+RAM
            case 0x1B: // MBC5+RAM+BATTERY
            case 0x1D: // MBC5+RUMBLE+RAM
            case 0x1E: // MBC5+RUMBLE+RAM+BATTERY
                return true;

            default:
                return false;
        }
    }

    private static bool CartridgeHasBattery(byte cartridgeType)
    {
        switch (cartridgeType)
        {
            case 0x03:
            case 0x06:
            case 0x09:
            case 0x0D:
            case 0x0F:
            case 0x10:
            case 0x13:
            case 0x1B:
            case 0x1E:
                return true;

            default:
                return false;
        }
    }


    //https://gbdev.io/pandocs/The_Cartridge_Header.html#014d--header-checksum
    private bool CartrideChecksum()
    {
        int checksum = 0;
        for (int address = 0x0134; address <= 0x014C; address++)
        {
            checksum = checksum - rom[address] - 1;
        }
        return true;
    }
}