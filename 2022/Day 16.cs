using NUnit.Framework;
using static System.Text.RegularExpressions.Regex;

[TestFixture]
public class Day16
{
    private List<Valve> valves;

    [Flags]
    enum Opens {};

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        valves = new List<Valve>();
        var id = 0;

        foreach (var line in File.ReadAllLines("Input.txt"))
        {
            var m = Match(line, @"Valve (\w+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)");

            var valve = m.Groups[1].Value;
            var flowRate = int.Parse(m.Groups[2].Value);
            var nextVales = m.Groups[3].Value.Split(", ").ToList();

            valves.Add(new Valve(id++, valve, flowRate, nextVales));
        }

        foreach (var valve in valves)
        {
            valve.NextValves = valve.NextValveNames.Select(x => valves.Find(v => v.Name == x)).OrderByDescending(x => x.FlowRate).ToList();
        }
    }

    [Test]
    public void Part1()
    {
        Assert.That(MaxFlow(), Is.EqualTo(1641));
    }

    [Test]
    public void Part2() {}

    private int MaxFlow()
    {
        var visited = new HashSet<(int time, int totalFlow, Valve valve, string opens)>();
        var startValve = valves.Find(x => x.Name == "AA");

        var queue = new Queue<(int time, int totalFlow, Valve valve, string opens)>();

        var start = (1, 0, startValve, new string('-', valves.Count));

        visited.Add(start);
        queue.Enqueue(start);

        var best = 0;

        while (queue.Count > 0)
        {
            var (time, totalFlow, valve, opens) = queue.Dequeue();

            var openValves = valves.Where(x => opens[x.Id] == 'O').ToList();

            totalFlow += openValves.Sum(x => x.FlowRate);

            if (time == 30)
            {
                if (totalFlow > best) best = totalFlow;
                continue;
            }

            if (opens[valve.Id] == '-' && valve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[valve.Id] = 'O';

                queue.Enqueue((time + 1, totalFlow, valve, new string(newOpens)));
            }

            foreach (var v in valve.NextValves)
            {
                var next = (time + 1, totalFlow, v, opens);

                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
        }

        return best;
    }

    private record Valve(int Id, string Name, int FlowRate, List<string> NextValveNames)
    {
        public List<Valve> NextValves;
    }
}
