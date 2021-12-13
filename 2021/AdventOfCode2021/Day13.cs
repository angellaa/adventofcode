using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day13
{
    List<(int X, int Y)> dots;
    List<(string FoldAlong, int Value)> folds;
    int n;
    int m;
    bool[,] map;

    [SetUp]
    public void SetUp()
    {
        var lines = File.ReadAllLines("Day13.txt");
        dots = lines.TakeWhile(x => x != "").Select(x => (int.Parse(x.Split(",")[0]), int.Parse(x.Split(",")[1]))).ToList();
        folds = lines.TakeLast(lines.Count() - dots.Count() - 1)
                        .Select(x => x.Substring(11))
                        .Select(x => (x.Split("=")[0], int.Parse(x.Split("=")[1])))
                        .ToList();
        n = dots.Select(x => x.Y).Max() + 1;
        m = dots.Select(x => x.X).Max() + 1;
    }

    [Test]
    public void Part1()
    {
        map = new bool[n, m];

        foreach (var (x, y) in dots)
        {
            map[y, x] = true;
        }

        //Print();

        int visibleDots = 0;


        foreach (var fold in folds)
        {
            if (fold.FoldAlong == "y")
            {
                for (var y = 0; y < fold.Value; y++)
                    for (var x = 0; x < m; x++)
                    {
                        var v = map[y + fold.Value + 1, x];

                        if (v)
                        {
                            map[fold.Value - y - 1, x] = v;
                            map[y + fold.Value + 1, x] = false;
                        }

                        map[fold.Value, x] = false;
                    }

                n = fold.Value;
            }

            if (fold.FoldAlong == "x")
            {
                for (var y = 0; y < n; y++)
                    for (var x = 0; x < fold.Value; x++)
                    {
                        var v = map[y, x + fold.Value + 1];

                        if (v)
                        {
                            map[y, fold.Value - x - 1] = v;
                            map[y, x + fold.Value + 1] = false;
                        }

                        map[y, fold.Value] = false;
                    }

                m = fold.Value;
            }

            visibleDots = map.Cast<bool>().Count(x => x == true);
            
            Console.WriteLine(visibleDots);
        }

        Print(); // RZKZLPGH

        Assert.That(visibleDots, Is.EqualTo(0)); 
    }

    void Print()
    {
        for (var y = 0; y < n; y++)
        {
            for (var x = 0; x < m; x++)
                Console.Write(map[y, x] ? '#' : '.');
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }
}