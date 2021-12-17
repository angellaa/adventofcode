using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day17
{
    private int tx1;
    private int tx2;
    private int ty1;
    private int ty2;

    [SetUp]
    public void SetUp()
    {
        var text = File.ReadAllText("Day17.txt");
        text = text.Substring(15);
        var first = text.Split(',')[0];
        var second = text.Split(',')[1].Substring(3);
        tx1 = int.Parse(first.Split("..")[0]);
        tx2 = int.Parse(first.Split("..")[1]);
        ty1 = int.Parse(second.Split("..")[0]);
        ty2 = int.Parse(second.Split("..")[1]);
    }

    [Test]
    public void Part1()
    {
        var globalMax = int.MinValue;

        for (var vx = -200; vx < 200; vx++)
        {
            for (var vy = -200; vy < 200; vy++)
            {
                var (inTarget, maxY) = MaxY(vx, vy);

                if (inTarget)
                {
                    if (maxY > globalMax) globalMax = maxY;
                }
            }
        }

        Assert.That(globalMax, Is.EqualTo(6786));

        (bool InTarget, int MaxY) MaxY(int vx, int vy)
        {
            var x = 0;
            var y = 0;
            var maxY = int.MinValue;
            var steps = 1000;

            while (!InTarget(x, y) && steps > 0)
            {
                x += vx;
                y += vy;
                if (y > maxY) maxY = y;

                vx = vx switch { < 0 => vx + 1, > 0 => vx - 1, _ => 0 };
                vy -= 1;
                steps--;
            }

            return (InTarget(x, y), maxY);
        }
    }

    public bool InTarget(int x, int y)
    {
        return x >= tx1 && x <= tx2 && y >= ty1 && y <= ty2;
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }
}