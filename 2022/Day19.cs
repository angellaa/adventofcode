using NUnit.Framework;

using static System.Text.RegularExpressions.Regex;

namespace AdventOfCode2022;

[TestFixture]
public class Day19
{
    private record Blueprint(
            int id,
            int oreRobotCostInOre, int clayRobotCostInOre, int obsidianRobotCostInOre, 
            int obsidianRobotCostInClay, int geodeRobotCostInOre, int geodeRobotCostInObsidian);

    private readonly List<Blueprint> blueprints = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        foreach (var line in File.ReadAllLines("Input.txt"))
        {
            var id = int.Parse(line.Split(":")[0].Split(" ")[1]);
            var parts = line.Split(":")[1].Split(".").Select(x => x.Trim()).ToList();

            var obsMatch = Match(parts[2], @"Each obsidian robot costs (\d+) ore and (\d+) clay");
            var geodeMatch = Match(parts[3], @"Each geode robot costs (\d+) ore and (\d+) obsidian");

            var blueprint = new Blueprint(
                id, 
                int.Parse(Match(parts[0], @"Each ore robot costs (\d+) ore").Groups[1].Value),
                int.Parse(Match(parts[1], @"Each clay robot costs (\d+) ore").Groups[1].Value),
                int.Parse(obsMatch.Groups[1].Value),
                int.Parse(obsMatch.Groups[2].Value),
                int.Parse(geodeMatch.Groups[1].Value),
                int.Parse(geodeMatch.Groups[2].Value)
            );

            blueprints.Add(blueprint);
        }
    }

    [Test]
    public void Part1()
    {
        Assert.That(blueprints.Sum(blueprint => OpenGeodes(blueprint) * blueprint.id), Is.EqualTo(1681));
    }

    [Test]
    public void Part2()
    {
        Assert.That(blueprints.Take(3).Aggregate(1, (current, blueprint) => current * OpenGeodes(blueprint, 32)), Is.EqualTo(5394));
    }

    private static int OpenGeodes(Blueprint blueprint, int targetMinutes = 24)
    {
        var root = (0, 0, 0, 0, 1, 0, 0, 0, 0);

        var explored = new HashSet<(int ore, int clay, int obsidian, int geode, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int minute)> { root };

        var stack = new Stack<(int ore, int clay, int obsidian, int geode, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int minute)>();
        stack.Push(root);

        var max = 0;

        while (stack.Count > 0)
        {
            var (ore, clay, obsidian, geode, oreRobots, clayRobots, obsidianRobots, geodeRobots, minute) = stack.Pop();

            if (minute == targetMinutes)
            {
                if (geode > max)
                {
                    max = geode;
                }
                continue;
            }

            if (geode + geodeRobots * (targetMinutes - minute) + (targetMinutes - minute) * (targetMinutes - minute + 1) / 2 < max)
            {
                continue;
            }

            var nextMoves = new List<(int ore, int clay, int obsidian, int geode, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int minute)>();

            if (obsidian >= blueprint.geodeRobotCostInObsidian && ore >= blueprint.geodeRobotCostInOre)
            {
                nextMoves.Add((ore - blueprint.geodeRobotCostInOre + oreRobots, clay + clayRobots, obsidian - blueprint.geodeRobotCostInObsidian + obsidianRobots, geode + geodeRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots + 1, minute + 1));
            }

            if (clay >= blueprint.obsidianRobotCostInClay && ore >= blueprint.obsidianRobotCostInOre)
            {
                nextMoves.Add((ore - blueprint.obsidianRobotCostInOre + oreRobots, clay - blueprint.obsidianRobotCostInClay + clayRobots, obsidian + obsidianRobots, geode + geodeRobots, oreRobots, clayRobots, obsidianRobots + 1, geodeRobots, minute + 1));
            }

            if (ore >= blueprint.clayRobotCostInOre)
            {
                nextMoves.Add((ore - blueprint.clayRobotCostInOre + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geode + geodeRobots, oreRobots, clayRobots + 1, obsidianRobots, geodeRobots, minute + 1));
            }

            if (ore >= blueprint.oreRobotCostInOre)
            {
                nextMoves.Add((ore - blueprint.oreRobotCostInOre + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geode + geodeRobots, oreRobots + 1, clayRobots, obsidianRobots, geodeRobots, minute + 1));
            }

            nextMoves.Add((ore + oreRobots, clay + clayRobots, obsidian + obsidianRobots, geode + geodeRobots, oreRobots, clayRobots, obsidianRobots, geodeRobots, minute + 1));

            foreach (var move in nextMoves)
            {
                if (!explored.Contains(move))
                {
                    explored.Add(move);
                    stack.Push(move);
                }
            }
        }

        return max;
    }
}