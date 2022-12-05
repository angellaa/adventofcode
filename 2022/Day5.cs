using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day5
{
    private List<Stack<char>> stacks;
    private List<(int n, int from, int to)> moves;

    [SetUp]
    public void Setup()
    {
        stacks = new List<Stack<char>>
        {
            "TPZCSLQN".ToStack(),
            "LPTVHCG".ToStack(),
            "DCZF" .ToStack(),
            "GWTDLMVC".ToStack(),
            "PWC".ToStack(),
            "PFJDCTSZ".ToStack(),
            "VWGBD".ToStack(),
            "NJSQHW".ToStack(),
            "RCQFSLV".ToStack()
        };

        moves = new List<(int, int, int)>();

        foreach (var line in File.ReadAllLines("Input.txt").Skip(10))
        {
            var groups = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups;

            moves.Add((int.Parse(groups[1].Value), 
                       int.Parse(groups[2].Value) - 1, 
                       int.Parse(groups[3].Value) - 1));
        }
    }

    [Test]
    public void Part1()
    {
        foreach (var (n, from, to) in moves)
        {
            for (var i = 0; i < n; i++)
            {
                stacks[to].Push(stacks[from].Pop());
            }
        }

        Assert.That(TopCrates(), Is.EqualTo("SVFDLGLWV"));
    }
    
    [Test]
    public void Part2()
    {
        foreach (var (n, from, to) in moves)
        {
            var tempStack = new Stack<char>();
            for (var i = 0; i < n; i++) tempStack.Push(stacks[from].Pop());
            for (var i = 0; i < n; i++) stacks[to].Push(tempStack.Pop());
        }

        Assert.That(TopCrates(), Is.EqualTo("DCVTCVPCL"));
    }

    private string TopCrates() => stacks.Aggregate("", (x, stack) => x + stack.Peek());
}