using CutilloRigby.Device.HMC6352;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

using var hmc6352 = HMC6352.Create();

//await hmc6352.Calibrate(TimeSpan.FromMinutes(3), cts.Token);

var opMode = hmc6352.GetOperationalMode();
opMode.Mode = OperationalMode.Continuous;
opMode.PeriodicSetReset = true;
opMode.Rate = ContinuousModeMeasurementRate._10Hz;

hmc6352.SetOperationalMode(opMode);

Console.WriteLine($"OpMode: {opMode.Mode}");

while(!cts.IsCancellationRequested)
{
    Console.WriteLine($"{hmc6352.Heading.Degrees:n2}°");
    Thread.Sleep(500);
}
