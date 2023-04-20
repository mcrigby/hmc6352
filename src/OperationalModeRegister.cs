namespace CutilloRigby.Device.HMC6352;

public sealed class OperationalModeRegister
{
    private byte _value;

    private OperationalModeRegister(byte value)
    {
        _value = value;
        _value &= 0x73;

        if (Mode == OperationalMode.NotAllowed)
            Mode = OperationalMode.Standby;
    }

    public OperationalMode Mode
    {
        get => (OperationalMode)(_value & 0x03);
        set
        {
            if (value == OperationalMode.NotAllowed)
                return;
            
            _value &= 0x70;
            _value |= (byte)value;
        }
    }

    public ContinuousModeMeasurementRate Rate
    {
        get => (ContinuousModeMeasurementRate)(_value & 0x60);
        set
        {
            _value &= 0x13;
            _value |= (byte)value;
        }
    }

    public bool PeriodicSetReset
    {
        get => (_value & 0x10) > 0;
        set
        {
            if (value)
                _value |= 0x10;
            else
                _value &= 0x63;
        }
    }

    public const byte FactoryDefault = 0x50;
    
    public static OperationalModeRegister Default() => new OperationalModeRegister(FactoryDefault);

    public static implicit operator OperationalModeRegister(byte value) => new OperationalModeRegister(value);
    public static implicit operator byte(OperationalModeRegister opMode) => opMode._value;
}
