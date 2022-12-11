using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode2022;

var chunks = File.ReadAllLines("Input.txt").ChunkBy(x => x == "").ToList();

var worries = new List<List<BigInteger>>();
var ops = new List<string>();
var values = new List<string>();
var parsedValues = new List<BigInteger>();
var tests = new List<BigInteger>();
var trueMonkey = new List<int>();
var falseMonkey = new List<int>();

foreach (var chunk in chunks)
{
    var monkey = chunk.ToList();

    worries.Add(monkey[1].Split(':')[1].Split(',').Select(x => x.Trim()).Select(BigInteger.Parse).ToList());

    var m = Regex.Match(monkey[2], @"  Operation: new = old ([\*|\+]) (.+)");
    ops.Add(m.Groups[1].Value);

    var value = m.Groups[2].Value;
    values.Add(value);
    parsedValues.Add(value == "old" ? 0 : BigInteger.Parse(value));

    tests.Add(int.Parse(monkey[3].Split(' ').Last()));
    trueMonkey.Add(int.Parse(monkey[4].Split(' ').Last()));
    falseMonkey.Add(int.Parse(monkey[5].Split(' ').Last()));
}

var n = worries.Count;
var inspections = new BigInteger[n];
var product = tests.Aggregate(1, (current, test) => (int)(current * test)); // Part 2

//var rounds = 20;  // Part 1
var rounds = 10000; // part 2

for (var round = 0; round < 10000; round++)
{
    for (var i = 0; i < n; i++)
    {
        inspections[i] += worries[i].Count;

        foreach (var worryLevel in worries[i])
        {
            var worry = ops[i] switch
            {
                "*" => worryLevel * (values[i] == "old" ? worryLevel : parsedValues[i]),
                "+" => worryLevel + (values[i] == "old" ? worryLevel : parsedValues[i]),
            };

            //worry /= 3;     // Part 1
            worry %= product; // Part 2

            if (worry % tests[i] == 0)
            {
                worries[trueMonkey[i]].Add(worry);
            }
            else
            {
                worries[falseMonkey[i]].Add(worry);
            }
        }

        worries[i].Clear();
    }
}

Array.Sort(inspections);

Console.WriteLine(inspections[n-1] * inspections[n - 2]);
