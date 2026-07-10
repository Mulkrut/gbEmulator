using System;

//https://izik1.github.io/gbops/index.html


//Modified version of FrozenBoyCores opcode handler
public partial class CPU
{
    public int ExecuteBaseOpcode(byte opcode)
    {
        // LD r8, r8 block: 0x40..0x7F, except 0x76 = HALT
        if (opcode >= 0x40 && opcode <= 0x7F)
        {
            if (opcode == 0x76)
            {
                Halt();
                return 4;
            }

            int dst = (opcode >> 3) & 0b111;
            int src = opcode & 0b111;

            byte value = ReadR8(src);
            WriteR8(dst, value);

            return (dst == 6 || src == 6) ? 8 : 4;
        }

        // ALU A, r8 block: 0x80..0xBF
        if (opcode >= 0x80 && opcode <= 0xBF)
        {
            int op = (opcode >> 3) & 0b111;
            int src = opcode & 0b111;
            byte value = ReadR8(src);

            switch (op)
            {
                case 0: A = Add8(A, value); break;
                case 1: A = Adc8(A, value); break;
                case 2: A = Sub8(A, value); break;
                case 3: A = Sbc8(A, value); break;
                case 4: A = And8(A, value); break;
                case 5: A = Xor8(A, value); break;
                case 6: A = Or8(A, value); break;
                case 7: Cp8(A, value); break;
            }

            return src == 6 ? 8 : 4;
        }

        switch (opcode)
        {
            // =====================================================================
            // MISC
            // =====================================================================
            case 0x00: return 4; // NOP
            case 0x10: Stop(); return 4; // STOP
            case 0xCB: return ExecuteCBOpcode(Fetch8());
            case 0xF3: DisableInterrupts(); return 4; // DI
            case 0xFB: EnableInterrupts(true); return 4; // EI
            case 0x76: Halt(); return 4; // HALT safety

            // =====================================================================
            // 8-bit immediate loads
            // =====================================================================
            case 0x06: B = Fetch8(); return 8;
            case 0x0E: C = Fetch8(); return 8;
            case 0x16: D = Fetch8(); return 8;
            case 0x1E: E = Fetch8(); return 8;
            case 0x26: H = Fetch8(); return 8;
            case 0x2E: L = Fetch8(); return 8;
            case 0x36: bus.WriteByte(HL, Fetch8()); return 12;
            case 0x3E: A = Fetch8(); return 8;

            // =====================================================================
            // 16-bit immediate loads
            // =====================================================================
            case 0x01: BC = Fetch16(); return 12;
            case 0x11: DE = Fetch16(); return 12;
            case 0x21: HL = Fetch16(); return 12;
            case 0x31: SP = Fetch16(); return 12;

            // =====================================================================
            // 8-bit INC
            // =====================================================================
            case 0x04: B = Inc8(B); return 4;
            case 0x0C: C = Inc8(C); return 4;
            case 0x14: D = Inc8(D); return 4;
            case 0x1C: E = Inc8(E); return 4;
            case 0x24: H = Inc8(H); return 4;
            case 0x2C: L = Inc8(L); return 4;
            case 0x34:
            {
                byte value = bus.ReadByte(HL);
                bus.WriteByte(HL, Inc8(value));
                return 12;
            }
            case 0x3C: A = Inc8(A); return 4;

            // =====================================================================
            // 8-bit DEC
            // =====================================================================
            case 0x05: B = Dec8(B); return 4;
            case 0x0D: C = Dec8(C); return 4;
            case 0x15: D = Dec8(D); return 4;
            case 0x1D: E = Dec8(E); return 4;
            case 0x25: H = Dec8(H); return 4;
            case 0x2D: L = Dec8(L); return 4;
            case 0x35:
            {
                byte value = bus.ReadByte(HL);
                bus.WriteByte(HL, Dec8(value));
                return 12;
            }
            case 0x3D: A = Dec8(A); return 4;

            // =====================================================================
            // 16-bit INC
            // =====================================================================
            case 0x03: BC++; return 8;
            case 0x13: DE++; return 8;
            case 0x23: HL++; return 8;
            case 0x33: SP++; return 8;

            // =====================================================================
            // 16-bit DEC
            // =====================================================================
            case 0x0B: BC--; return 8;
            case 0x1B: DE--; return 8;
            case 0x2B: HL--; return 8;
            case 0x3B: SP--; return 8;

            // =====================================================================
            // 8-bit immediate ALU
            // =====================================================================
            case 0xC6: A = Add8(A, Fetch8()); return 8;
            case 0xCE: A = Adc8(A, Fetch8()); return 8;
            case 0xD6: A = Sub8(A, Fetch8()); return 8;
            case 0xDE: A = Sbc8(A, Fetch8()); return 8;
            case 0xE6: A = And8(A, Fetch8()); return 8;
            case 0xEE: A = Xor8(A, Fetch8()); return 8;
            case 0xF6: A = Or8(A, Fetch8()); return 8;
            case 0xFE: Cp8(A, Fetch8()); return 8;

            // =====================================================================
            // 16-bit arithmetic
            // =====================================================================
            case 0x09: HL = Add16(HL, BC); return 8;
            case 0x19: HL = Add16(HL, DE); return 8;
            case 0x29: HL = Add16(HL, HL); return 8;
            case 0x39: HL = Add16(HL, SP); return 8;
            case 0xE8: SP = AddSigned8ToSP(SP, Fetch8()); return 16;
            case 0xF8: HL = AddSigned8ToSP(SP, Fetch8()); return 12;
            case 0xF9: SP = HL; return 8;

            // =====================================================================
            // Direct memory/register loads
            // =====================================================================
            case 0x02: bus.WriteByte(BC, A); return 8;
            case 0x12: bus.WriteByte(DE, A); return 8;
            case 0x0A: A = bus.ReadByte(BC); return 8;
            case 0x1A: A = bus.ReadByte(DE); return 8;
            case 0x22: bus.WriteByte(HL++, A); return 8;
            case 0x32: bus.WriteByte(HL--, A); return 8;
            case 0x2A: A = bus.ReadByte(HL++); return 8;
            case 0x3A: A = bus.ReadByte(HL--); return 8;

            case 0x08:
            {
                ushort addr = Fetch16();
                bus.WriteByte(addr, (byte)(SP & 0x00FF));
                bus.WriteByte((ushort)(addr + 1), (byte)(SP >> 8));
                return 20;
            }

            case 0xEA:
            {
                ushort addr = Fetch16();
                bus.WriteByte(addr, A);
                return 16;
            }

            case 0xFA:
            {
                ushort addr = Fetch16();
                A = bus.ReadByte(addr);
                return 16;
            }

            case 0xE0:
            {
                byte offset = Fetch8();
                bus.WriteByte((ushort)(0xFF00 + offset), A);
                return 12;
            }

            case 0xF0:
            {
                byte offset = Fetch8();
                A = bus.ReadByte((ushort)(0xFF00 + offset));
                return 12;
            }

            case 0xE2: bus.WriteByte((ushort)(0xFF00 + C), A); return 8;
            case 0xF2: A = bus.ReadByte((ushort)(0xFF00 + C)); return 8;

            // =====================================================================
            // JP / JR
            // =====================================================================
            case 0xC3: PC = Fetch16(); return 16;
            case 0xE9: PC = HL; return 4;

            case 0xC2: return JumpConditional(!GetZFlag());
            case 0xCA: return JumpConditional(GetZFlag());
            case 0xD2: return JumpConditional(!GetCFlag());
            case 0xDA: return JumpConditional(GetCFlag());

            case 0x18: return JumpRelative(true);
            case 0x20: return JumpRelative(!GetZFlag());
            case 0x28: return JumpRelative(GetZFlag());
            case 0x30: return JumpRelative(!GetCFlag());
            case 0x38: return JumpRelative(GetCFlag());

            // =====================================================================
            // CALL / RET / RST
            // =====================================================================
            case 0xCD: return CallConditional(true);
            case 0xC4: return CallConditional(!GetZFlag());
            case 0xCC: return CallConditional(GetZFlag());
            case 0xD4: return CallConditional(!GetCFlag());
            case 0xDC: return CallConditional(GetCFlag());

            case 0xC9: PC = Pop16(); return 16;
            case 0xD9:
                PC = Pop16();
                EnableInterrupts(false);
                return 16;

            case 0xC0: return ReturnConditional(!GetZFlag());
            case 0xC8: return ReturnConditional(GetZFlag());
            case 0xD0: return ReturnConditional(!GetCFlag());
            case 0xD8: return ReturnConditional(GetCFlag());

            case 0xC7: Push16(PC); PC = 0x00; return 16;
            case 0xCF: Push16(PC); PC = 0x08; return 16;
            case 0xD7: Push16(PC); PC = 0x10; return 16;
            case 0xDF: Push16(PC); PC = 0x18; return 16;
            case 0xE7: Push16(PC); PC = 0x20; return 16;
            case 0xEF: Push16(PC); PC = 0x28; return 16;
            case 0xF7: Push16(PC); PC = 0x30; return 16;
            case 0xFF: Push16(PC); PC = 0x38; return 16;

            // =====================================================================
            // PUSH / POP
            // =====================================================================
            case 0xC1: BC = Pop16(); return 12;
            case 0xD1: DE = Pop16(); return 12;
            case 0xE1: HL = Pop16(); return 12;
            case 0xF1: AF = Pop16(); F &= 0xF0; return 12;

            case 0xC5: Push16(BC); return 16;
            case 0xD5: Push16(DE); return 16;
            case 0xE5: Push16(HL); return 16;
            case 0xF5: Push16(AF); return 16;

            // =====================================================================
            // Rotates on A
            // =====================================================================
            case 0x07: A = RlcA(A); SetZFlag(false); return 4;
            case 0x17: A = RlA(A); SetZFlag(false); return 4;
            case 0x0F: A = RrcA(A); SetZFlag(false); return 4;
            case 0x1F: A = RrA(A); SetZFlag(false); return 4;

            // =====================================================================
            // Flag / misc ops
            // =====================================================================
            case 0x27: Daa(); return 4;
            case 0x2F:
                A = (byte)~A;
                SetNFlag(true);
                SetHFlag(true);
                return 4;

            case 0x37:
                SetNFlag(false);
                SetHFlag(false);
                SetCFlag(true);
                return 4;

            case 0x3F:
                SetNFlag(false);
                SetHFlag(false);
                SetCFlag(!GetCFlag());
                return 4;

            default:
                throw new NotImplementedException($"Opcode 0x{opcode:X2} not implemented");
        }
    }

    private byte ReadR8(int index)
    {
        return index switch
        {
            0 => B,
            1 => C,
            2 => D,
            3 => E,
            4 => H,
            5 => L,
            6 => bus.ReadByte(HL),
            7 => A,
            _ => throw new InvalidOperationException($"Invalid r8 index: {index}")
        };
    }

    private void WriteR8(int index, byte value)
    {
        switch (index)
        {
            case 0: B = value; break;
            case 1: C = value; break;
            case 2: D = value; break;
            case 3: E = value; break;
            case 4: H = value; break;
            case 5: L = value; break;
            case 6: bus.WriteByte(HL, value); break;
            case 7: A = value; break;
            default: throw new InvalidOperationException($"Invalid r8 index: {index}");
        }
    }


    private int JumpConditional(bool condition)
    {
        ushort address = Fetch16();
        if (condition)
        {
            PC = address;
        }
        return 16;
    }

    private int JumpRelative(bool condition)
    {
        sbyte offset = unchecked((sbyte)Fetch8());
        if (condition)
        {
            PC = (ushort)(PC + offset);
            return 12;
        }
        return 8;
    }

    private int CallConditional(bool condition)
    {
        ushort address = Fetch16();
        if (condition)
        {
            Push16(PC);
            PC = address;
            return 24;
        }
        return 12;
    }

    private int ReturnConditional(bool condition)
    {
        if (condition)
        {
            PC = Pop16();
            return 20;
        }
        return 8;
    }


}