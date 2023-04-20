using System.Device.I2c;
using UnitsNet;

namespace CutilloRigby.Device.HMC6352;

public sealed class HMC6352 : IDisposable
{
    private readonly I2cDevice _i2cDevice;

    private OperationalModeRegister _operationalMode;
    private OutputDataModeRegister _outputMode;

    /// <summary>
    /// Creates a new instance of the HMC6352
    /// </summary>
    /// <param name="i2cDevice">The I2C device used for communication.</param>
    public HMC6352(I2cDevice i2cDevice)
    {
        _i2cDevice = i2cDevice ?? throw new ArgumentNullException(nameof(i2cDevice));

        Disabled = false;

        if (!Initialise())
            throw new Exception("Non-Production Software Version");

        _operationalMode = ReadByte(Command.RAM_Read, (byte)RAM.Operational_Mode);
        _outputMode = ReadByte(Command.RAM_Read, (byte)RAM.Output_Mode);
        GetHeading = () => 0;
        BuildGetHeading();
    }

    /// <summary>
    /// The I2C slave address byte consists of the 7 most significant bits with the least siginificant bit zero filled. As described
    /// earlier, the default (factory) value is 42(hex) and the legal I2C bounded values are between 10(hex) and F6(hex). This
    /// slave address is written into EEPROM address 00(hex) and changed on the power up. 
    /// </summary>
    public const byte DefaultI2CAddress = 0x21;

    /// <summary>
    /// HMC6352 Heading
    /// </summary>
    public Angle Heading { get => Angle.FromDegrees(GetHeading() / 10.0); }

    private bool _disabled;

    /// <summary>
    /// Disable HMC6352
    /// </summary>
    public bool Disabled
    {
        get => _disabled;
        set
        {
            SetShutdown(value);
        }
    }

    /// <summary>
    /// Checks if the device is a HMC6352
    /// </summary>
    /// <returns>True if device has been correctly detected</returns>
    private bool Initialise()
    {
        if (ReadByte(Command.EEPROM_Read, (byte)EEPROM.Software_Version) == 0x00)
            return false;

        UpdateBridgeOffsets();

        return true;
    }

    /// <summary>
    /// The I2C slave address byte consists of the 7 most significant bits with the least siginificant bit zero filled. As described
    /// earlier, the default (factory) value is 42(hex) and the legal I2C bounded values are between 10(hex) and F6(hex). This
    /// slave address is written into EEPROM address 00(hex) and changed on the power up. 
    /// </summary>
    public bool SetI2CAddress(byte i2cAddress)
    {
        if (i2cAddress < 0x10 || 0xf6 < i2cAddress)
            return false;

        WriteByte(Command.EEPROM_Write, (byte)EEPROM.I2C_Slave_Address, i2cAddress);
        return true;
    }

    /// <summary>
    /// Return the Operational Mode
    /// </summary>
    /// <returns>Operational Mode</returns>
    public OperationalModeRegister GetOperationalMode() => _operationalMode;

    /// <summary>
    /// The HMC6352 has three operational modes plus the ability to enter/exit the non-operational (sleep) mode by command.
    /// Sleep mode sends the internal microprocessor into clock shutdown to save power, and can be brought back by the “W”
    /// command (wake). The “S” command returns the processor to sleep mode. The three operational modes are defined by
    /// two bits in the internal HMC6352 Operation Mode register. If the master device sends the “L” command, the current
    /// operational mode control byte in the RAM register is loaded into the internal EEPROM register and becomes the default
    /// operational mode on the next power-up. The application environment of the HMC6352 will dictate the most suitable
    /// operational mode. 
    /// </summary>
    public void SetOperationalMode(OperationalModeRegister operationalMode, bool saveToEEPROM = false)
    {
        _operationalMode = operationalMode;
        WriteByte(Command.RAM_Write, (byte)RAM.Operational_Mode, _operationalMode);
        BuildGetHeading();

        if (saveToEEPROM) WriteCommand(Command.Save_Op_Mode);
    }

    /// <summary>
    /// Return the Output Data Mode
    /// </summary>
    /// <returns>Output Mode</returns>
    public OutputDataModeRegister GetOutputDataMode() => _outputMode;
    /// <summary>
    /// The read response bytes after an “A” command, will cause the HMC6352 will return two bytes with binary formatted data.
    /// Either heading or magnetometer data can be retrieved depending on the output data selection byte value. Negative
    /// signed magnetometer data will be returned in two’s complement form. This output data control byte is located in RAM
    /// register location 4E(hex) and defaults to value zero (heading) at power up. 
    /// </summary>
    public void SetOutputDataMode(OutputDataModeRegister value)
    {
        _outputMode = value;
        WriteByte(Command.RAM_Write, (byte)RAM.Output_Mode, _outputMode);
        BuildGetHeading();
    }

    /// <summary>
    /// Wakes-up the device
    /// </summary>
    public void Wake() => SetShutdown(false);

    /// <summary>
    /// Shuts down the device
    /// </summary>
    public void Sleep() => SetShutdown(true);

    /// <summary>
    /// Begin User Calibration
    /// </summary>
    public void BeginCalibration() => WriteCommand(Command.Calibration_Start, true);

    /// <summary>
    /// End User Calibration
    /// </summary>
    public void EndCalibration() => WriteCommand(Command.Calibration_End, true);

    public void UpdateBridgeOffsets() => WriteCommand(Command.Set_Reset, true);

    public byte GetSummedMeasurements() => ReadByte(Command.EEPROM_Read, (byte)EEPROM.Summed_Measurements);
    /// <summary>
    /// This EEPROM summed measurement byte permits designers/users to back average or data smooth the output data
    /// (heading, magnetometer values) to reduce the amount of jitter in the data presentation. The default value is 04(hex) which
    /// is four measurements summed. A value of 00(hex) would be no summing. Up to 16 sets of magnetometer data may be
    /// selected for averaging. This slave address is written into EEPROM address 06(hex) and loaded to RAM on the power up. 
    /// </summary>
    public void SetSummedMeasurements(byte value = 0x04) =>
        WriteByte(Command.EEPROM_Write, (byte)EEPROM.Summed_Measurements, value &= 0x0F);
    
    public byte GetTimeDelay() => ReadByte(Command.EEPROM_Read, (byte)EEPROM.Time_Delay);
    /// <summary>
    /// The EEPROM time delay byte is the binary value of the number of milliseconds from the time a measurement request was
    /// commanded and the time the actual measurements are made. The default value is 01(hex) for no delay. Extra
    /// measurement delays maybe desired to allow for amplifier stabilization from immediate HMC6352 power-up or for external
    /// filter capacitor selection that limits the bandwidth and time response of the amplifier stages. This value is written into
    /// EEPROM address 05(hex) and loaded to RAM on the power up. 
    /// </summary>
    public void SetTimeDelay(byte value = 0x01) =>
        WriteByte(Command.EEPROM_Write, (byte)EEPROM.Time_Delay, value);
    
    /// <summary>
    /// Read HMC6352 Heading (degrees)
    /// </summary>
    /// <returns>Heading in millidegrees</returns>
    public Func<ushort> GetHeading;

    private void BuildGetHeading()
    {
        if (_outputMode.Value != OutputDataMode.Heading)
        {
            GetHeading = () => 0;
            return;
        }

        GetHeading = _operationalMode.Mode switch
        {
            OperationalMode.Standby => () => ReadShort(Command.Get_Data),
            OperationalMode.Query => () => {
                var result = ReadShort();
                WriteCommand(Command.Get_Data);
                return result;
            },
            OperationalMode.Continuous => () => ReadShort(),
            _ => () => 0,
        };
    }

    /// <summary>
    /// Set HMC6352 Shutdown
    /// </summary>
    /// <param name="isShutdown">Shutdown when value is true.</param>
    private void SetShutdown(bool isShutdown)
    {
        if (isShutdown)
            WriteCommand(Command.Sleep);
        else
            WriteCommand(Command.Wakeup);

        _disabled = isShutdown;
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    public void Dispose()
    {
        _i2cDevice?.Dispose();
    }

    internal void WriteCommand(Command command, bool delay = false)
    {
        _i2cDevice.WriteByte((byte)command);
        if (delay) Thread.Sleep(TimingDelay(command));
    }

    internal byte ReadByte(Command command, byte address)
    {
        _i2cDevice.Write(new byte[] { (byte)command, address });
        Thread.Sleep(TimingDelay_ReadWrite);
        return _i2cDevice.ReadByte();
    }

    internal void WriteByte(Command command, byte address, byte value)
    {
        _i2cDevice.Write(new byte[] { (byte)command, address, value });
        Thread.Sleep(TimingDelay_ReadWrite);
    }

    internal ushort ReadShort()
    {
        Span<byte> buf = stackalloc byte[2];
        _i2cDevice.Read(buf);

        return (ushort)(buf[0] << 8 | buf[1]);
    }
    internal ushort ReadShort(Command command)
    {
        WriteCommand(command, true);
        return ReadShort();
    }

    private const byte TimingDelay_ReadWrite = 70;

    private static short TimingDelay(Command command)
    {
        return command switch
        {
            Command.Sleep => 10,
            Command.Wakeup => 100,
            Command.Set_Reset => 6000,
            Command.Calibration_Start => 10,
            Command.Calibration_End => 14000,
            Command.Save_Op_Mode => 125,
            Command.Get_Data => 6000,
            //Command.EEPROM_Write => 70,
            //Command.EEPROM_Read => 70,
            //Command.RAM_Write => 70,
            //Command.RAM_Read => 70,
            _ => TimingDelay_ReadWrite,
        };
    }

    public static HMC6352 Create(byte address = DefaultI2CAddress, int busId = 1)
    {
        var settings = new I2cConnectionSettings(busId, address);
        var device = I2cDevice.Create(settings);

        return new HMC6352(device);
    }
}
