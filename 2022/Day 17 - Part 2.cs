using System.Diagnostics;
using System.Linq;
using System.Numerics;

var map = new List<(int X, long Y)>();
var rock1 = new List<(int X, long Y)> { (0, 0), (1, 0), (2, 0), (3, 0) };
var rock2 = new List<(int X, long Y)> { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) };
var rock3 = new List<(int X, long Y)> { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };
var rock4 = new List<(int X, long Y)> { (0, 0), (0, 1), (0, 2), (0, 3) };
var rock5 = new List<(int X, long Y)> { (0, 0), (1, 0), (0, 1), (1, 1) };

var rocks = new List<List<(int X, long Y)>> { rock1, rock2, rock3, rock4, rock5 };

var jets = File.ReadAllText("Input.txt");
var topY = 0L;
var bottomY = 0L;
var rockIndex = -1;
var jetIndex = -1;
List<(int X, long Y)> rock;
NextRock();
long stoppedRocks = 0;
var highest = new long[7];
var diffs = new List<long>();
var lastY = 0L;
var cycleAdds = new List<long>();

// part 2 example
//var startFirstCycle = 77L;     
//var cycleLength = 35L;
//var highIncrease = 53L;

// part 2 real
var startFirstCycle = 171710L;
var cycleLength = 60050L;
var highIncrease = 90819L;

while (true)
{
    jetIndex = (jetIndex + 1) % jets.Length;
    var jet = jets[jetIndex];

    if (jet == '>') MoveRockRight();
    else if (jet == '<') MoveRockLeft();
    else throw new Exception("fuck");

    if (!MoveRockDown())
    {
        FixRock();
        NextRock();
        stoppedRocks++;

        //Console.Clear();
        //for (long y = topY; y >= 0; y--)
        //{
        //    var line = new string('.', 7).ToCharArray();
        //    foreach (var p in map.Where(p => p.Y == y))
        //        line[p.X] = '#';
        //    Console.WriteLine(new string(line));
        //}

        //Console.ReadKey();

        if (stoppedRocks == startFirstCycle + cycleLength) break;

        if (stoppedRocks >= startFirstCycle)
        {
            cycleAdds.Add(topY - lastY);
        }

        lastY = topY;
    }
}

// Use the cycle information to calculate high
BigInteger sum = cycleAdds.Sum();
BigInteger result = topY +
                    highIncrease * ((1000000000000 - stoppedRocks) / cycleLength) +
                    cycleAdds.Skip(1).Take((int)((1000000000000 - stoppedRocks) % cycleLength)).Sum();

Console.WriteLine(result);

void SetInitialPosition()
{
    for (var i = 0; i < rock.Count; i++)
    {
        rock[i] = (rock[i].X + 2, rock[i].Y + topY + 3);
    }
}

void NextRock()
{
    rockIndex = (rockIndex + 1) % rocks.Count;
    rock = rocks[rockIndex].ToList();
    SetInitialPosition();
}

void FixRock()
{
    map.AddRange(rock);

    foreach (var r in rock)
    {
        highest[r.X] = Math.Max(highest[r.X], r.Y);
    }

    topY = highest.Max() + 1;
    bottomY = highest.Min();

    map.RemoveAll(p => p.Y < bottomY);

    // Code to find the cycle
    //if (map.All(p => p.Y <= bottomY))
    //{
    //    Console.WriteLine("One layer after " + stoppedRocks + " rocks. High increase = " + (topY - lastY));
    //    lastY = topY;
    //}
}

bool MoveRockLeft() => MoveRock((-1, 0));
bool MoveRockRight() => MoveRock((1, 0));
bool MoveRockDown() => MoveRock((0, -1));

bool MoveRock((int X, int Y) dir)
{
    var newRock = rock.ToList();

    for (var i = 0; i < rock.Count; i++)
    {
        var p = (X: newRock[i].X + dir.X, Y: newRock[i].Y + dir.Y);

        if (p.X is < 0 or >= 7 || p.Y < 0 || map.Contains(p))
        {
            return false;
        }

        newRock[i] = p;
    }

    rock = newRock;
    return true;
}


