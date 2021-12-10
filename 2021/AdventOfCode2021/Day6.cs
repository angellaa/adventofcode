using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day6
{
    List<int> input;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllText("Day6.txt").Split(",").Select(x => int.Parse(x)).ToList();
    }

    [Test]
    public void Part1()
    {
        for (var i = 0; i < 80; i++)
        {
            int add = 0;

            for (var j = 0; j < input.Count; j++)
            {
                if (input[j] > 0) input[j]--;
                else
                {
                    input[j] = 6;
                    add++;
                }
            }

            input.AddRange(Enumerable.Range(0, add).Select(x => 8));
        }

        Assert.That(input.Count, Is.EqualTo(366057));
    }

    [Test]
    public void Part2()
    {
        var age = new long[10];

        foreach (var i in input)
        {
            age[i]++;
        }

        //Print(0, age);

        var index = 0;

        for (var i = 0; i < 256; i++)
        {
            age[(index + 9) % 10] += age[index];
            age[(index + 7) % 10] += age[index];
            age[index] = 0;

            index = (index + 1) % 10;

            //Print(index, age);
        }

        Assert.That(age.Sum(), Is.EqualTo(1653559299811));
    }

    private void Print(int index, long[] age)
    {
        for (var i = 0; i < age.Length; i++)
        {
            var currentAge = (int)age[(index + i) % 9];
            Console.Write(string.Join(" ", Enumerable.Repeat(i, currentAge)));
            if (currentAge > 0) Console.Write(" ");
        }
        Console.WriteLine();
    }
}