using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day2
{
    private List<(string FirstHand, string SecondHand)> rounds = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        rounds = File.ReadAllLines("Day2.txt").Select(x => (x.Split()[0], x.Split()[1])).ToList();
    }

    [Test]
    public void Part1()
    {
        var scores = from round in rounds
                     let opHand = Shape(round.FirstHand)
                     let myHand = Shape(round.SecondHand)
                     select HandScore(myHand) + RoundScore(myHand, opHand);

        Assert.That(scores.Sum(), Is.EqualTo(8890));
    }

    [Test]
    public void Part2()
    {
        var scores = from round in rounds
                     let opHand = Shape(round.FirstHand)
                     let myHand = round.SecondHand switch { "X" => LoosingShape(opHand), "Y" => opHand, "Z" => WinningShape(opHand) }
                     select HandScore(myHand) + RoundScore(myHand, opHand);

        Assert.That(scores.Sum(), Is.EqualTo(10238));
    }

    private static string Shape(string shape) => shape switch { "A" or "X" => "R", "B" or "Y" => "P", "C" or "Z" => "S" };
    private static string WinningShape(string shape) => shape switch { "R" => "P", "P" => "S", "S" => "R" };
    private static string LoosingShape(string shape) => shape switch { "P" => "R", "S" => "P", "R" => "S" };
    private static int HandScore(string shape) => shape switch { "R" => 1, "P" => 2, "S" => 3 };
    private static int RoundScore(string myHand, string opHand) => (myHand, opHand) switch
    {
        ("R", "S") or ("P", "R") or ("S", "P") => 6,
        _ when (myHand == opHand) => 3,
        _ => 0
    };
}