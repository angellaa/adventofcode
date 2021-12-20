using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day19
{
    record Point(int X, int Y, int Z)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public int DistanceTo(Point p) => Math.Abs(p.X - X) + Math.Abs(p.Y - Y) + Math.Abs(p.Z - Z);

        public override string ToString() => $"{X},{Y},{Z}";
    }

    readonly List<List<Point>> scanners = new();

    [SetUp]
    public void SetUp()
    {
        var lines = File.ReadAllLines("Day19.txt").Skip(1);
        var scanner = new List<Point>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("---"))
            {
                scanners.Add(scanner.ToList());
                scanner.Clear();
            }
            else
            {
                var x = int.Parse(line.Split(",")[0]);
                var y = int.Parse(line.Split(",")[1]);
                var z = int.Parse(line.Split(",")[2]);

                scanner.Add(new Point(x, y, z));
            }
        }

        scanners.Add(scanner.ToList());
    }

    [Test]
    public void Part1()
    {
        var beacons = new HashSet<Point>();

        foreach (var p in scanners[0])
        {
            beacons.Add(p);
        }

        var scannersAbsolutePositions = new Dictionary<int, (Point Position, List<Point> AbsoluteBeacons)>
        {
            [0] = (new(0, 0, 0), scanners[0])
        };

        while (scannersAbsolutePositions.Count < scanners.Count)
        {
            foreach (var scannerIndex in scannersAbsolutePositions.Keys.ToList())
            {
                for (var i = 0; i < scanners.Count; i++)
                {
                    if (!scannersAbsolutePositions.Keys.Contains(i))
                    {
                        var result = ConvertToRelativePoints(
                            scannersAbsolutePositions[scannerIndex].AbsoluteBeacons.Select(x => x - scannersAbsolutePositions[scannerIndex].Position).ToList(), 
                            scanners[i]);

                        if (result.RelativeScannerPosition != null)
                        {
                            var newBeacons = result.RelativePoints.Select(x => x + scannersAbsolutePositions[scannerIndex].Position).ToList();
                            scannersAbsolutePositions[i] = (result.RelativeScannerPosition + scannersAbsolutePositions[scannerIndex].Position, newBeacons);

                            foreach (var p in newBeacons)
                            {
                                beacons.Add(p);
                            }
                        }
                    }
                }
            }
        }

        foreach (var beacon in beacons)
        {
            Console.WriteLine(beacon);
        }

        int max = int.MinValue;

        for (int i = 0; i < scannersAbsolutePositions.Count; i++)
        for (int j = i + 1; j < scannersAbsolutePositions.Count; j++)
        {
            max = Math.Max(max, scannersAbsolutePositions[i].Position.DistanceTo(scannersAbsolutePositions[j].Position));
        }

        Assert.That(beacons.Count, Is.EqualTo(447));
        Assert.That(max, Is.EqualTo(15672));
    }

[Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }

    private static (Point RelativeScannerPosition, List<Point> RelativePoints) ConvertToRelativePoints(List<Point> scan1, List<Point> scan2)
    {
        var diff1 = Diffs(scan1).ToList();

        var variants = Variants(scan2);

        foreach (var variant in variants)
        {
            var diff2 = Diffs(variant).ToList();

            var shared = diff1.Select(x => x.Diff).Intersect(diff2.Select(x => x.Diff)).ToList();
            
            if (shared.Count >= 12)
            {
                var maps = new HashSet<(Point From, Point To)>();

                foreach (var s in shared)
                {
                    var s1 = diff1.Single(x => x.Diff == s);
                    var s2 = diff2.Single(x => x.Diff == s);

                    maps.Add((s1.P1, s2.P1));
                    maps.Add((s1.P2, s2.P2));
                }

                Point scannerPosition = null;
                foreach (var map in maps)
                {
                    scannerPosition = (map.From - map.To);

                    Console.WriteLine(map.From + " => " + map.To + "\t\tEstimated " + scannerPosition);
                }
                Console.WriteLine();

                return (scannerPosition, variant.Select(x => x + scannerPosition).ToList());
            }
        }

        return new(null, new());
    }

    private static void Print(List<Point> diff1)
    {
        foreach (var point in diff1)
        {
            Console.WriteLine(point);
        }
    }

    private static IEnumerable<List<Point>> Variants(List<Point> points)
    {
        var z = points.Select(p =>
        {
            var (x, y, z) = p;

            return new List<Point>
            {
                new(x, y, z), 
                new(x, -z, y), 
                new(x, -y, -z), 
                new(x, z, -y),
                new(z, y, -x), 
                new(y, -z, -x), 
                new(-z, -y, -x), 
                new(-y, z, -x),
                new(-x, y, -z), 
                new(-x, -z, -y), 
                new(-x, -y, z), 
                new(-x, z, y),
                new(-z, y, x), 
                new(-y, -z, x), 
                new(z, -y, x), 
                new(y, z, x),
                new(-y, x, z), 
                new(z, x, y), 
                new(y, x, -z), 
                new(-z, x, -y),
                new(-y, -x, -z), 
                new(z, -x, -y), 
                new(y, -x, z), 
                new(-z, -x, y)
            };
        }).ToList();

        for (var i = 0; i < 24; i++)
        {
            yield return z.Select(x => x[i]).ToList();
        }
    }

    private static IEnumerable<(Point P1, Point P2, Point Diff)> Diffs(List<Point> ps)
    {
        for (var i = 0; i < ps.Count; i++)
            for (var j = i + 1; j < ps.Count; j++)
            {
                yield return (ps[i], ps[j], ps[i] - ps[j]);
            }
    }

    private static IEnumerable<(Point P1, Point P2, Point Diff)> AllDiffs(List<Point> ps)
    {
        for (var i = 0; i < ps.Count; i++)
            for (var j = 0; j < ps.Count; j++)
            {
                if (i == j) continue;

                yield return (ps[i], ps[j], ps[i] - ps[j]);
            }
    }
}