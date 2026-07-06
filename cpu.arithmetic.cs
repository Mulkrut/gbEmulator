public partial class CPU
{

  //Block 2 - 8-bit arithmetic

    private byte Add8(byte a, byte b)
    {
        int result = a + b;
        byte r = (byte)result; //converting from int to byte

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(((a & 0x0F) + (b & 0x0F)) > 0x0F); //0x0F = 00001111
        SetCFlag(result > 0xFF);

        return r;
    }
    
    private byte Adc8(byte a, byte b)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int result = a + b + carryIn;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(((a & 0x0F) + (b & 0x0F) + carryIn) > 0x0F);
        SetCFlag(result > 0xFF);

        return r;
    }
    
    private byte Sub8(byte a, byte b)
    {
        int result = a - b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < (b & 0x0F));
        SetCFlag(a < b);

        return r;
    }

    private byte Sbc8(byte a, byte b)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int result = a - b - carryIn;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < ((b & 0x0F) + carryIn));
        SetCFlag(a < b + carryIn);

        return r;
    }

    private byte And8(byte a, byte b)
    {
        int result = a & b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(true);
        SetCFlag(false);

        return r;
    }

    private byte Or8(byte a, byte b)
    {
        int result = a | b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(false);

        return r;
    }

    private byte Xor8(byte a, byte b)
    {
        int result = a ^ b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(false);
        return r;
    }

    //Uses to compare, like subtract except you dont return a
    private void Cp8(byte a, byte b)
    {
        int result = a - b;
        byte r = (byte)result;

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) < (b & 0x0F));
        SetCFlag(a < b);
    }

    private byte Inc8(byte a) 
    {
        byte r = (byte)(a + 1);

        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag((a & 0x0F) == 0x0F);
        return r;
    }

    private byte Dec8(byte a) 
    {
        byte r = (byte)(a - 1);

        SetZFlag(r == 0);
        SetNFlag(true);
        SetHFlag((a & 0x0F) == 0x00);
        return r;
    }
}