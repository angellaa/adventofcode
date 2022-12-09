
using System.Runtime.CompilerServices;

List<(string Direction, int Steps)> moves = File.ReadAllLines("Input.txt").Select(x => (x.Split()[0], int.Parse(x.Split()[1]))).ToList();

var tail = (X: 0, Y: 0);
var head = (X: 0, Y: 0);
var visited = new HashSet<(int, int)> { tail };

foreach (var (Direction, Steps) in moves)
{
    for (var i  = 0; i < Steps; i++)
    {
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

        UpdateTail();
        Console.WriteLine("Move " + Direction + ": " + head + " " + tail);
    }
}

Console.WriteLine(visited.Count);

void UpdateTail()
{
    if (head.X - tail.X >= 2)
    {
        tail.X++;
        tail.Y += head.Y - tail.Y;
    }

    if (tail.X - head.X >= 2)
    {
        tail.X--;
        tail.Y += head.Y - tail.Y;
    }

    if (head.Y - tail.Y >= 2)
    {
        tail.Y++;
        tail.X += head.X - tail.X;
    }

    if (tail.Y - head.Y >= 2)
    {
        tail.Y--;
        tail.X += head.X - tail.X;
    }

    visited.Add(tail);
}


