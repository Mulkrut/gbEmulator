using System.Transactions;

public class DMA
{

    private BUS bus;
    private int _ticks;
    private byte _DMA_Register = 0xff;
    private int from;

    private bool _transferInProgress;
    private bool _transferRestarted;

    public void SetBUS(BUS bus)
    {
        this.bus = bus;
    }

    public bool IsOamBlocked() => _transferRestarted || _transferInProgress && _ticks >= 5;

    public byte DMA_Register
    {
        get => _DMA_Register;
        set
        {
            from = value * 0x100;
            _ticks = 0;
            _transferInProgress = true;
            _transferRestarted = IsOamBlocked();
            _DMA_Register = value;
        }
    }

    public void Tick()
    {
        if (!_transferInProgress) return;
        if (++_ticks < 648) return;

        _transferInProgress = false;
        _transferRestarted = false;
        _ticks = 0;

        for (var i = 0; i < 0xA0; i++)
        {
            bus.WriteByte((ushort)(0xfe00 + i), bus.ReadByte((ushort)(from + i)));
        }
    }
















}