using System.Text.RegularExpressions;

var sensors = new List<(int X, int Y)>();
var beacons = new List<(int X, int Y)>();
var beaconsBySensor = new Dictionary<(int X, int Y), (int X, int Y)>();

foreach (var line in  File.ReadAllLines("Input.txt"))
{
    var m = Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

    var sensor = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
    var beacon = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));

    sensors.Add(sensor);
    beacons.Add(beacon);
    beaconsBySensor[sensor] = beacon;
}

var yRanges = new List<(int StartX, int EndX)>[4000000];

for (var i = 0; i < sensors.Count; i++)
{
    var sensor = sensors[i];
    var beacon = beacons[i];

    var d = Distance(sensor, beacon);

    for (var j = 0; j <= d; j++)
    {
        if (sensor.Y + j >= 0 && sensor.Y + j < 4000000)
        {
            (yRanges[sensor.Y + j] ??= new List<(int, int)>()).Add((sensor.X - d + j, sensor.X + d - j));
        }

        if (sensor.Y - j >= 0 && sensor.Y - j < 4000000)
        {
            (yRanges[sensor.Y - j] ??= new List<(int, int)>()).Add((sensor.X - d + j, sensor.X + d - j));
        }
    }
}

for (var y = 0; y < 4000000; y++)
{
    var start = y;

    yRanges[y].Sort();

    foreach (var range in yRanges[y])
    {
        for (var x = start; x < range.StartX; x++)
        {
            var p = (x, y);

            if (!beacons.Contains(p) && !sensors.Contains(p) && sensors.All(s => Distance(s, p) > Distance(s, beaconsBySensor[s])))
            {
                Console.WriteLine(p.x * (long)4000000 + p.y);
                return;
            }
        }

        start = Math.Max(start, range.EndX);
    }
}

int Distance((int X, int Y) p1, (int X, int Y) p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
