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
var lastY = 0L;

var cycleLength = 35; // Use cycle 60050 for part 2 input data or 35 for part 2 example;

var diffs = new long[cycleLength * 2];
var diffIndex = 0;
var firstHighIncrease = 0L;
var secondHighIncrease = 0L;

while (true)
{
    jetIndex = (jetIndex + 1) % jets.Length;
    var jet = jets[jetIndex];

    if (jet == '>') MoveRockRight();
    else if (jet == '<') MoveRockLeft();
    else throw new Exception("ops");
    
    if (!MoveRockDown())
    {
        FixRock();
        NextRock();
        stoppedRocks++;

        if (stoppedRocks < cycleLength * 2)
        {
            diffs[stoppedRocks] = topY - lastY;

        }
        else if (stoppedRocks == cycleLength * 2)
        {
            firstHighIncrease = diffs.Take(cycleLength).Sum();
            secondHighIncrease = diffs.TakeLast(cycleLength).Sum();
        }
        else
        {
            firstHighIncrease -= diffs[diffIndex];
            firstHighIncrease += diffs[(diffIndex + cycleLength) % (2 * cycleLength)];

            secondHighIncrease -= diffs[(diffIndex + cycleLength) % (2 * cycleLength)];

            diffs[diffIndex] = topY - lastY;
            secondHighIncrease += diffs[diffIndex];

            diffIndex = (diffIndex + 1) % (2 * cycleLength);

            if (firstHighIncrease == secondHighIncrease)
            {
                var first = diffs.Skip(diffIndex).Take(cycleLength).ToList();
                var second = diffs.Skip(diffIndex + cycleLength).Concat(diffs.Take(diffIndex)).ToList();

                if (first.SequenceEqual(second))
                {
                    Console.WriteLine("Cycle found starting at " + stoppedRocks + " with high increase of " + firstHighIncrease);
                    break;
                }
                else
                {
                    Console.WriteLine("False positive - no cycle found starting at " + stoppedRocks);
                }
            }
        }

        lastY = topY;

        if (stoppedRocks > 1000000)
        {
            Console.WriteLine("no cycle found");
            return;
        }
    }
}

// Use the cycle information to calculate high
BigInteger result = topY +
                    firstHighIncrease * ((1000000000000 - stoppedRocks) / cycleLength) +
                    diffs.Take((int)((1000000000000 - stoppedRocks) % cycleLength)).Sum();

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


