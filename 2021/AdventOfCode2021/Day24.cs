using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day24
{
    List<string> instructions;

    [SetUp]
    public void SetUp()
    {
        instructions = File.ReadAllLines("Day24.txt").ToList();
    }

    [Test]
    public void Part1()
    {
        var variables = new Dictionary<string, long>();
        var ops = new List<(string op, string a, string b)>();

        foreach (string instruction in instructions)
        {
            var parts = instruction.Split();
            var op = parts[0];

            var a = instruction.Split()[1];
            var b = parts.Length > 2 ? instruction.Split()[2] : null;

            ops.Add((op, a, b));
        }

        var varNames = ops.Select(x => x.a).Union(ops.Select(x => x.b).Where(x => x!= null && char.IsLetter(x[0]))).ToList();

        long n = 37500000000000;

        for (; n >= 11111111111111; n--)
        {
            var s = n.ToString();

            if (s.Contains('0')) continue;

            foreach (var varName in varNames)
            {
                variables[varName] = 0;
            }

            int inputIndex = 0;

            foreach (var (op, a, b) in ops)
            {
                _ = op switch
                {
                    "inp" => variables[a] = s[inputIndex++] - '0',
                    "add" => variables[a] += long.TryParse(b, out var v) ? v : variables[b],
                    "mul" => variables[a] *= long.TryParse(b, out var v) ? v : variables[b],
                    "div" => variables[a] /= long.TryParse(b, out var v) ? v : variables[b],
                    "mod" => variables[a] %= long.TryParse(b, out var v) ? v : variables[b],
                    "eql" => variables[a] = variables[a] == (long.TryParse(b, out var v) ? v : variables[b]) ? 1 : 0,
                    _ => throw new NotImplementedException()
                };
            }

            long x = variables["x"];
            long y = variables["y"];
            long w = variables["w"];
            long z = variables["z"];
            var valid = z == 0;

            //File.AppendAllText("c:\\temp\\out.txt", $"{n}:\tx={x}\ty={y}\tw={s}\tz={z}\n");

            if (valid) break;
        }

        Assert.That(n, Is.EqualTo(0));
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }
}