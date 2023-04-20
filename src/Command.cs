namespace CutilloRigby.Device.HMC6352;

/// <summary>
/// HMC6352 I2C Commands
/// </summary>
internal enum Command : byte
{
    /// <summary>
    /// w - Write to EEPROM
    /// </summary>
    EEPROM_Write = 0x77,
    /// <summary>
    /// r - Read from EEPROM
    /// </summary>
    EEPROM_Read = 0x72,
    /// <summary>
    /// G - Write to RAM Register
    /// </summary>
    RAM_Write = 0x47,
    /// <summary>
    /// g - Read from RAM Register
    /// </summary>
    RAM_Read = 0x67,
    /// <summary>
    /// S - Enter Sleep Mode (Sleep)
    /// </summary>
    Sleep = 0x53,
    /// <summary>
    /// W - Exit Sleep Mode (Wakeup)
    /// </summary>
    Wakeup = 0x57,
    /// <summary>
    /// O - Update Bride Offsets (S/R Now)
    /// </summary>
    Set_Reset = 0x4f,
    /// <summary>
    /// C - Enter User Calibration Mode
    /// </summary>
    Calibration_Start = 0x43,
    /// <summary>
    /// E - Exit User Calibtration Mode
    /// </summary>
    Calibration_End = 0x45,
    /// <summary>
    /// L - Save Op Mode to EEPROM
    /// </summary>
    Save_Op_Mode = 0x4c,
    /// <summary>
    /// A - Get Data. Compensate and Calculate New Heading
    /// </summary>
    Get_Data = 0x41,
}