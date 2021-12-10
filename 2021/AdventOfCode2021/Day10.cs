using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day10
{
    List<string> lines;

    [SetUp]
    public void SetUp()
    {
        lines = File.ReadAllLines("Day10.txt").ToList();
    }

    private static Dictionary<char, char> match = new()
    { 
        ['}'] = '{',
        [')'] = '(',
        [']'] = '[',
        ['>'] = '<',
    };

    private static Dictionary<char, int> points = new()
    {
        ['}'] = 1197,
        [')'] = 3,
        [']'] = 57,
        ['>'] = 25137,

        ['{'] = 1197,
        ['('] = 3,
        ['['] = 57,
        ['<'] = 25137,
    };

    private static Dictionary<char, int> points2 = new()
    {
        ['('] = 1,
        ['['] = 2,
        ['{'] = 3,
        ['<'] = 4,
    };
    [Test]
    public void Part1()
    {
        var result = 0;

        foreach (var line in lines)
        {
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                if (c is '(' or '{' or '[' or '<') stack.Push(c);
                else
                {
                    var o = stack.Pop();
                    if (match[c] != o)
                    {
                        result += points[c];
                        break;
                    }
                }
            }
        }

        Assert.That(result, Is.EqualTo(243939));
    }

    [Test]
    public void Part2()
    {
        var scores = new List<long>();

        foreach (var line in lines)
        {
            var incomplete = true;
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                if (c is '(' or '{' or '[' or '<') stack.Push(c);
                else
                {
                    var o = stack.Pop();
                    if (match[c] != o)
                    {
                        incomplete = false;
                        break;
                    }
                }
            }

            if (incomplete)
            {
                long score = 0;
                while(stack.Count > 0)
                {
                    var c = stack.Pop();
                    score = (score * 5) + points2[c];
                }
                scores.Add(score);
            }
        }
              
        scores.Sort();

        foreach (var score in scores)
        {
            Console.WriteLine(score);
        }

        var middle = scores[scores.Count / 2];

        Assert.That(middle, Is.EqualTo(2421222841));
    }
}