using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day8
{
    private List<(List<string> SignalPattern, List<string> Display)> input;

    [SetUp]
    public void SetUp()
    {
        input = new();

        foreach (var line in File.ReadAllText("Day8.txt").Split("\n"))
        {
            var digits = line.Split("|")[0].Split().Where(x => x.Trim() != "").ToList();
            var display = line.Split("|")[1].Split().Where(x => x.Trim() != "").ToList();

            input.Add((digits, display));
        }
    }

    [Test]
    public void Part1()
    {
        var count = input.SelectMany(x => x.Display).Count(x => x.Count() is 2 or 3 or 4 or 7);

        Assert.That(count, Is.EqualTo(534));
    }

    [Test]
    public void Part2()
    {
        var result = 0;

        foreach (var (signalPatterns, display) in input)
        {
            var one = signalPatterns.First(x => x.Length == 2).Sort();
            var four = signalPatterns.First(x => x.Length == 4).Sort();
            var seven = signalPatterns.First(x => x.Length == 3).Sort();
            var eight = signalPatterns.First(x => x.Length == 7).Sort();

            var fiveSegmentsSignalPatterns = signalPatterns.Where(x => x.Length == 5).ToArray();
            var sixSegmentsSignalPatternsAndOne = signalPatterns.Where(x => x.Length == 6).Union(new[] { one }).ToArray();

            var three = (GetSharedSegments(fiveSegmentsSignalPatterns) + one).Sort();
            var nine = new string(sixSegmentsSignalPatternsAndOne.First(x => GetNotSharedSegment(x, three).Count() == 1)).Sort();

            var a = seven.Except(one).First();
            var b = GetNotSharedSegment(three, nine).First();
            var f = GetSharedSegments(sixSegmentsSignalPatternsAndOne).First();
            var c = one.First(x => x != f);
            var d = GetNotSharedSegment(four, $"{b}{c}{f}").First();
            var g = GetNotSharedSegment(nine, $"{a}{b}{c}{d}{f}").First();
            var e = GetNotSharedSegment("abcdefg", $"{a}{b}{c}{d}{f}{g}").First();

            var zero = $"{a}{b}{c}{e}{f}{g}".Sort();
            var two = $"{a}{c}{d}{e}{g}".Sort();
            var five = $"{a}{b}{d}{f}{g}".Sort();
            var six = $"{a}{b}{d}{e}{f}{g}".Sort();

            var map = new Dictionary<string, char>
            {
                [zero] = '0',
                [one] = '1',
                [two] = '2',
                [three] = '3',
                [four] = '4',
                [five] = '5',
                [six] = '6',
                [seven] = '7',
                [eight] = '8',
                [nine] = '9',
            };

            var outputValue = int.Parse(new(display.Select(x => x.Sort()).Select(x => map[x]).ToArray()));

            result += outputValue;
        }

        Assert.That(result, Is.EqualTo(1070188));

        string GetSharedSegments(params string[] digits) => new("abcdefg".Where(x => digits.All(c => c.Contains(x))).ToArray());

        string GetNotSharedSegment(params string[] digits)
        {
            var sharedSegments = GetSharedSegments(digits);

            return new("abcdefg".Where(x => !sharedSegments.Contains(x) && digits.Any(d => d.Contains(x))).ToArray());
        }
    }
}

public static class StringExtensions
{
    public static string Sort(this string s) => new(s.OrderBy(x => x).ToArray());
}
