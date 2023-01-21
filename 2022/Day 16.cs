using NUnit.Framework;
using static System.Text.RegularExpressions.Regex;

[TestFixture]
public class Day16
{
    private List<Valve> valves;
    private Stack<Valve> openValves;
    private HashSet<Valve> visited;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        valves = new List<Valve>();

        foreach (var line in File.ReadAllLines("Input.txt"))
        {
            var m = Match(line, @"Valve (\w+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)");

            var valve = m.Groups[1].Value;
            var flowRate = int.Parse(m.Groups[2].Value);
            var nextVales = m.Groups[3].Value.Split(", ").ToList();

            valves.Add(new Valve(valve, flowRate, nextVales));
        }

        foreach (var valve in valves)
        {
            valve.NextValves = valve.NextValveNames.Select(x => valves.Find(v => v.Name == x)).OrderByDescending(x => x.FlowRate).ToList();
        }
    }

    [Test]
    public void Part1()
    {
        openValves = new Stack<Valve>();
        visited = new HashSet<Valve>();

        var maxFlow = MaxFlow(1, 0, valves.Find(x => x.Name == "AA"), 0);

        Assert.That(maxFlow, Is.EqualTo(1641));
    }

    [Test]
    public void Part2() {}

    private int MaxFlow(int time, int totalFlow, Valve startValve, int movesWithoutOpens)
    {
        if (movesWithoutOpens > 3)
        {
            return totalFlow + openValves.Sum(x => x.FlowRate) * (30 - time);
        }

        totalFlow += openValves.Sum(x => x.FlowRate);

        if (time == 30)
        {
            return totalFlow;
        }

        visited.Add(startValve);

        var max = totalFlow;

        if (!openValves.Contains(startValve) && startValve.FlowRate > 0)
        {
            openValves.Push(startValve);

            max = MaxFlow(time + 1, totalFlow, startValve, 0);

            openValves.Pop();
        }

        foreach (var valve in startValve.NextValves)
        {
            max = Math.Max(max, MaxFlow(time + 1, totalFlow, valve, movesWithoutOpens + 1));
        }

        visited.Remove(startValve);

        return max;
    }

    private record Valve(string Name, int FlowRate, List<string> NextValveNames)
    {
        public List<Valve> NextValves;
    }
}
