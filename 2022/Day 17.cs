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

while(true)
{
    jetIndex = (jetIndex + 1) % jets.Length;
    var jet = jets[jetIndex];

    if (jet == '>') MoveRockRight();
    if (jet == '<') MoveRockLeft();
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

        if (stoppedRocks == 2022) break;
    }
}

Console.WriteLine(topY);

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


