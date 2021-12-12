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

    // TODO: Refactor into two tests
    [Test]
    public void Part1()
    {
        var result = 0;
        var p = "start";
        var visited = new Stack<string>();
        visited.Push(p);

        Explore(p);

        Assert.That(result, Is.EqualTo(123054)); // Part 2 answer is 123054

        List<string> GetNext(string from)
        {
            return input
                .Where(node => node.From == from)
                .Select(node => node.To)

                // Part 1
                //.Where(next =>
                //{
                //    if (next == "start")
                //    {
                //        return false;
                //    }

                //    if (next.ToLower() == next)
                //    {
                //        return visited.Count(y => y == next) < 1;
                //    }

                //    return true;
                //})

                .Where(next =>
                {
                    if (next == "start")
                    {
                        return false;
                    }

                    if (next.ToLower() == next)
                    {
                        var lowers = visited.Where(x => x.ToLower() == x);

                        if (lowers.Count() > lowers.Distinct().Count())
                        {
                            return lowers.Count(y => y == next) < 1;
                        }

                        return lowers.Count(y => y == next) < 2;
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