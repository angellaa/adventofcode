//using Newtonsoft.Json.Linq;
//using System.Diagnostics;
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

//var stopWatch = Stopwatch.StartNew();
//var openValves = new Stack<Valve>();
//int best = 0;
//var bestByTime = new int[27];

//var explorationSpace = ExplorationSpace("AA");
//var maxOpenableValves = explorationSpace.Count(x => valves.Find(y => y.Name == x).FlowRate > 0);
////var unopenedValves = valves.Where(x => x.FlowRate > 0).ToList();
//var rank = new Dictionary<string, double>();

//var myValve = valves.Find(x => x.Name == "AA");
//var elephantValve = valves.Find(x => x.Name == "AA");

//var myVisited = new HashSet<Valve>();

//var elephantVisited = new HashSet<Valve>();
//var openValvesFlow = 0;

//Valve lastVisited = myValve;
//Valve lastElephantVisited = elephantValve;

//var distance = new Dictionary<(string, string), int>();

//foreach (var p in explorationSpace)
//    foreach (var q in explorationSpace)
//    {
//        distance[(p, q)] = AStar(p, q);
//    }

//UpdateRanking();

//Console.WriteLine(MaxFlow(1, 0, myValve, elephantValve, 0, 0));
//Console.WriteLine("end in " + stopWatch.Elapsed);


//int MaxFlow(int time, int totalFlow, Valve myValve, Valve elephantValve, int myMovesWithoutOpens, int elephantMovesWithoutOpens)
//{
//    if (myVisited.Contains(myValve) || 
//        elephantVisited.Contains(elephantValve) || 
//        openValves.Count >= maxOpenableValves ||
//         myMovesWithoutOpens > 6 || elephantMovesWithoutOpens > 5 ||
//        (time > 15 && totalFlow < best * 0.5))//+ unopenedValves.Sum(x => x.FlowRate) * (26 - time) < best))
//    {
//        totalFlow += openValvesFlow * (26 - time);

//        if (totalFlow > best)
//        {
//            Console.WriteLine($"New best {totalFlow} \t t = {time}");
//            best = totalFlow;
//            bestByTime[time] = best;
//        }

//        return totalFlow;
//    }

//    myVisited.Add(myValve);
//    elephantVisited.Add(elephantValve);
//    lastVisited = myValve;
//    lastElephantVisited = elephantValve;

//    totalFlow += openValvesFlow;

//    if (time == 26)
//    {
//        if (totalFlow > best)
//        {
//            Console.WriteLine($"New best {totalFlow} \t t = {time}");
//            best = totalFlow;
//            bestByTime[time] = best;
//        }
//        return totalFlow;
//    }

//    var max = totalFlow;

//    if (!openValves.Contains(myValve) && myValve.FlowRate > 0 &&
//        !openValves.Contains(elephantValve) && elephantValve.FlowRate > 0 &&
//        myValve != elephantValve)
//    {
//        openValves.Push(myValve);
//        openValves.Push(elephantValve);
//        //unopenedValves.Remove(myValve);
//        //unopenedValves.Remove(elephantValve);

//        openValvesFlow += myValve.FlowRate + elephantValve.FlowRate;

//        myVisited.Clear();
//        elephantVisited.Clear();

//        max = MaxFlow(time + 1, totalFlow, myValve, elephantValve, 0, 0);

//        openValves.Pop();
//        openValves.Pop();
//        //unopenedValves.Add(myValve);
//        //unopenedValves.Add(elephantValve);

//        openValvesFlow -= myValve.FlowRate + elephantValve.FlowRate;
//    }

//    if (!openValves.Contains(myValve) && myValve.FlowRate > 0)
//    {
//        openValves.Push(myValve);
//        //unopenedValves.Remove(myValve);

//        openValvesFlow += myValve.FlowRate; 
//        myVisited.Clear();

//        foreach (var valve in elephantValve.Next(valves, rank))
//        {
//            max = Math.Max(max, MaxFlow(time + 1, totalFlow, myValve, valve, 0, elephantMovesWithoutOpens + 1));
//        }

//        openValves.Pop();
//        //unopenedValves.Add(myValve);

//        openValvesFlow -= myValve.FlowRate;
//    }

//    if (!openValves.Contains(elephantValve) && elephantValve.FlowRate > 0)
//    {
//        openValves.Push(elephantValve);
//        //unopenedValves.Remove(elephantValve);

//        openValvesFlow += elephantValve.FlowRate;

//        elephantVisited.Clear();

//        foreach (var valve in myValve.Next(valves, rank))
//        {
//            max = Math.Max(max, MaxFlow(time + 1, totalFlow, valve, elephantValve, myMovesWithoutOpens + 1, 0));
//        }

//        openValves.Pop();
//        //unopenedValves.Add(elephantValve);

//        openValvesFlow -= elephantValve.FlowRate;
//    }
    
//    foreach (var valve in myValve.Next(valves, rank))
//    {
//        foreach (var eValve in elephantValve.Next(valves, rank))
//        {
//            if (valve != lastVisited && eValve != lastElephantVisited)
//            {
//                max = Math.Max(max, MaxFlow(time + 1, totalFlow, valve, eValve, myMovesWithoutOpens + 1, elephantMovesWithoutOpens + 1));
//            }
//            else
//            {
//                //Console.Write(".");
//            }
//        }
//    }

//    if (max > best)
//    {
//        Console.WriteLine($"New best {totalFlow} \t t = {time}");
//        best = max;
//        bestByTime[time] = best;
//    }

//    myVisited.Remove(myValve);
//    elephantVisited.Remove(elephantValve);

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

////double Rank(string name) => valves.Find(x => x.Name == name).FlowRate;

//double Rank(string name)
//{
//    //explorationSpace.Except(openValves.Select(x => x.Name)).Sum(p => valves.Find(x => x.Name == p).FlowRate * (26 - distance[(name, p)]));

//    // best rank to the valve closest to an valve with flow
//    var min = explorationSpace.Except(openValves.Select(x => x.Name)).Except(new[] {name})
//        .Where(n => valves.Find(x => x.Name == n).FlowRate > 0)
//        .Select(p => distance[(name, p)])
//        .Min();

//    return min;
//}

//void UpdateRanking()
//{
//    foreach (var name in explorationSpace)
//    {
//        rank[name] = Rank(name);
//    }
//}

//int AStar(string start, string goal)
//{
//    var hashSet = new HashSet<string>();
//    var openSet = new PriorityQueue<string, int>();
//    openSet.Enqueue(start, 0);
//    hashSet.Add(start);

//    var gScore = new Dictionary<string, int>
//    {
//        [start] = 0
//    };

//    var fScore = new Dictionary<string, int>
//    {
//        [start] = h(start)
//    };

//    while (openSet.TryDequeue(out var current, out var priority))
//    {
//        if (current == goal)
//        {
//            return priority;
//        }

//        hashSet.Remove(current);

//        var neighbors = GetNeighbors(current);

//        foreach (var neighbor in neighbors)
//        {
//            var tentative_gScore = g(current) + 1;

//            if (tentative_gScore < g(neighbor))
//            {
//                gScore[neighbor] = tentative_gScore;
//                fScore[neighbor] = tentative_gScore + h(neighbor);

//                if (!hashSet.Contains(neighbor))
//                {
//                    openSet.Enqueue(neighbor, f(neighbor));
//                    hashSet.Add(neighbor);
//                }
//            }
//        }
//    }

//    return int.MaxValue; // no path found

//    IEnumerable<string> GetNeighbors(string i)
//    {
//        foreach (var v in valves.Find(x => x.Name == i).NextValves)
//        {
//            yield return v;
//        }
//    }

//    int g(string current) => gScore.ContainsKey(current) ? gScore[current] : int.MaxValue;
//    int f(string current) => fScore.ContainsKey(current) ? fScore[current] : int.MaxValue;

//    int h(string i)
//    {
//        return 1;
//    }
//}

//public class Valve
//{    
//    public readonly List<string> NextValves;
//    public string Name { get; }
//    public int FlowRate { get; }
    
//    private List<Valve> _nextValves;

//    public Valve(string Name, int FlowRate, List<string> NextValves)
//    {
//        this.NextValves = NextValves;
//        this.Name = Name;
//        this.FlowRate = FlowRate;
//    }
//    public List<Valve> Next(List<Valve> valves, Dictionary<string, double> rank)
//    {
//        if (_nextValves == null)
//        {
//            _nextValves = NextValves.Select(x => valves.Find(v => v.Name == x)).OrderBy(x => rank[x.Name]).ToList();
//        }
        
//        return _nextValves;
//    }
//}