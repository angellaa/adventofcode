//using System.Text.RegularExpressions;

//var valves = new List<Valve>();

//foreach (var line in File.ReadAllLines("Input.txt"))
//{
//    var m = Regex.Match(line, @"Valve (\w+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)");

//    var valve = m.Groups[1].Value;
//    var flowRate = int.Parse(m.Groups[2].Value);
//    var nextVales = m.Groups[3].Value.Split(", ").ToList();

//    valves.Add(new Valve(valve, flowRate, nextVales));
//}

//var openValves = new Stack<Valve>();
//var visited = new HashSet<Valve>();
//int best = 0;

//var maxOpenableValves = ExplorationSpace("AA").Count(x => valves.Find(y => y.Name == x).FlowRate > 0);

//Console.WriteLine(MaxFlow(1, 0, valves.Find(x => x.Name == "AA"), 0));
//Console.WriteLine("end");

//int MaxFlow(int time, int totalFlow, Valve startValve, int movesWithoutOpens)
//{
//    if (openValves.Count >= maxOpenableValves || movesWithoutOpens > 3)
//    {
//        totalFlow += openValves.Sum(x => x.FlowRate) * (30 - time);

//        if (totalFlow > best)
//        {
//            Console.WriteLine("New best " + totalFlow);
//            best = totalFlow;
//        }

//        return totalFlow;
//    }

//    totalFlow += openValves.Sum(x => x.FlowRate);

//    if (time == 30)
//    {
//        if (totalFlow > best)
//        {
//            Console.WriteLine("New best " + totalFlow);
//            best = totalFlow;
//        }
//        return totalFlow;
//    }

//    visited.Add(startValve);

//    var max = totalFlow;

//    if (!openValves.Contains(startValve) && startValve.FlowRate > 0)
//    {
//        openValves.Push(startValve);
//        max = MaxFlow(time + 1, totalFlow, startValve, 0);
//        openValves.Pop();
//    }

//    foreach (var valve in startValve.Next(valves))
//    {
//        max = Math.Max(max, MaxFlow(time + 1, totalFlow, valve, movesWithoutOpens + 1));
//    }

//    if (max > best)
//    {
//        Console.WriteLine("New best " + max);
//        best = max;
//    }

//    visited.Remove(startValve);

//    return max;
//}

//HashSet<string> ExplorationSpace(string valve)
//{
//    var toExplore = new Stack<string>();
//    var result = new HashSet<string>();

//    toExplore.Push(valve);

//    while (toExplore.Count > 0)
//    {
//        var next = toExplore.Pop();

//        if (result.Contains(next)) continue;

//        result.Add(next);

//        foreach (var nextValve in valves.Find(x => x.Name == next).NextValves)
//        {
//            toExplore.Push(nextValve);
//        }
//    }

//    return result;
//}


//record Valve(string Name, int FlowRate, List<string> NextValves)
//{
//    private readonly List<Valve> nextValves = null;

//    public List<Valve> Next(List<Valve> valves)
//    {
//        return nextValves ?? NextValves.Select(x => valves.Find(v => v.Name == x)).OrderByDescending(x => x.FlowRate).ToList();
//    }
//}
