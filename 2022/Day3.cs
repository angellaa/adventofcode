﻿using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day3
{
    private List<string> rucksacks = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        rucksacks = File.ReadAllLines("input.txt").ToList();
    }

    [Test]
    public void Part1()
    {
        var total = rucksacks.Sum(r => Priority(r[..(r.Length / 2)].Intersect(r[(r.Length / 2)..]).First()));

        Assert.That(total, Is.EqualTo(8109));
    }

    [Test]
    public void Part2()
    {
        var total = rucksacks.Chunk(3)
                             .Sum(groups => Priority(groups.Aggregate((IEnumerable<char>) groups[0], (x, r) => x.Intersect(r))
                             .First()));

        Assert.That(total, Is.EqualTo(2738));
    }

    private static int Priority(char item) => char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;
}