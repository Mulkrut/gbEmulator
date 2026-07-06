//Reference sheet, will most likely have a simpler way to determine the opcode.

public class OPCODE 
{
    

    //different opcodes and suffix with their intended command taken from:
    //https://github.com/BotRandomness/CODE-DMG/blob/master/src/CPU.cs#L2235

    public int ExecuteInstruction()
    {
        
        int interruptCycles = HandleInterrupts();
        if (interruptCycles > 0) {
            return interruptCycles;
        }

        if (halted) {
            return 4;
        }

        byte opcode = Fetch();

        switch (opcode) {
            case 0x00:
                return NOP();
            case 0x01:
                return LD_RR_U16(ref B, ref C);
            case 0x02:
                return LD_ARR_R(ref A, "bc");
            case 0x03:
                return INC_RR("bc");
            case 0x04:
                return INC_R(ref B);
            case 0x05:
                return DEC_R(ref B);
            case 0x06:
                return LD_R_U8(ref B);
            case 0x07:
                return RLCA();
            case 0x08:
                return LD_AU16_SP();
            case 0x09:
                return ADD_HL_RR("bc");
            case 0x0A:
                return LD_R_ARR(ref A, "bc");
            case 0x0B:
                return DEC_RR("bc");
            case 0x0C:
                return INC_R(ref C);
            case 0x0D:
                return DEC_R(ref C);
            case 0x0E:
                return LD_R_U8(ref C);
            case 0x0F:
                return RRCA();
            case 0x10:
                return STOP();
            case 0x11:
                return LD_RR_U16(ref D, ref E);
            case 0x12:
                return LD_ARR_R(ref A, "de");
            case 0x13:
                return INC_RR("de");
            case 0x14:
                return INC_R(ref D);
            case 0x15:
                return DEC_R(ref D);
            case 0x16:
                return LD_R_U8(ref D);
            case 0x17:
                return RLA();
            case 0x18:
                return JR_CON_I8(true);
            case 0x19:
                return ADD_HL_RR("de");
            case 0x1A:
                return LD_R_ARR(ref A, "de");
            case 0x1B:
                return DEC_RR("de");
            case 0x1C:
                return INC_R(ref E);
            case 0x1D:
                return DEC_R(ref E);
            case 0x1E:
                return LD_R_U8(ref E);
            case 0x1F:
                return RRA();
            case 0x20:
                return JR_CON_I8(!zero);
            case 0x21:
                return LD_RR_U16(ref H, ref L);
            case 0x22:
                return LD_AHLI_A();
            case 0x23:
                return INC_RR("hl");
            case 0x24:
                return INC_R(ref H);
            case 0x25:
                return DEC_R(ref H);
            case 0x26:
                return LD_R_U8(ref H);
            case 0x27:
                return DAA();
            case 0x28:
                return JR_CON_I8(zero);
            case 0x29:
                return ADD_HL_RR("hl");
            case 0x2A:
                return LD_A_AHLI();
            case 0x2B:
                return DEC_RR("hl");
            case 0x2C:
                return INC_R(ref L);
            case 0x2D:
                return DEC_R(ref L);
            case 0x2E:
                return LD_R_U8(ref L);
            case 0x2F:
                return CPL();
            case 0x30:
                return JR_CON_I8(!carry);
            case 0x31:
                return LD_SP_U16();
            case 0x32:
                return LD_AHLM_A();
            case 0x33:
                return INC_SP();
            case 0x34:
                return INC_AHL();
            case 0x35:
                return DEC_AHL();
            case 0x36:
                return LD_AHL_U8();
            case 0x37:
                return SCF();
            case 0x38:
                return JR_CON_I8(carry);
            case 0x39:
                return ADD_HL_SP();
            case 0x3A:
                return LD_A_AHLM();
            case 0x3B:
                return DEC_SP();
            case 0x3C:
                return INC_R(ref A);
            case 0x3D:
                return DEC_R(ref A);
            case 0x3E:
                return LD_R_U8(ref A);
            case 0x3F:
                return CCF();
            case 0x40:
                return LD_R1_R2(ref B, ref B);
            case 0x41:
                return LD_R1_R2(ref B, ref C);
            case 0x42:
                return LD_R1_R2(ref B, ref D);
            case 0x43:
                return LD_R1_R2(ref B, ref E);
            case 0x44:
                return LD_R1_R2(ref B, ref H);
            case 0x45:
                return LD_R1_R2(ref B, ref L);
            case 0x46:
                return LD_R_ARR(ref B, "hl");
            case 0x47:
                return LD_R1_R2(ref B, ref A);
            case 0x48:
                return LD_R1_R2(ref C, ref B);
            case 0x49:
                return LD_R1_R2(ref C, ref C);
            case 0x4A:
                return LD_R1_R2(ref C, ref D);
            case 0x4B:
                return LD_R1_R2(ref C, ref E);
            case 0x4C:
                return LD_R1_R2(ref C, ref H);
            case 0x4D:
                return LD_R1_R2(ref C, ref L);
            case 0x4E:
                return LD_R_ARR(ref C, "hl");
            case 0x4F:
                return LD_R1_R2(ref C, ref A);
            case 0x50:
                return LD_R1_R2(ref D, ref B);
            case 0x51:
                return LD_R1_R2(ref D, ref C);
            case 0x52:
                return LD_R1_R2(ref D, ref D);
            case 0x53:
                return LD_R1_R2(ref D, ref E);
            case 0x54:
                return LD_R1_R2(ref D, ref H);
            case 0x55:
                return LD_R1_R2(ref D, ref L);
            case 0x56:
                return LD_R_ARR(ref D, "hl");
            case 0x57:
                return LD_R1_R2(ref D, ref A);
            case 0x58:
                return LD_R1_R2(ref E, ref B);
            case 0x59:
                return LD_R1_R2(ref E, ref C);
            case 0x5A:
                return LD_R1_R2(ref E, ref D);
            case 0x5B:
                return LD_R1_R2(ref E, ref E);
            case 0x5C:
                return LD_R1_R2(ref E, ref H);
            case 0x5D:
                return LD_R1_R2(ref E, ref L);
            case 0x5E:
                return LD_R_ARR(ref E, "hl");
            case 0x5F:
                return LD_R1_R2(ref E, ref A);
            case 0x60:
                return LD_R1_R2(ref H, ref B);
            case 0x61:
                return LD_R1_R2(ref H, ref C);
            case 0x62:
                return LD_R1_R2(ref H, ref D);
            case 0x63:
                return LD_R1_R2(ref H, ref E);
            case 0x64:
                return LD_R1_R2(ref H, ref H);
            case 0x65:
                return LD_R1_R2(ref H, ref L);
            case 0x66:
                return LD_R_ARR(ref H, "hl");
            case 0x67:
                return LD_R1_R2(ref H, ref A);
            case 0x68:
                return LD_R1_R2(ref L, ref B);
            case 0x69:
                return LD_R1_R2(ref L, ref C);
            case 0x6A:
                return LD_R1_R2(ref L, ref D);
            case 0x6B:
                return LD_R1_R2(ref L, ref E);
            case 0x6C:
                return LD_R1_R2(ref L, ref H);
            case 0x6D:
                return LD_R1_R2(ref L, ref L);
            case 0x6E:
                return LD_R_ARR(ref L, "hl");
            case 0x6F:
                return LD_R1_R2(ref L, ref A);
            case 0x70:
                return LD_ARR_R(ref B, "hl");
            case 0x71:
                return LD_ARR_R(ref C, "hl");
            case 0x72:
                return LD_ARR_R(ref D, "hl");
            case 0x73:
                return LD_ARR_R(ref E, "hl");
            case 0x74:
                return LD_ARR_R(ref H, "hl");
            case 0x75:
                return LD_ARR_R(ref L, "hl");
            case 0x76:
                return HALT();
            case 0x77:
                return LD_ARR_R(ref A, "hl");
            case 0x78:
                return LD_R1_R2(ref A, ref B);
            case 0x79:
                return LD_R1_R2(ref A, ref C);
            case 0x7A:
                return LD_R1_R2(ref A, ref D);
            case 0x7B:
                return LD_R1_R2(ref A, ref E);
            case 0x7C:
                return LD_R1_R2(ref A, ref H);
            case 0x7D:
                return LD_R1_R2(ref A, ref L);
            case 0x7E:
                return LD_R_ARR(ref A, "hl");
            case 0x7F:
                return LD_R1_R2(ref A, ref A);
            case 0x80:
                return ADD_A_R(ref B);
            case 0x81:
                return ADD_A_R(ref C);
            case 0x82:
                return ADD_A_R(ref D);
            case 0x83:
                return ADD_A_R(ref E);
            case 0x84:
                return ADD_A_R(ref H);
            case 0x85:
                return ADD_A_R(ref L);
            case 0x86:
                return ADD_A_ARR("hl");
            case 0x87:
                return ADD_A_R(ref A);
            case 0x88:
                return ADC_A_R(ref B);
            case 0x89:
                return ADC_A_R(ref C);
            case 0x8A:
                return ADC_A_R(ref D);
            case 0x8B:
                return ADC_A_R(ref E);
            case 0x8C:
                return ADC_A_R(ref H);
            case 0x8D:
                return ADC_A_R(ref L);
            case 0x8E:
                return ADC_A_ARR("hl");
            case 0x8F:
                return ADC_A_R(ref A);
            case 0x90:
                return SUB_A_R(ref B);
            case 0x91:
                return SUB_A_R(ref C);
            case 0x92:
                return SUB_A_R(ref D);
            case 0x93:
                return SUB_A_R(ref E);
            case 0x94:
                return SUB_A_R(ref H);
            case 0x95:
                return SUB_A_R(ref L);
            case 0x96:
                return SUB_A_ARR("hl");
            case 0x97:
                return SUB_A_R(ref A);
            case 0x98:
                return SBC_A_R(ref B);
            case 0x99:
                return SBC_A_R(ref C);
            case 0x9A:
                return SBC_A_R(ref D);
            case 0x9B:
                return SBC_A_R(ref E);
            case 0x9C:
                return SBC_A_R(ref H);
            case 0x9D:
                return SBC_A_R(ref L);
            case 0x9E:
                return SBC_A_ARR("hl");
            case 0x9F:
                return SBC_A_R(ref A);
            case 0xA0:
                return AND_A_R(ref B);
            case 0xA1:
                return AND_A_R(ref C);
            case 0xA2:
                return AND_A_R(ref D);
            case 0xA3:
                return AND_A_R(ref E);
            case 0xA4:
                return AND_A_R(ref H);
            case 0xA5:
                return AND_A_R(ref L);
            case 0xA6:
                return AND_A_ARR("hl");
            case 0xA7:
                return AND_A_R(ref A);
            case 0xA8:
                return XOR_A_R(ref B);
            case 0xA9:
                return XOR_A_R(ref C);
            case 0xAA:
                return XOR_A_R(ref D);
            case 0xAB:
                return XOR_A_R(ref E);
            case 0xAC:
                return XOR_A_R(ref H);
            case 0xAD:
                return XOR_A_R(ref L);
            case 0xAE:
                return XOR_A_ARR("hl");
            case 0xAF:
                return XOR_A_R(ref A);
            case 0xB0:
                return OR_A_R(ref B);
            case 0xB1:
                return OR_A_R(ref C);
            case 0xB2:
                return OR_A_R(ref D);
            case 0xB3:
                return OR_A_R(ref E);
            case 0xB4:
                return OR_A_R(ref H);
            case 0xB5:
                return OR_A_R(ref L);
            case 0xB6:
                return OR_A_ARR("hl");
            case 0xB7:
                return OR_A_R(ref A);
            case 0xB8:
                return CP_A_R(ref B);
            case 0xB9:
                return CP_A_R(ref C);
            case 0xBA:
                return CP_A_R(ref D);
            case 0xBB:
                return CP_A_R(ref E);
            case 0xBC:
                return CP_A_R(ref H);
            case 0xBD:
                return CP_A_R(ref L);
            case 0xBE:
                return CP_A_ARR("hl");
            case 0xBF:
                return CP_A_R(ref A);
            case 0xC0:
                return RET_CON(!zero);
            case 0xC1:
                return POP_RR("bc");
            case 0xC2:
                return JP_CON_U16(!zero);
            case 0xC3:
                return JP_CON_U16(true);
            case 0xC4:
                return CALL_CON_U16(!zero);
            case 0xC5:
                return PUSH_RR("bc");
            case 0xC6:
                return ADD_A_U8();
            case 0xC7:
                return RST(0x0000);
            case 0xC8:
                return RET_CON(zero);
            case 0xC9:
                return RET();
            case 0xCA:
                return JP_CON_U16(zero);
            case 0xCB:
                return ExecuteCB();
            case 0xCC:
                return CALL_CON_U16(zero);
            case 0xCD:
                return CALL_U16();
            case 0xCE:
                return ADC_A_U8();
            case 0xCF:
                return RST(0x0008);
            case 0xD0:
                return RET_CON(!carry);
            case 0xD1:
                return POP_RR("de");
            case 0xD2:
                return JP_CON_U16(!carry);
            case 0xD3:
                return DMG_EXIT(opcode);
            case 0xD4:
                return CALL_CON_U16(!carry);
            case 0xD5:
                return PUSH_RR("de");
            case 0xD6:
                return SUB_A_U8();
            case 0xD7:
                return RST(0x0010);
            case 0xD8:
                return RET_CON(carry);
            case 0xD9:
                return RETI();
            case 0xDA:
                return JP_CON_U16(carry);
            case 0xDB:
                return DMG_EXIT(opcode);
            case 0xDC:
                return CALL_CON_U16(carry);
            case 0xDD:
                return DMG_EXIT(opcode);
            case 0xDE:
                return SBC_A_U8();
            case 0xDF:
                return RST(0x0018);
            case 0xE0:
                return LD_FF00_U8_A();
            case 0xE1:
                return POP_RR("hl");
            case 0xE2:
                return LD_FF00_C_A();
            case 0xE3:
                return DMG_EXIT(opcode);
            case 0xE4:
                return DMG_EXIT(opcode);
            case 0xE5:
                return PUSH_RR("hl");
            case 0xE6:
                return AND_A_U8();
            case 0xE7:
                return RST(0x0020);
            case 0xE8:
                return ADD_SP_I8();
            case 0xE9:
                return JP_HL();
            case 0xEA:
                return LD_AU16_A();
            case 0xEB:
                return DMG_EXIT(opcode);
            case 0xEC:
                return DMG_EXIT(opcode);
            case 0xED:
                return DMG_EXIT(opcode);
            case 0xEE:
                return XOR_A_U8();
            case 0xEF:
                return RST(0x0028);
            case 0xF0:
                return LD_A_FF00_U8();
            case 0xF1:
                return POP_AF();
            case 0xF2:
                return LD_A_FF00_C();
            case 0xF3:
                return DI();
            case 0xF4:
                return DMG_EXIT(opcode);
            case 0xF5:
                return PUSH_RR("af");
            case 0xF6:
                return OR_A_U8();
            case 0xF7:
                return RST(0x0030);
            case 0xF8:
                return LD_HL_SP_I8();
            case 0xF9:
                return LD_SP_HL();
            case 0xFA:
                return LD_A_AU16();
            case 0xFB:
                return EI();
            case 0xFC:
                return DMG_EXIT(opcode);
            case 0xFD:
                return DMG_EXIT(opcode);
            case 0xFE:
                return CP_A_U8();
            case 0xFF:
                return RST(0x0038);
            default:
                //Console.WriteLine("Unimplemented Opcode: " + opcode.ToString("X2") + " , PC: " + (PC-1).ToString("X4"));
                //Environment.Exit(1);
                //return 0;
        }
    }



    public int ExecuteCB()
    {
        byte suffix = Fetch();

        switch (suffix)
        {
            case 0x00:
                return RLC_R(ref B);
            case 0x01:
                return RLC_R(ref C);
            case 0x02:
                return RLC_R(ref D);
            case 0x03:
                return RLC_R(ref E);
            case 0x04:
                return RLC_R(ref H);
            case 0x05:
                return RLC_R(ref L);
            case 0x06:
                return RLC_AHL();
            case 0x07:
                return RLC_R(ref A);
            case 0x08:
                return RRC_R(ref B);
            case 0x09:
                return RRC_R(ref C);
            case 0x0A:
                return RRC_R(ref D);
            case 0x0B:
                return RRC_R(ref E);
            case 0x0C:
                return RRC_R(ref H);
            case 0x0D:
                return RRC_R(ref L);
            case 0x0E:
                return RRC_AHL();
            case 0x0F:
                return RRC_R(ref A);
            case 0x10:
                return RL_R(ref B);
            case 0x11:
                return RL_R(ref C);
            case 0x12:
                return RL_R(ref D);
            case 0x13:
                return RL_R(ref E);
            case 0x14:
                return RL_R(ref H);
            case 0x15:
                return RL_R(ref L);
            case 0x16:
                return RL_AHL();
            case 0x17:
                return RL_R(ref A);
            case 0x18:
                return RR_R(ref B);
            case 0x19:
                return RR_R(ref C);
            case 0x1A:
                return RR_R(ref D);
            case 0x1B:
                return RR_R(ref E);
            case 0x1C:
                return RR_R(ref H);
            case 0x1D:
                return RR_R(ref L);
            case 0x1E:
                return RR_AHL();
            case 0x1F:
                return RR_R(ref A);
            case 0x20:
                return SLA_R(ref B);
            case 0x21:
                return SLA_R(ref C);
            case 0x22:
                return SLA_R(ref D);
            case 0x23:
                return SLA_R(ref E);
            case 0x24:
                return SLA_R(ref H);
            case 0x25:
                return SLA_R(ref L);
            case 0x26:
                return SLA_AHL();
            case 0x27:
                return SLA_R(ref A);
            case 0x28:
                return SRA_R(ref B);
            case 0x29:
                return SRA_R(ref C);
            case 0x2A:
                return SRA_R(ref D);
            case 0x2B:
                return SRA_R(ref E);
            case 0x2C:
                return SRA_R(ref H);
            case 0x2D:
                return SRA_R(ref L);
            case 0x2E:
                return SRA_AHL();
            case 0x2F:
                return SRA_R(ref A);
            case 0x30:
                return SWAP_R(ref B);
            case 0x31:
                return SWAP_R(ref C);
            case 0x32:
                return SWAP_R(ref D);
            case 0x33:
                return SWAP_R(ref E);
            case 0x34:
                return SWAP_R(ref H);
            case 0x35:
                return SWAP_R(ref L);
            case 0x36:
                return SWAP_AHL();
            case 0x37:
                return SWAP_R(ref A);
            case 0x38:
                return SRL_R(ref B);
            case 0x39:
                return SRL_R(ref C);
            case 0x3A:
                return SRL_R(ref D);
            case 0x3B:
                return SRL_R(ref E);
            case 0x3C:
                return SRL_R(ref H);
            case 0x3D:
                return SRL_R(ref L);
            case 0x3E:
                return SRL_AHL();
            case 0x3F:
                return SRL_R(ref A);
            case 0x40:
                return BIT_N_R(0, ref B);
            case 0x41:
                return BIT_N_R(0, ref C);
            case 0x42:
                return BIT_N_R(0, ref D);
            case 0x43:
                return BIT_N_R(0, ref E);
            case 0x44:
                return BIT_N_R(0, ref H);
            case 0x45:
                return BIT_N_R(0, ref L);
            case 0x46:
                return BIT_N_AHL(0);
            case 0x47:
                return BIT_N_R(0, ref A);
            case 0x48:
                return BIT_N_R(1, ref B);
            case 0x49:
                return BIT_N_R(1, ref C);
            case 0x4A:
                return BIT_N_R(1, ref D);
            case 0x4B:
                return BIT_N_R(1, ref E);
            case 0x4C:
                return BIT_N_R(1, ref H);
            case 0x4D:
                return BIT_N_R(1, ref L);
            case 0x4E:
                return BIT_N_AHL(1);
            case 0x4F:
                return BIT_N_R(1, ref A);
            case 0x50:
                return BIT_N_R(2, ref B);
            case 0x51:
                return BIT_N_R(2, ref C);
            case 0x52:
                return BIT_N_R(2, ref D);
            case 0x53:
                return BIT_N_R(2, ref E);
            case 0x54:
                return BIT_N_R(2, ref H);
            case 0x55:
                return BIT_N_R(2, ref L);
            case 0x56:
                return BIT_N_AHL(2);
            case 0x57:
                return BIT_N_R(2, ref A);
            case 0x58:
                return BIT_N_R(3, ref B);
            case 0x59:
                return BIT_N_R(3, ref C);
            case 0x5A:
                return BIT_N_R(3, ref D);
            case 0x5B:
                return BIT_N_R(3, ref E);
            case 0x5C:
                return BIT_N_R(3, ref H);
            case 0x5D:
                return BIT_N_R(3, ref L);
            case 0x5E:
                return BIT_N_AHL(3);
            case 0x5F:
                return BIT_N_R(3, ref A);
            case 0x60:
                return BIT_N_R(4, ref B);
            case 0x61:
                return BIT_N_R(4, ref C);
            case 0x62:
                return BIT_N_R(4, ref D);
            case 0x63:
                return BIT_N_R(4, ref E);
            case 0x64:
                return BIT_N_R(4, ref H);
            case 0x65:
                return BIT_N_R(4, ref L);
            case 0x66:
                return BIT_N_AHL(4);
            case 0x67:
                return BIT_N_R(4, ref A);
            case 0x68:
                return BIT_N_R(5, ref B);
            case 0x69:
                return BIT_N_R(5, ref C);
            case 0x6A:
                return BIT_N_R(5, ref D);
            case 0x6B:
                return BIT_N_R(5, ref E);
            case 0x6C:
                return BIT_N_R(5, ref H);
            case 0x6D:
                return BIT_N_R(5, ref L);
            case 0x6E:
                return BIT_N_AHL(5);
            case 0x6F:
                return BIT_N_R(5, ref A);
            case 0x70:
                return BIT_N_R(6, ref B);
            case 0x71:
                return BIT_N_R(6, ref C);
            case 0x72:
                return BIT_N_R(6, ref D);
            case 0x73:
                return BIT_N_R(6, ref E);
            case 0x74:
                return BIT_N_R(6, ref H);
            case 0x75:
                return BIT_N_R(6, ref L);
            case 0x76:
                return BIT_N_AHL(6);
            case 0x77:
                return BIT_N_R(6, ref A);
            case 0x78:
                return BIT_N_R(7, ref B);
            case 0x79:
                return BIT_N_R(7, ref C);
            case 0x7A:
                return BIT_N_R(7, ref D);
            case 0x7B:
                return BIT_N_R(7, ref E);
            case 0x7C:
                return BIT_N_R(7, ref H);
            case 0x7D:
                return BIT_N_R(7, ref L);
            case 0x7E:
                return BIT_N_AHL(7);
            case 0x7F:
                return BIT_N_R(7, ref A);
            case 0x80:
                return RES_N_R(0, ref B);
            case 0x81:
                return RES_N_R(0, ref C);
            case 0x82:
                return RES_N_R(0, ref D);
            case 0x83:
                return RES_N_R(0, ref E);
            case 0x84:
                return RES_N_R(0, ref H);
            case 0x85:
                return RES_N_R(0, ref L);
            case 0x86:
                return RES_N_AHL(0);
            case 0x87:
                return RES_N_R(0, ref A);
            case 0x88:
                return RES_N_R(1, ref B);
            case 0x89:
                return RES_N_R(1, ref C);
            case 0x8A:
                return RES_N_R(1, ref D);
            case 0x8B:
                return RES_N_R(1, ref E);
            case 0x8C:
                return RES_N_R(1, ref H);
            case 0x8D:
                return RES_N_R(1, ref L);
            case 0x8E:
                return RES_N_AHL(1);
            case 0x8F:
                return RES_N_R(1, ref A);
            case 0x90:
                return RES_N_R(2, ref B);
            case 0x91:
                return RES_N_R(2, ref C);
            case 0x92:
                return RES_N_R(2, ref D);
            case 0x93:
                return RES_N_R(2, ref E);
            case 0x94:
                return RES_N_R(2, ref H);
            case 0x95:
                return RES_N_R(2, ref L);
            case 0x96:
                return RES_N_AHL(2);
            case 0x97:
                return RES_N_R(2, ref A);
            case 0x98:
                return RES_N_R(3, ref B);
            case 0x99:
                return RES_N_R(3, ref C);
            case 0x9A:
                return RES_N_R(3, ref D);
            case 0x9B:
                return RES_N_R(3, ref E);
            case 0x9C:
                return RES_N_R(3, ref H);
            case 0x9D:
                return RES_N_R(3, ref L);
            case 0x9E:
                return RES_N_AHL(3);
            case 0x9F:
                return RES_N_R(3, ref A);
            case 0xA0:
                return RES_N_R(4, ref B);
            case 0xA1:
                return RES_N_R(4, ref C);
            case 0xA2:
                return RES_N_R(4, ref D);
            case 0xA3:
                return RES_N_R(4, ref E);
            case 0xA4:
                return RES_N_R(4, ref H);
            case 0xA5:
                return RES_N_R(4, ref L);
            case 0xA6:
                return RES_N_AHL(4);
            case 0xA7:
                return RES_N_R(4, ref A);
            case 0xA8:
                return RES_N_R(5, ref B);
            case 0xA9:
                return RES_N_R(5, ref C);
            case 0xAA:
                return RES_N_R(5, ref D);
            case 0xAB:
                return RES_N_R(5, ref E);
            case 0xAC:
                return RES_N_R(5, ref H);
            case 0xAD:
                return RES_N_R(5, ref L);
            case 0xAE:
                return RES_N_AHL(5);
            case 0xAF:
                return RES_N_R(5, ref A);
            case 0xB0:
                return RES_N_R(6, ref B);
            case 0xB1:
                return RES_N_R(6, ref C);
            case 0xB2:
                return RES_N_R(6, ref D);
            case 0xB3:
                return RES_N_R(6, ref E);
            case 0xB4:
                return RES_N_R(6, ref H);
            case 0xB5:
                return RES_N_R(6, ref L);
            case 0xB6:
                return RES_N_AHL(6);
            case 0xB7:
                return RES_N_R(6, ref A);
            case 0xB8:
                return RES_N_R(7, ref B);
            case 0xB9:
                return RES_N_R(7, ref C);
            case 0xBA:
                return RES_N_R(7, ref D);
            case 0xBB:
                return RES_N_R(7, ref E);
            case 0xBC:
                return RES_N_R(7, ref H);
            case 0xBD:
                return RES_N_R(7, ref L);
            case 0xBE:
                return RES_N_AHL(7);
            case 0xBF:
                return RES_N_R(7, ref A);
            case 0xC0:
                return SET_N_R(0, ref B);
            case 0xC1:
                return SET_N_R(0, ref C);
            case 0xC2:
                return SET_N_R(0, ref D);
            case 0xC3:
                return SET_N_R(0, ref E);
            case 0xC4:
                return SET_N_R(0, ref H);
            case 0xC5:
                return SET_N_R(0, ref L);
            case 0xC6:
                return SET_N_AHL(0);
            case 0xC7:
                return SET_N_R(0, ref A);
            case 0xC8:
                return SET_N_R(1, ref B);
            case 0xC9:
                return SET_N_R(1, ref C);
            case 0xCA:
                return SET_N_R(1, ref D);
            case 0xCB:
                return SET_N_R(1, ref E);
            case 0xCC:
                return SET_N_R(1, ref H);
            case 0xCD:
                return SET_N_R(1, ref L);
            case 0xCE:
                return SET_N_AHL(1);
            case 0xCF:
                return SET_N_R(1, ref A);
            case 0xD0:
                return SET_N_R(2, ref B);
            case 0xD1:
                return SET_N_R(2, ref C);
            case 0xD2:
                return SET_N_R(2, ref D);
            case 0xD3:
                return SET_N_R(2, ref E);
            case 0xD4:
                return SET_N_R(2, ref H);
            case 0xD5:
                return SET_N_R(2, ref L);
            case 0xD6:
                return SET_N_AHL(2);
            case 0xD7:
                return SET_N_R(2, ref A);
            case 0xD8:
                return SET_N_R(3, ref B);
            case 0xD9:
                return SET_N_R(3, ref C);
            case 0xDA:
                return SET_N_R(3, ref D);
            case 0xDB:
                return SET_N_R(3, ref E);
            case 0xDC:
                return SET_N_R(3, ref H);
            case 0xDD:
                return SET_N_R(3, ref L);
            case 0xDE:
                return SET_N_AHL(3);
            case 0xDF:
                return SET_N_R(3, ref A);
            case 0xE0:
                return SET_N_R(4, ref B);
            case 0xE1:
                return SET_N_R(4, ref C);
            case 0xE2:
                return SET_N_R(4, ref D);
            case 0xE3:
                return SET_N_R(4, ref E);
            case 0xE4:
                return SET_N_R(4, ref H);
            case 0xE5:
                return SET_N_R(4, ref L);
            case 0xE6:
                return SET_N_AHL(4);
            case 0xE7:
                return SET_N_R(4, ref A);
            case 0xE8:
                return SET_N_R(5, ref B);
            case 0xE9:
                return SET_N_R(5, ref C);
            case 0xEA:
                return SET_N_R(5, ref D);
            case 0xEB:
                return SET_N_R(5, ref E);
            case 0xEC:
                return SET_N_R(5, ref H);
            case 0xED:
                return SET_N_R(5, ref L);
            case 0xEE:
                return SET_N_AHL(5);
            case 0xEF:
                return SET_N_R(5, ref A);
            case 0xF0:
                return SET_N_R(6, ref B);
            case 0xF1:
                return SET_N_R(6, ref C);
            case 0xF2:
                return SET_N_R(6, ref D);
            case 0xF3:
                return SET_N_R(6, ref E);
            case 0xF4:
                return SET_N_R(6, ref H);
            case 0xF5:
                return SET_N_R(6, ref L);
            case 0xF6:
                return SET_N_AHL(6);
            case 0xF7:
                return SET_N_R(6, ref A);
            case 0xF8:
                return SET_N_R(7, ref B);
            case 0xF9:
                return SET_N_R(7, ref C);
            case 0xFA:
                return SET_N_R(7, ref D);
            case 0xFB:
                return SET_N_R(7, ref E);
            case 0xFC:
                return SET_N_R(7, ref H);
            case 0xFD:
                return SET_N_R(7, ref L);
            case 0xFE:
                return SET_N_AHL(7);
            case 0xFF:
                return SET_N_R(7, ref A);
            default:
                //Console.WriteLine("Unimplemented CB Opcode: " + suffix.ToString("X2") + " , PC: " + (PC-1).ToString("X4"));
                //Environment.Exit(1);
                //return 0;
        }
    }
}