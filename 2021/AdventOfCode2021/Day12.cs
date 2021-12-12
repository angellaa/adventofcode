using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day12
{
    List<(string From, string To)> input;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day12.txt").Select(x => (x.Split("-")[0], x.Split("-")[1])).ToList();
        input.AddRange(input.Select(x => (x.To, x.From)).ToList());
    }

    [Test]
    public void Part1()
    {
        var result = 0;
        var p = "start";
        var visited = new Stack<string>();
        visited.Push(p);

        Explore(p);

        Assert.That(result, Is.EqualTo(4749));

        List<string> GetNext(string from)
        {
            return input
                .Where(node => node.From == from)
                .Select(node => node.To)
                .Where(next =>
                {
                    if (next == "start")
                    {
                        return false;
                    }

                    if (next.ToLower() == next)
                    {
                        return visited.Count(y => y == next) < 1;
                    }

                    return true;
                })                
                .ToList();
        }

        void Explore(string p)
        {
            if (p == "end")
            {
                result++;
                Print();
                return;
            }

            var next = GetNext(p);

            foreach (var n in next)
            {
                visited.Push(n);
                Explore(n);
                visited.Pop();
            }
        }

        void Print()
        {
            var path = "";
            foreach (var v in visited)
            {
                path = v + " " + path;
            }

            Console.WriteLine(path);
        }
    }


    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }
}