namespace CutilloRigby.Device.HMC6352;

public enum OperationalMode : byte
{
    /// <summary>
    /// (Operational Mode=0) This is the factory default mode. The HMC6352 waits for master device
    /// commands or change in operational mode. Receiving an “A” command (get data) will make the HMC6352 perform a
    /// measurement of sensors (magnetometers), compute the compensated magnetometer and heading data, and wait for the
    /// next read or command. No new measurements are done until another “A” command is sent. This mode is useful to get
    /// data on demand or at random intervals as long as the application can withstand the time delay in getting the data. 
    /// </summary>
    Standby = 0x00,

    /// <summary>
    /// (Operational Mode=1) In this mode the internal processor waits for “A” commands (get data), makes the
    /// measurements and computations, and waits for the next read command to output the data. After each read command, the
    /// HMC6352 automatically performs another get data routine and updates the data registers. This mode is designed to get
    /// data on demand without repeating “A” commands, and with the master device controlling the timing and data throughput.
    /// The tradeoff in this mode is the previous query latency for the advantage of an immediate read of data. 
    /// </summary>
    Query = 0x01,

    /// <summary>
    /// (Operational Mode=2) The HMC6352 performs continuous sensor measurements and data
    /// computations at selectable rates of 1Hz, 5Hz, 10Hz, or 20Hz, and updates the output data bytes. Subsequent “A”
    /// commands are un-necessary unless re-synchronization to the command is desired. Data reads automatically get the most
    /// recent updates. This mode is useful for data demanding applications.
    /// 
    /// The continuous mode measurement rate is selected by two bits in the operational mode selection byte, along with the
    /// mode selection and the periodic Set/Reset bit. The periodic Set/Reset function performs a re-alignment of the sensors
    /// magnetic domains in case of sensor perming (magnetic upset event), operating temperature shifts, and normal thermal
    /// agitation of the domains. Exposure of the HMC6352 to magnetic fields above 20 gauss (disturbing field threshold) leads to
    /// possible measurement inaccuracy or “stuck” sensor readings until the set/reset function is performed. With the periodic
    /// Set/Reset bit set, the set/reset function occurs every few minutes. 
    /// </summary>
    Continuous = 0x02,

    /// <summary>
    /// Not Valid.
    /// </summary>
    NotAllowed = 0x03,
}
