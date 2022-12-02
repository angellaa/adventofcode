using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day2
{
    private List<(string First, string Second)> lines = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        lines = File.ReadAllLines("Day2.txt").Select(x => (x.Split()[0], x.Split()[1])).ToList();
    }

    [Test]
    public void Part1()
    {
        var total = 0;

        foreach (var line in lines)
        {
            var op = line.First switch
            {
                "A" => "R",
                "B" => "P",
                "C" => "S",
                _ => ""
            };

            var you = line.Second switch
            {
                "X" => "R",
                "Y" => "P",
                "Z" => "S",
                _ => ""
            };

            var score = scorePerShape(you) + (s: you, f: op) switch
            {
                ("R", "P") => 0,
                ("R", "S") => 6,
                ("P", "S") => 0,
                ("P", "R") => 6,
                ("S", "P") => 6,
                ("S", "R") => 0,
                _ => 3
            };

            int scorePerShape(string shape) => shape switch
            {
                "R" => 1,
                "P" => 2,
                "S" => 3,
                _ => 0
            };

            total += score;
        }

        Assert.That(total, Is.EqualTo(8890));
    }

    [Test]
    public void Part2()
    {
        var total = 0;

        foreach (var line in lines)
        {
            var op = line.First switch
            {
                "A" => "R",
                "B" => "P",
                "C" => "S",
                _ => ""
            };

            var you = line.Second switch
            {
                "X" => loseShape(op),
                "Y" => op,
                "Z" => winShape(op),
                _ => ""
            };
            
            var score = scorePerShape(you) + (s: you, f: op) switch
            {
                ("R", "P") => 0,
                ("R", "S") => 6,
                ("P", "S") => 0,
                ("P", "R") => 6,
                ("S", "P") => 6,
                ("S", "R") => 0,
                _ => 3
            };

            int scorePerShape(string shape) => shape switch
            {
                "R" => 1,
                "P" => 2,
                "S" => 3,
                _ => 0
            };

            string winShape(string shape) => shape switch
            {
                "R" => "P",
                "P" => "S",
                "S" => "R",
                _ => ""
            };

            string loseShape(string shape) => shape switch
            {
                "P" => "R",
                "S" => "P",
                "R" => "S",
                _ => ""
            };

            total += score;
        }

        Assert.That(total, Is.EqualTo(10238));
    }
}