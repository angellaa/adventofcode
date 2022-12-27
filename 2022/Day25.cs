using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day25
{
    private List<long> numbers;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        numbers = File.ReadAllLines("Input.txt").Select(ToInteger).ToList();
    }

    private static long ToInteger(string number)
    {
        var chars = number.Reverse().ToArray();

        var result = 0L;

        for (var i = 0; i < number.Length; i++)
        {
            result += (long)Math.Pow(5, i) * chars[i] switch
            {
                '2' => 2, '1' => 1, '-' => -1, '=' => -2, _ => 0
            };
        }

        return result;
    }

    [Test]
    public void Part1()
    {
        Assert.That(ToSNAFU(numbers.Sum()), Is.EqualTo("2=001=-2=--0212-22-2"));
    }

    [Test]
    public void Part2()
    {
    }

    private static string ToSNAFU(long n)
    {
        var result = "";

        while (n > 0)
        {
            result += n % 5;
            n /= 5;
        }

        result += "0";

        var chars = result.ToArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '3') { chars[i] = '='; chars[i + 1]++; }
            if (chars[i] == '4') { chars[i] = '-'; chars[i + 1]++; }
            if (chars[i] == '5') { chars[i] = '0'; chars[i + 1]++; }
        }

        return new string(chars.Reverse().ToArray()).TrimStart('0');
    }
}