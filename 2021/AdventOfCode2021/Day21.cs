using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day21
{
    List<string> input;
    private int pos1;
    private int pos2;
    private int score1;
    private int score2;
    private int die = 0;
    private int rolls = 0;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day21.txt").ToList();
        pos1 = int.Parse(input[0].Split(":")[1]) - 1;
        pos2 = int.Parse(input[1].Split(":")[1]) - 1;
    }

    [Test]
    public void Part1()
    {
        while (score2 <= 1000)
        {
            pos1 = (pos1 + Roll()) % 10;
            score1 += (pos1 + 1);

            if (score1 >= 1000) break;

            pos2 = (pos2 + Roll()) % 10;
            score2 += (pos2 + 1);
        }

        Assert.That(rolls * Math.Min(score1, score2), Is.EqualTo(900099));
    }


    [Test]
    public void Part2()
    {
        var universes = new int[21, 21, 10, 10]; // score1, score2, position1, position2 - value is how many win universes

        Assert.That(rolls * Math.Min(score1, score2), Is.EqualTo(900099));
    }

    public int Roll()
    {
        return NextDice() + NextDice() + NextDice();

        int NextDice()
        {
            rolls++;
            die++;
            if (die > 100) die = 1;
            return die;
        }
    }
}