namespace CutilloRigby.Device.HMC6352;

public static class HMC6352Extensions
{
    /// <summary>
    /// The HMC6352 provides a user calibration routine with the “C” command permitting entry into the calibration mode and the
    /// “E” command to exit the calibration mode. Once in calibration mode, the user is requested to rotate the compass on a flat
    /// surface at least one full circular rotation while the HMC6352 collects several readings per second at various headings with
    /// the emphasis on rotation smoothness to gather uniformly spaced readings. Optimally two rotations over 20 seconds
    /// duration would provide an accurate calibration. The calibration time window is recommended to be from 6 seconds up to
    /// 3 minutes depending on the end user’s platform.
    /// 
    /// The calibration routine collects these readings to correct for hard-iron distortions of the earth’s magnetic field. These 
    /// hard-iron effects are due to magnetized materials nearby the HMC6352 part that in a fixed position with respect to the end user
    /// platform. An example would be the magnetized chassis or engine block of a vehicle in which the compass is mounted
    /// onto. Upon exiting the calibration mode, the resulting magnetometer offsets and scaling factors are updated
    /// </summary>
    public static async Task Calibrate(this HMC6352 hmc6352, TimeSpan duration, CancellationToken cancellationToken = default,
        Action? beginCalibration = null, Action<uint>? calibrationInProgress = null, Action? finalisingCalibration = null,
        Action? endCalibration = null, Action<string>? log = null)
    {
        const ushort minimumCalibrationTime = 6000;
        const uint maximumCalibrationTime = 180000;
        const ushort calibrationStepTime = 500;

        uint calibrationTime = 0;

        if (log == null)
            log = s => { };
        if (calibrationInProgress == null)
            calibrationInProgress = d => { };

        if (duration.TotalMilliseconds > maximumCalibrationTime)
        {
            duration = TimeSpan.FromMilliseconds(maximumCalibrationTime);
            log($"Calibration time reduced to {maximumCalibrationTime}ms.");
        }

        log("Rotate the compass on a flat surface once every 10 seconds.");

        var timeDelay = hmc6352.GetTimeDelay();
        var summedMeasurements = hmc6352.GetSummedMeasurements();

        try
        {
            hmc6352.SetTimeDelay(10);
            hmc6352.SetSummedMeasurements(10);

            log("Beginning HMC6352 Calibration.");
            beginCalibration?.Invoke();

            hmc6352.BeginCalibration();

            while(!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(calibrationStepTime);
                calibrationTime += calibrationStepTime;
                calibrationInProgress(calibrationTime);
                log($"HMC6352 Calibration in Progress. {calibrationTime}ms Elapsed.");
            }
        }
        finally
        {
            log("Finalising HMC6352 Calibration.");
            finalisingCalibration?.Invoke();

            if (calibrationTime < minimumCalibrationTime)
                await Task.Delay ((int)(minimumCalibrationTime - calibrationTime));
            
            hmc6352.EndCalibration();
            endCalibration?.Invoke();
            log("Calibration HMC6352 Complete.");

            hmc6352.SetTimeDelay(timeDelay);
            hmc6352.SetSummedMeasurements(summedMeasurements);
        }
    }
}
