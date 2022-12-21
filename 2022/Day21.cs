using Microsoft.CodeAnalysis.CSharp.Scripting;
using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day21
{
    private readonly Dictionary<string, string> monkeys = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        foreach (var line in File.ReadAllLines("Input.txt"))
        {
            monkeys.Add(line.Split(":")[0], line.Split(":")[1].Trim());
        }
    }

    [Test]
    public void Part1()
    {
        Assert.That(Eval(ExpLong("root")), Is.EqualTo(286698846151845));
    }

    [Test]
    public void Part2()
    {
        monkeys["humn"] = "X";

        var firstExp = Exp(monkeys["root"].Split("+")[0].Trim());
        var secondExp = Eval(ExpLong(monkeys["root"].Split("+")[1].Trim()));

        var octaveEquation = $"solve({firstExp} == {secondExp})".Replace(" ", "");

        Console.WriteLine(octaveEquation); // Use https://octave-online.net/ to solve this expression :) 
    }

    public string ExpLong(string name) => Exp(name, "L");

    public string Exp(string name, string suffix = "")
    {
        var expression = monkeys[name];

        while (expression.Any(char.IsLower))
        {
            foreach (var monkey in monkeys)
            {
                expression = expression.Replace(monkey.Key, long.TryParse(monkey.Value, out var v) ? $"{v}{suffix}" : $"({monkey.Value})");
            }
        }

        return expression;
    }

    public long Eval(string exp) => CSharpScript.EvaluateAsync<long>(exp).Result;
}