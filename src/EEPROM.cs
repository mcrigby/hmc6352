namespace CutilloRigby.Device.HMC6352;

/// <summary>
/// HMS6352 EEPROM Registers
/// </summary>
internal enum EEPROM : byte
{
    /// <summary>
    /// I2C Slave Address (Factory Default: 0x42)
    /// </summary>
    I2C_Slave_Address = 0x00,
    /// <summary>
    /// Magnetometer X Offset MSB (Factory Default: Factory Test Value)
    /// </summary>
    Magnetometer_X_Offset_MSB = 0x01,
    /// <summary>
    /// Magnetometer X Offset LSB (Factory Default: Factory Test Value)
    /// </summary>
    Magnetometer_X_Offset_LSB = 0x02,
    /// <summary>
    /// Magnetometer Y Offset MSB (Factory Default: Factory Test Value)
    /// </summary>
    Magnetometer_Y_Offset_MSB = 0x03,
    /// <summary>
    /// Magnetometer Y Offset LSB (Factory Default: Factory Test Value)
    /// </summary>
    Magnetometer_Y_Offset_LSB = 0x04,
    /// <summary>
    /// Time Delay (0 - 255ms) (Factory Default: 0x01)
    /// </summary>
    Time_Delay = 0x05,
    /// <summary>
    /// Number of Summed Measurements (1 - 16) (Factory Default: 0x04)
    /// </summary>
    Summed_Measurements = 0x06,
    /// <summary>
    /// Software Version Number (Factory Default: > 0x01)
    /// </summary>
    Software_Version = 0x07,
    /// <summary>
    /// Operation Mode Byte (Factory Default: 0x50)
    /// </summary>
    Operation_Mode = 0x08,
}