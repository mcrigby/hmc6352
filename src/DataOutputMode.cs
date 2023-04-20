namespace CutilloRigby.Device.HMC6352;

public enum OutputDataMode
{
    /// <summary>
    /// The heading output data will be the value in tenths of degrees from zero to 3599 and provided in binary
    /// format over the two bytes. 
    /// </summary>
    Heading = 0x00,
    
    /// <summary>
    /// The X raw magnetometer data readings are the internal sensor values measured
    /// at the output of amplifiers A and B respectively and are 10-bit 2’s complement binary ADC counts of the analog voltages
    /// at pins CA1 and CB1. The leading 6-bits on the MSB are zero filled or complemented for negative values. The zero count
    /// value will be about half of the supply voltage. If measurement averaging is implemented, the most significant bits may
    /// contain values of the summed readings. 
    /// </summary>
    Raw_X = 0x01,

    /// <summary>
    /// The Y raw magnetometer data readings are the internal sensor values measured
    /// at the output of amplifiers A and B respectively and are 10-bit 2’s complement binary ADC counts of the analog voltages
    /// at pins CA1 and CB1. The leading 6-bits on the MSB are zero filled or complemented for negative values. The zero count
    /// value will be about half of the supply voltage. If measurement averaging is implemented, the most significant bits may
    /// contain values of the summed readings. 
    /// </summary>
    Raw_Y = 0x02,

    /// <summary>
    /// The X magnetometer data readings are the raw magnetometer readings plus offset and
    /// scaling factors applied. The data format is the same as the raw magnetometer data. These compensated data values
    /// come from the calibration routine factors plus additional offset factors provided by the set/reset routine
    /// </summary>
    X = 0x03,

    /// <summary>
    /// The Y magnetometer data readings are the raw magnetometer readings plus offset and
    /// scaling factors applied. The data format is the same as the raw magnetometer data. These compensated data values
    /// come from the calibration routine factors plus additional offset factors provided by the set/reset routine
    /// </summary>
    Y = 0x04
}
