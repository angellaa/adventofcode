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

    //private static List<(Valve, int)> FindNextFlowValves(Valve startValve)
    //{
    //    var visited = new HashSet<Valve>();
    //    var queue = new Queue<(Valve, int)>();

    //    visited.Add(startValve);
    //    queue.Enqueue((startValve, 0));

    //    var result = new List<(Valve Valve, int Distance)>();

    //    while (queue.Count > 0)
    //    {
    //        var (valve, distance) = queue.Dequeue();

    //        foreach (var v in valve.NextValves)
    //        {
    //            if (v.FlowRate > 0)
    //            {
    //                result.Add((v, distance + 1));
    //            }
    //            else
    //            {
    //                var next = (v, distance + 1);

    //                if (!visited.Contains(v))
    //                {
    //                    visited.Add(v);
    //                    queue.Enqueue(next);
    //                }
    //            }
    //        }
    //    }

    //    return result;
    //}

    [Test]
    public void Part1()
    {
        Assert.That(MaxFlow(), Is.EqualTo(1641));
    }

    [Test]
    public void Part2()
    {
        Assert.That(MaxFlowWithElephant(), Is.EqualTo(1707));
    }

    private int MaxFlowWithElephant()
    {
        var visited = new HashSet<(int time, int totalFlow, Valve valve, Valve elephantValve, string opens, int noOpens, int noElephantOpens)>();
        var startValve = valves.Find(x => x.Name == "AA");

        var queue = new Queue<(int time, int totalFlow, Valve valve, Valve elephantValve, string opens, int noOpens, int noElephantOpens)>();

        var start = (1, 0, startValve, startValve, new string('-', valves.Count), 0, 0);

        visited.Add(start);
        queue.Enqueue(start);

        var best = 0;

        var allOpened = "";
        foreach (var v in valves)
        {
            allOpened += v.FlowRate > 0 ? 'O' : '-';
        }

        while (queue.Count > 0)
        {
            var (time, totalFlow, valve, elephantValve, opens, noOpens, noElephantOpens) = queue.Dequeue();

            var openValves = valves.Where(x => opens[x.Id] == 'O').ToList();

            if (opens == allOpened)
            {
                totalFlow += openValves.Sum(x => x.FlowRate) * (26 - time);
                if (totalFlow > best) best = totalFlow;
                continue;
            }

            totalFlow += openValves.Sum(x => x.FlowRate);

            if (time == 26)
            {
                if (totalFlow > best) best = totalFlow;
                continue;
            }

            // Human and elephant are in different unopened valves with flow => open both
            if (valve != elephantValve && opens[valve.Id] == '-' && opens[elephantValve.Id] == '-' && valve.FlowRate > 0 && elephantValve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[valve.Id] = 'O';
                newOpens[elephantValve.Id] = 'O';

                queue.Enqueue((time + 1, totalFlow, valve, elephantValve, new string(newOpens), 0, 0));
            }

            if (opens[valve.Id] == '-' && valve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[valve.Id] = 'O';

                foreach (var v in elephantValve.NextValves)
                {
                    var next = (time + 1, totalFlow, valve, v, new string(newOpens), 0, noElephantOpens + 1);

                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }

            if (opens[elephantValve.Id] == '-' && elephantValve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[elephantValve.Id] = 'O';

                foreach (var v in valve.NextValves)
                {
                    var next = (time + 1, totalFlow, v, elephantValve, new string(newOpens), noOpens + 1, 0);

                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }

            foreach (var hv in valve.NextValves)
            foreach (var ev in elephantValve.NextValves)
            {
                if (hv != ev)
                {
                    var next = (time + 1, totalFlow, hv, ev, opens, noOpens + 1, noElephantOpens + 1);

                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }
        }

        return best;
    }

    private int MaxFlow()
    {
        var visited = new HashSet<(int time, int totalFlow, Valve valve, string opens, int distance)>();
        var startValve = valves.Find(x => x.Name == "AA");

        var queue = new Queue<(int time, int totalFlow, Valve valve, string opens, int distance)>();

        var start = (1, 0, startValve, new string('-', valves.Count), 1);

        visited.Add(start);
        queue.Enqueue(start);

        var best = 0;

        while (queue.Count > 0)
        {
            var (time, totalFlow, valve, opens, distance) = queue.Dequeue();

            var openValves = valves.Where(x => opens[x.Id] == 'O').ToList();

            totalFlow += openValves.Sum(x => x.FlowRate) * distance;

            if (time == 30)
            {
                if (totalFlow > best) best = totalFlow;
                continue;
            }

            if (time < 30)
            {
                var result = totalFlow + openValves.Sum(x => x.FlowRate) * (30 - time);
                if (result > best) best = result;
            }

            if (opens[valve.Id] == '-' && valve.FlowRate > 0)
            {
                var newOpens = opens.ToCharArray();
                newOpens[valve.Id] = 'O';

                queue.Enqueue((time + 1, totalFlow, valve, new string(newOpens), 1));
                continue;
            }
            
            foreach (var (v, d) in valve.NextFlowValves.Where(x => opens[x.Valve.Id] == '-'))
            {
                var next = (time + d, totalFlow, v, opens, d);

                if (!visited.Contains(next) && time + d <= 30)
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
