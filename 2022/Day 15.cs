using System.Text.RegularExpressions;

var sensors = new List<(int X, int Y)>();
var beacons = new List<(int X, int Y)>();
var beaconsBySensor = new Dictionary<(int X, int Y), (int X, int Y)>();
var largestDistance = 0;

foreach (var line in  File.ReadAllLines("Input.txt"))
{
    var m = Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

    var sensor = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
    var beacon = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));

    sensors.Add(sensor);
    beacons.Add(beacon);
    beaconsBySensor[sensor] = beacon;

    if (Distance(sensor, beacon) > largestDistance) largestDistance = Distance(sensor, beacon);
}

var minX = Math.Min(sensors.MinBy(x => x.X).X, beacons.MinBy(x => x.X).X) - largestDistance;
var maxX = Math.Min(sensors.MaxBy(x => x.X).X, beacons.MaxBy(x => x.X).X) + largestDistance;

var y = 2000000;

var result = 0;

for (var x = minX; x <= maxX; x++)
{
    var p = (x, y);

    if (!beacons.Contains(p) && !sensors.Contains(p) && sensors.Any(s => Distance(s, p) <= Distance(s, beaconsBySensor[s])))
    {
        result++;
    }
}

Console.WriteLine(result);

int Distance((int X, int Y) p1, (int X, int Y) p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
