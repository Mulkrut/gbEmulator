public partial class CPU
{
    public delegate void Step();
    //idk what delegate does tbh

    public class Opcode(byte value, string label, int length, int tcycles, Step[] steps)
    {
        public byte value = value;
        public string label = label;
        public int length = length;
        public int tcycles = tcycles;
        public int mcycles = tcycles / 4;
        public Step[] steps = steps;
    }
}