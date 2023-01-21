using NUnit.Framework;
using static System.Text.RegularExpressions.Regex;

[TestFixture]
public class Day16
{
    private List<Valve> valves;

    [Flags]
    enum Opens { };

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

        foreach (var p in valves)
        {
            foreach (var q in valves.Where(x => x.FlowRate > 0).Except(new [] {p}))
            {
                p.NextFlowValves.Add((q, AStar(p.Name, q.Name) - 1));
            }
        }
    }

    [Test]
    public void Part1()
    {
        var paths = GetPaths();
        var maxFlow = 0;

        foreach (var path in paths)
        {
            var totalFlow = 0;
            var flow = 0;
            var time = 0;

            foreach (var (v, d) in path)
            {
                totalFlow += flow * (d - time);
                flow += v.FlowRate;
                time = d;
            }

            totalFlow += flow * (30 - path.Last().Item2);

            maxFlow = Math.Max(maxFlow, totalFlow);
        }

        Assert.That(maxFlow, Is.EqualTo(1641));
    }

    [Test]
    public void Part2()
    {
    }
    
    private List<List<(Valve, int)>> GetPaths()
    {
        var visited = new HashSet<(int time, int totalFlow, Valve valve, string opens, int distance, List<(Valve, int)> path)>();
        var startValve = valves.Find(x => x.Name == "AA");

        var queue = new Queue<(int time, int totalFlow, Valve valve, string opens, int distance, List<(Valve, int)> path)>();

        var start = (1, 0, startValve, new string('-', valves.Count), 1, new List<(Valve, int)>());

        visited.Add(start);
        queue.Enqueue(start);

        var best = 0;
        var result = new List<List<(Valve, int)>>();

        while (queue.Count > 0)
        {
            var (time, totalFlow, valve, opens, distance, path) = queue.Dequeue();

            var openValves = valves.Where(x => opens[x.Id] == 'O').ToList();

            totalFlow += openValves.Sum(x => x.FlowRate) * distance;

            if (time == 30)
            {
                if (totalFlow > best) best = totalFlow;
                continue;
            }

            if (opens[valve.Id] == '-' && valve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[valve.Id] = 'O';

                queue.Enqueue((time + 1, totalFlow, valve, new string(newOpens), 1, path));
                continue;
            }

            foreach (var (v, d) in valve.NextFlowValves.Where(x => opens[x.Valve.Id] == '-'))
            {
                var newPath = path.ToList();
                newPath.Add((v, time + d));

                var next = (time + d, totalFlow, v, opens, d, newPath);

                if (!visited.Contains(next))
                {
                    if (time + d <= 30)
                    {
                        if (newPath.Count > 7)
                        {
                            result.Add(newPath);
                        }

                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }
        }

        return result;
    }


    private record Valve(int Id, string Name, int FlowRate, List<string> NextValveNames)
    {
        public List<Valve> NextValves = new();
        public List<(Valve Valve, int Distance)> NextFlowValves = new();
    }

    int AStar(string start, string goal)
    {
        var hashSet = new HashSet<string>();
        var openSet = new PriorityQueue<string, int>();
        openSet.Enqueue(start, 0);
        hashSet.Add(start);

        var gScore = new Dictionary<string, int>
        {
            [start] = 0
        };

        var fScore = new Dictionary<string, int>
        {
            [start] = h(start)
        };

        while (openSet.TryDequeue(out var current, out var priority))
        {
            if (current == goal)
            {
                return priority;
            }

            hashSet.Remove(current);

            var neighbors = GetNeighbors(current);

            foreach (var neighbor in neighbors)
            {
                var tentative_gScore = g(current) + 1;

                if (tentative_gScore < g(neighbor))
                {
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = tentative_gScore + h(neighbor);

                    if (!hashSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, f(neighbor));
                        hashSet.Add(neighbor);
                    }
                }
            }
        }

        return int.MaxValue; // no path found

        IEnumerable<string> GetNeighbors(string i)
        {
            foreach (var v in valves.Find(x => x.Name == i).NextValveNames)
            {
                yield return v;
            }
        }

        int g(string current) => gScore.ContainsKey(current) ? gScore[current] : int.MaxValue;
        int f(string current) => fScore.ContainsKey(current) ? fScore[current] : int.MaxValue;

        int h(string i)
        {
            return 1;
        }
    }
}
