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
var rockIndex = -1;
var jetIndex = -1;
List<(int X, long Y)> rock;
NextRock();
int stoppedRocks = 0;
var highest = new long[7];
var patterns = new List<(HashSet<(int X, long Y)> Set, int RockIndex, char Jet)>();
var patternSize = 20;
int cycleSize;
var diffs = new List<long>();
var lastY = 0L;

char jet;

while (true)
{
    jetIndex = (jetIndex + 1) % jets.Length;
    jet = jets[jetIndex];

    if (jet == '>') MoveRockRight();
    else if (jet == '<') MoveRockLeft();
    else throw new Exception("ops");
    
    if (!MoveRockDown())
    {
        FixRock();
        NextRock();

        diffs.Add(topY - lastY);
        lastY = topY;

        if (topY > patternSize)
        {
            for (var i = 1; i < diffs.Count; i++)
            {
                if (stoppedRocks >= 0 && stoppedRocks - i >= 0 &&
                    patterns[stoppedRocks].Set.SetEquals(patterns[stoppedRocks - i].Set) &&
                    patterns[stoppedRocks].RockIndex == patterns[stoppedRocks - i].RockIndex &&
                    patterns[stoppedRocks].Jet == patterns[stoppedRocks - i].Jet)
                {
                    Console.WriteLine("Found repeated pattern at rock index " + stoppedRocks + " topY=" + topY + " i=" + i);
                    cycleSize = i;
                    stoppedRocks++;
                    goto result;
                }
            }
        }

        stoppedRocks++;
    }
}

result:

// Use the cycle information to calculate high
var cycleSum = diffs.TakeLast(cycleSize).Sum();

BigInteger result = topY +
                    cycleSum * ((1000000000000L - stoppedRocks) / cycleSize) +
                    diffs.TakeLast(cycleSize).Take((int)((1000000000000L - stoppedRocks) % cycleSize)).Sum();

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
    
    patterns.Add((map.Where(p => p.Y >= topY - patternSize).Select(p => (p.X, p.Y - topY)).ToHashSet(), rockIndex, jet));

    map.RemoveAll(p => p.Y < topY - 2 * patternSize);
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


