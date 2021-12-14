using NUnit.Framework;
using System.Text;

namespace AdventOfCode2021;

[TestFixture]
public class Day14
{
    string input;
    Dictionary<string, char> pairs;

    [SetUp]
    public void SetUp()
    {
        var lines= File.ReadAllLines("Day14.txt").ToList();
        input = lines[0];
        pairs = lines.Skip(2).Select(x => x.Split(" -> ")).ToDictionary(x => x[0], y => y[1][0]);
    }

    [Test]
    public void Part1()
    {
        for (var j = 0; j < 10; j++)        
        {
            var sb = new StringBuilder();

            for (int i = 0; i < input.Length - 1; i++)
            {
                var s = new string(input[i..(i + 2)].ToArray());
                var ns = pairs[s];

                sb.Append($"{s[0]}{ns}");
            }

            sb.Append(input[^1]);

            input = sb.ToString();
        }

        var count = new Dictionary<char, int>();
        foreach (var x in input)
        {
            if (count.ContainsKey(x)) count[x]++;
            else count[x] = 1;
        }

        var result = count.Max(x => x.Value) - count.Min(x => x.Value);

        Assert.That(result, Is.EqualTo(2967));
    }

    [Test]
    public void Part2()
    {
        var newPairs = new Dictionary<string, long>();
        var count = new Dictionary<char, long>();

        foreach (var c in input)
        {
            if (count.ContainsKey(c)) count[c] += 1;
            else count[c] = 1;
        }

        for (int i = 0; i < input.Length - 1; i++)
        {
            var s = "" + input[i] + input[i + 1];
            Add(s);
        }

        for (var j = 0; j < 40; j++)
        {
            foreach (var (pair, v) in newPairs.ToDictionary(x => x.Key, x => x.Value))
            {
                newPairs[pair] -= v;

                if (newPairs[pair] <= 0)
                {
                    newPairs.Remove(pair);
                }

                var c = pairs[pair];

                var first = $"{pair[0]}{c}";
                var second = $"{c}{pair[1]}";

                Add(first, v);
                Add(second, v);

                if (count.ContainsKey(c)) count[c] += v;
                else count[c] = v;
            }
        }

        var mostCommon = count.Max(x => x.Value);
        var leastCommon = count.Min(x => x.Value);
        var result = mostCommon - leastCommon;

        Assert.That(result, Is.EqualTo(3692219987038));

        void Add(string s, long v = 1)
        {
            if (newPairs.ContainsKey(s)) newPairs[s]+=v;
            else newPairs[s] = v;
        }
    }
}