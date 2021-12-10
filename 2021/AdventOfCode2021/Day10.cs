using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day10
{
    private List<string> lines;

    [SetUp]
    public void SetUp()
    {
        lines = File.ReadAllLines("Day10.txt").ToList();
    }

    private static readonly Dictionary<char, char> closingChunks = new()
    { 
        ['}'] = '{',
        [')'] = '(',
        [']'] = '[',
        ['>'] = '<',
    };

    [Test]
    public void Part1()
    {
        Dictionary<char, int> points = new() { ['}'] = 1197, [')'] = 3, [']'] = 57, ['>'] = 25137, };

        var syntaxErrorScore = 0;

        foreach (var line in lines)
        {
            var openingChunks = new Stack<char>();

            foreach (var chunk in line)
            {
                if (chunk is '(' or '{' or '[' or '<')
                {
                    openingChunks.Push(chunk);
                    continue;
                }

                var openingChunk = openingChunks.Pop();
                
                if (openingChunk != closingChunks[chunk])
                {
                    syntaxErrorScore += points[chunk];
                    break;
                }
            }
        }

        Assert.That(syntaxErrorScore, Is.EqualTo(243939));
    }

    [Test]
    public void Part2()
    {
        Dictionary<char, int> points = new() { ['('] = 1, ['['] = 2, ['{'] = 3, ['<'] = 4 };

        var scores = new List<long>();

        foreach (var line in lines)
        {
            var incompleteLine = true;
            var openingChunks = new Stack<char>();

            foreach (var chunk in line)
            {
                if (chunk is '(' or '{' or '[' or '<')
                {
                    openingChunks.Push(chunk);
                    continue;
                }

                var openingChunk = openingChunks.Pop();

                if (closingChunks[chunk] != openingChunk)
                {
                    incompleteLine = false;
                    break;
                }
            }

            if (incompleteLine)
            {
                long score = 0;

                while(openingChunks.Count > 0)
                {
                    score = (score * 5) + points[openingChunks.Pop()];
                }

                scores.Add(score);
            }
        }
              
        scores.Sort();

        var middleScore = scores[scores.Count / 2];

        Assert.That(middleScore, Is.EqualTo(2421222841));
    }
}