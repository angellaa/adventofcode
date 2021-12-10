using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day9
{
    private List<string> heightMap;
    private bool[,] visited;
    private int rows, columns;

    [SetUp]
    public void SetUp()
    {
        heightMap = File.ReadAllLines("Day9.txt").ToList();
        rows = heightMap.Count;
        columns = heightMap[0].Length;
    }

    [Test]
    public void Part1()
    {
        var result = 0;

        for (var r = 0; r < rows; r++)
        for (var c = 0; c < columns; c++)
        {
            result += Risk(r, c);
        }

        Assert.That(result, Is.EqualTo(478));

        int Risk(int r, int c)
        {
            var height = Value(r, c);

            return height < Value(r - 1, c)  && height < Value(r + 1, c) && height < Value(r, c - 1) && height < Value(r, c + 1) 
                    ? 1 + height 
                    : 0;
        }
    }

    [Test]
    public void Part2()
    {
        var basinSizes = new List<long>();

        for (var r = 0; r < rows; r++)
        for (var c = 0; c < columns; c++)
        {
            var value = Value(r, c);

            if (value < Value(r - 1, c) && value < Value(r + 1, c) && value < Value(r, c - 1) && value < Value(r, c + 1))
            {
                visited = new bool[rows, columns];
                var basinSize = BasinSize(r, c);

                basinSizes.Add(basinSize);
            }
        }

        var result = basinSizes.OrderByDescending(x => x).Take(3).Aggregate(1L, (x, y) => x * y);

        Assert.That(result, Is.EqualTo(1327014));

        int BasinSize(int r, int c)
        {
            if (Value(r, c) == 9 || Visited(r, c)) return 0;

            visited[r, c] = true;

            return 1 + BasinSize(r - 1, c) + BasinSize(r + 1, c) + BasinSize(r, c - 1) + BasinSize(r, c + 1);
        }

        bool Visited(int r, int c) => r < 0 || r >= rows || c < 0 || c >= columns || visited[r, c];
    }

    private int Value(int r, int c) => r < 0 || r >= rows || c < 0 || c >= columns ? int.MaxValue : heightMap[r][c] - '0';
}