public partial class CPU
{

    //8-bit arithmetic
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
        SetCFlag(result < 0);

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

////////////////////////////////////////////////////////////////////////////////////////

    private ushort Add16(ushort a, ushort b)
    {
        int result = a + b;
        ushort r = (ushort)result; //converting from int to ushort

        SetNFlag(false);
        SetHFlag(((a & 0x0FFF) + (b & 0x0FFF)) > 0x0FFF); //0x0F = 00001111
        SetCFlag(result > 0xFFFF);

        return r;
    }

    private ushort AddSigned8ToSP(ushort sp, byte value)
    {
        sbyte offset = unchecked((sbyte)value);
        int result = sp + offset;

        SetZFlag(false);
        SetNFlag(false);
        SetHFlag(((sp & 0x0F) + (value & 0x0F)) > 0x0F);
        SetCFlag(((sp & 0xFF) + (value & 0xFF)) > 0xFF);

        return (ushort)result;
    }


    //Rotation / bit shifting
    private byte RlcA(byte value)
    {
        int oldBit7 = (value >> 7) & 0x01;
        byte r = (byte)((value << 1) | oldBit7);

        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit7 != 0);
        return r;
    }

    private byte RlA(byte value)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int oldBit7 = (value >> 7) & 0x01;
        byte r = (byte)((value << 1) | carryIn);

        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit7 != 0);

        return r;
    }

    //to the right now yaall
    private byte RrcA(byte value)
    {
        int oldBit0 = value & 0x01;
        byte r = (byte)((value >> 1) | oldBit0 << 7);

        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);
        return r;
    }

    private byte RrA(byte value)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int oldBit0 = value & 0x01;
        byte r = (byte)((value >> 1) | carryIn << 7);

        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);

        return r;
    }


    //just like the A versions but they set the Z flag (only using in the ExecuteCBOpcode)
    private byte Rlc(byte value)
    {
        int oldBit7 = (value >> 7) & 1;
        byte r = (byte)((value << 1) | oldBit7);
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit7 != 0);
        return r;
    }

    private byte Rrc(byte value)
    {
        int oldBit0 = value & 1;
        byte r = (byte)((value >> 1) | (oldBit0 << 7));
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);
        return r;
    }

    private byte Rl(byte value)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int oldBit7 = (value >> 7) & 1;
        byte r = (byte)((value << 1) | carryIn);
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit7 != 0);
        return r;
    }

    private byte Rr(byte value)
    {
        int carryIn = GetCFlag() ? 1 : 0;
        int oldBit0 = value & 1;
        byte r = (byte)((value >> 1) | (carryIn << 7));
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);
        return r;
    }

    private byte Sla(byte value)
    {
        int oldBit7 = (value >> 7) & 1;
        byte r = (byte)(value << 1);
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit7 != 0);
        return r;
    }

    private byte Sra(byte value)
    {
        int oldBit0 = value & 1;
        byte r = (byte)((value >> 1) | (value & 0x80));
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);
        return r;
    }

    private byte Srl(byte value)
    {
        int oldBit0 = value & 1;
        byte r = (byte)(value >> 1);
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(oldBit0 != 0);
        return r;
    }

    private byte Swap(byte value)
    {
        byte r = (byte)((value << 4) | (value >> 4));
        SetZFlag(r == 0);
        SetNFlag(false);
        SetHFlag(false);
        SetCFlag(false);
        return r;
    }

    private void TestBit(int bit, byte value)
    {
        SetZFlag((value & (1 << bit)) == 0);
        SetNFlag(false);
        SetHFlag(true);
    }
}