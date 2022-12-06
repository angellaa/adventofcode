using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day6
{
    [TestCase(4, 1578)]
    [TestCase(14, 2178)]
    public void Test(int n, int expected)
    {
        var input = File.ReadAllText("Input.txt");

        for (var i = n; i < input.Length; i++)
        {
            var sequence = input[(i - n)..i];

            if (sequence.Distinct().SequenceEqual(sequence))
            {
                Assert.That(i, Is.EqualTo(expected));
                return;
            }
        }

        Assert.Fail();
    }
}