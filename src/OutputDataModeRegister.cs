namespace CutilloRigby.Device.HMC6352;

public sealed class OutputDataModeRegister
{
    private byte _value;

    private OutputDataModeRegister(byte value)
    {
        _value = value;
        _value &= 0x07;
    }

    public OutputDataMode Value
    {
        get => (OutputDataMode)(_value & 0x07);
        set
        {
            _value &= 0x00;
            _value |= (byte)value;
        }
    }

    public const byte FactoryDefault = 0x00;
    
    public static implicit operator OutputDataModeRegister(byte value) => new OutputDataModeRegister(value);
    public static implicit operator byte(OutputDataModeRegister outMode) => outMode._value;
}
