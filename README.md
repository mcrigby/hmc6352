# Introduction 
Digital Compass Solution HMC6352

The Honeywell HMC6352 is a fully integrated compass
module that combines 2-axis magneto-resistive sensors
with the required analog and digital support circuits, and
algorithms for heading computation. By combining the
sensor elements, processing electronics, and firmware in
to a 6.5mm by 6.5mm by 1.5mm LCC package,
Honeywell offers a complete, ready to use electronic
compass. This provides design engineers with the
simplest solution to integrate high volume, cost effective
compasses into wireless phones, consumer electronics,
vehicle compassing, and antenna positioning.

# Getting Started
using var hmc6352 = HMC6352.Create();

var opMode = hmc6352.GetOperationalMode();
opMode.Mode = OperationalMode.Continuous;
opMode.PeriodicSetReset = true;
opMode.Rate = ContinuousModeMeasurementRate._10Hz;

hmc6352.SetOperationalMode(opMode);

while(true)
{
    Console.WriteLine($"{hmc6352.Heading.Degrees:n2}°");
    Thread.Sleep(500);
}
