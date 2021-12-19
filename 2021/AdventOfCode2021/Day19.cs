using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day19
{
    record Point(int X, int Y, int Z)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
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
        Overlaps(scanners[0], scanners[1]);

        Assert.That(-1, Is.EqualTo(0));
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }

    private static List<Point> Overlaps(List<Point> scan1, List<Point> scan2)
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

                foreach (var map in maps)
                {
                    var point = (map.From - map.To);

                    Console.WriteLine(map.From + " => " + map.To + "\t\tEstimated " + point);
                }

                break;
            }
        }


        return new();
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

    private static IEnumerable<(Point P1, Point P2, Point Diff)> Diffs(List<Point> points)
    {
        for (var i = 0; i < points.Count; i++)
            for (var j = i + 1; j < points.Count; j++)
            {
                yield return (points[i], points[j], points[i] - points[j]);
            }
    }

    private static IEnumerable<(Point P1, Point P2, Point Diff)> AllDiffs(List<Point> points)
    {
        for (var i = 0; i < points.Count; i++)
            for (var j = 0; j < points.Count; j++)
            {
                if (i == j) continue;

                yield return (points[i], points[j], points[i] - points[j]);
            }
    }
}