using NUnit.Framework;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021;

[TestFixture]
public class Day22
{
    record class Cube(bool on, int X1, int X2, int Y1, int Y2, int Z1, int Z2)
    {
        public long Size()
        {
            if (X1 > X2) throw new Exception();
            if (Y1 > Y2) throw new Exception();
            if (Z1 > Z2) throw new Exception();

            return ((X2 - X1) + 1L) * ((Y2 - Y1) + 1) * ((Z2 - Z1) + 1);
        }
    }

    List<Cube> cubes = new();
    bool[,,] grid = new bool[101,101,101];
    HashSet<(int, int, int)> onCubes = new();

    [SetUp]
    public void SetUp()
    {
        var lines = File.ReadAllLines("Day22.txt");

        foreach (var line in lines)
        {
            var m =Regex.Match(line, @"(.*) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)");

            var x1 = Math.Min(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
            var x2 = Math.Max(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));

            var y1 = Math.Min(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));
            var y2 = Math.Max(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

            var z1 = Math.Min(int.Parse(m.Groups[6].Value), int.Parse(m.Groups[7].Value));
            var z2 = Math.Max(int.Parse(m.Groups[6].Value), int.Parse(m.Groups[7].Value));

            cubes.Add(new(m.Groups[1].Value == "on", x1, x2, y1, y2, z1, z2));
        }
    }

    [Test]
    public void Part1()
    {
        foreach (var (on, X1, X2, Y1, Y2, Z1, Z2) in cubes)
        {
            var boarders = new[] { X1, X2, Y1, Y2, Z1, Z2 };

            if (boarders.All(x => x >= -50 && x <= 50))
            {
                for (int x = X1; x <= X2; x++)
                for (int y = Y1; y <= Y2; y++)
                for (int z = Z1; z <= Z2; z++)
                {
                    grid[x + 50, y + 50, z + 50] = on;
                }
            }
        }

        Assert.That(grid.Cast<bool>().Count(x => x == true), Is.EqualTo(0));
    }

    [Test]
    public void Part2()
    {
        var xs = cubes.Select(x => x.X1).Union(cubes.Select(x => x.X1+1)).Union(cubes.Select(x => x.X2)).Union(cubes.Select(x => x.X2+1)).Distinct().OrderBy(x => x).ToList();
        var ys = cubes.Select(x => x.Y1).Union(cubes.Select(x => x.Y1+1)).Union(cubes.Select(x => x.Y2)).Union(cubes.Select(x => x.Y2+1)).Distinct().OrderBy(y => y).ToList();
        var zs = cubes.Select(x => x.Z1).Union(cubes.Select(x => x.Z1+1)).Union(cubes.Select(x => x.Z2)).Union(cubes.Select(x => x.Z2+1)).Distinct().OrderBy(z => z).ToList();

        grid = new bool[xs.Count, ys.Count, zs.Count];

        foreach (var (on, X1, X2, Y1, Y2, Z1, Z2) in cubes)
        {
            var boarders = new[] { X1, X2, Y1, Y2, Z1, Z2 };

            if (boarders.All(x => x >= -50 && x <= 50))
            {
                for (var x = xs.IndexOf(X1); x < xs.IndexOf(X2+1); x++)
                for (var y = ys.IndexOf(Y1); y < ys.IndexOf(Y2+1); y++)
                for (var z = zs.IndexOf(Z1); z < zs.IndexOf(Z2+1); z++)
                {
                    grid[x, y, z] = on;
                }
            }
        }

        //for (var x = 0; x < xs.Count; x++)
        //{
        //    Console.WriteLine($"x = {xs[x]}");
        //    for (var y = 0; y < ys.Count; y++)
        //    {
        //        for (var z = 0; z < zs.Count; z++)
        //        {
        //            Console.Write(grid[x, y, z] ? "*" : ".");
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}

        BigInteger sum = 0;

        var vx = new HashSet<int>();
        var vy = new HashSet<int>();
        var vz = new HashSet<int>();

        for (var x = 0; x < xs.Count; x++)
        for (var y = 0; y < ys.Count; y++)
        for (var z = 0; z < zs.Count; z++)
        {
            if (grid[x, y, z])
            {
                sum += new BigInteger(xs[x + 1] - xs[x] + 1 - (vx.Contains(x + 1) ? 1 : 0) - (vx.Contains(x) ? 1 : 0)) *
                       new BigInteger(ys[y + 1] - ys[y] + 1 - (vy.Contains(y + 1) ? 1 : 0) - (vy.Contains(y) ? 1 : 0)) *
                       new BigInteger(zs[z + 1] - zs[z] + 1 - (vz.Contains(z + 1) ? 1 : 0) - (vz.Contains(z) ? 1 : 0));

                vx.Add(x);
                vx.Add(x + 1);

                vy.Add(y);
                vy.Add(y + 1);

                vz.Add(z);
                vz.Add(z + 1);
            }
        }

        Assert.That(sum, Is.EqualTo(0));

        //Cube Intersect(Cube a, Cube b)
        //{
        //    if (a == null || b == null) return null;

        //    var xs = Side(a.X1, a.X2, b.X1, b.X2);
        //    var ys = Side(a.Y1, a.Y2, b.Y1, b.Y2);
        //    var zs = Side(a.Z1, a.Z2, b.Z1, b.Z2);

        //    if (xs == default || ys == default || zs == default) return null;

        //    return new(true, xs.From, xs.To, ys.From, ys.To, zs.From, zs.To);
        //}

        //(int From, int To) Side(int x1, int x2, int x11, int x22)
        //{
        //    if (x22 < x1) return default;
        //    if (x11 > x2) return default;

        //    if (x11 < x1)
        //    {
        //        return x22 < x2 ? (x1, x22) : (x1, x2);
        //    }
        //    else
        //    {
        //        return x22 < x2 ? (x11, x22) : (x11, x2);
        //    }
        //}
    }
}