
List<(string Direction, int Steps)> moves = File.ReadAllLines("Input.txt").Select(x => (x.Split()[0], int.Parse(x.Split()[1]))).ToList();

var visited = new HashSet<(int, int)> {(0, 0)};
var knots = new List<(int X, int Y)>();
for (var i = 0; i < 10; i++)
{
    knots.Add((0, 0));
}

foreach (var (Direction, Steps) in moves)
{
    //Console.WriteLine(Direction + " " + Steps + "\n");

    for (var i  = 0; i < Steps; i++)
    {
        (int X, int Y) head = knots[0];

        if (Direction == "U")
        {
            head = (head.X, head.Y - 1);
        }
        else if (Direction == "D")
        {
            head = (head.X, head.Y + 1);
        }
        else if (Direction == "L")
        {
            head = (head.X - 1, head.Y);
        }
        else if (Direction == "R")
        {
            head = (head.X + 1, head.Y);
        }

        knots[0] = head;

        for (var j = 1; j < 10; j++)
        {
            knots[j] = NextKnot(knots[j - 1], knots[j]);
        }

        visited.Add(knots[9]);
    }

    //Print();
}

Console.WriteLine(visited.Count);

(int, int) NextKnot((int X, int Y) head, (int X, int Y) tail)
{
    if (head.X - tail.X >= 2)
    {
        tail.X++;
        tail.Y += Dir(head.Y, tail.Y);
    }
    else if (tail.X - head.X >= 2)
    {
        tail.X--;
        tail.Y += Dir(head.Y, tail.Y);
    }
    else if (head.Y - tail.Y >= 2)
    {
        tail.Y++;
        tail.X += Dir(head.X, tail.X);
    }
    else if (tail.Y - head.Y >= 2)
    {
        tail.Y--;
        tail.X += Dir(head.X, tail.X);
    }

    return tail;
}

int Dir(int x1, int x2) => x1 == x2 ? 0 : x1 > x2 ? 1 : -1;

void Print()
{
    var nx = 50;
    var ny = 50;
    var sx = Math.Abs(knots.MinBy(k => k.X).X);
    var sy = Math.Abs(knots.MinBy(k => k.Y).Y);

    var map = new List<char[]>();

    for (var i = 0; i < ny; i++)
    {
        map.Add(new string('.', nx).ToCharArray());
    }

    map[sy][sx] = 's';

    for (var i = 9; i >= 0; i--)
    {
        var k = knots[i];
        var c = i.ToString()[0];
        map[sy + k.Y][sx + k.X] = c == '0' ? 'H' : c;
    }

    foreach (var s in map)
    {
        Console.WriteLine(new string(s));
    }
    
    Console.WriteLine();
}