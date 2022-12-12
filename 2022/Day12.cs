
var lines = File.ReadAllLines("Input.txt").Select(x => x.ToCharArray()).ToList();
var rows = lines.Count;
var cols = lines[0].Length;

var sr = lines.FindIndex(x => x.Contains('S'));
var sc = lines[sr].ToList().FindIndex(x => x == 'S');

var er = lines.FindIndex(x => x.Contains('E'));
var ec = lines[er].ToList().FindIndex(x => x == 'E');

lines[sr][sc] = 'a';
lines[er][ec] = 'z';

Console.WriteLine(AStar(Index(sr, sc), Index(er, ec))); // part 1

var min = int.MaxValue;

for (var r = 0; r < rows; r++)
    for (var c = 0; c < cols; c++)
        if (lines[r][c] == 'a')
        {
            var result = AStar(Index(r, c), Index(er, ec));
 
            if (result < min)
            {
                min = result;
            }
        }

Console.WriteLine(min);

int AStar(int start, int goal)
{
    var hashSet = new HashSet<int>();
    var openSet = new PriorityQueue<int, int>();
    openSet.Enqueue(start, 0);
    hashSet.Add(start);

    var gScore = new Dictionary<int, int>
    {
        [start] = 0
    };

    var fScore = new Dictionary<int, int>
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
            var tentative_gScore = g(current) + d(current, neighbor);

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

    IEnumerable<int> GetNeighbors(int i)
    {
        var r = i / cols;
        var c = i % cols;

        var current = lines[r][c];

        if (InBound(r - 1, c) && lines[r - 1][c    ] - current <= 1) yield return Index(r - 1, c);
        if (InBound(r + 1, c) && lines[r + 1][c    ] - current <= 1) yield return Index(r + 1, c);
        if (InBound(r, c - 1) && lines[    r][c - 1] - current <= 1) yield return Index(r, c - 1);
        if (InBound(r, c + 1) && lines[    r][c + 1] - current <= 1) yield return Index(r, c + 1);
    }

    int g(int current) => gScore.ContainsKey(current) ? gScore[current] : int.MaxValue;
    int f(int current) => fScore.ContainsKey(current) ? fScore[current] : int.MaxValue;

    int h(int i)
    {
        var r = i / cols;
        var c = i % cols;

        return Math.Abs(er - r) + Math.Abs(ec - c);
    }

    int d(int current, int neighbor) => Cost(neighbor);

    bool InBound(int r, int c) => r >= 0 && r < rows && c >= 0 && c < cols;
}

int Index(int r, int c) => r * cols + c;
int Cost(int i) => 1;



