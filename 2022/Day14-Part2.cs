
var paths = File.ReadAllLines("Input.txt").ToList();

var map = new List<char[]>();

var offset = 1000;

for (var i = 0; i < 2000; i++)
    map.Add(new string('.', 2000).ToCharArray());

var floorY = int.MinValue;

foreach (var path in paths)
{
    List<(int X, int Y)> points = path.Split(" -> ")
                                      .Select(x => (int.Parse(x.Split(",")[0]), int.Parse(x.Split(",")[1])))
                                      .ToList();

    for (var i = 0; i < points.Count - 1; i++)
    {
        var p1 = points[i];
        var p2 = points[i + 1];
        var d = (X: Math.Sign(p2.X - p1.X), Y: Math.Sign(p2.Y - p1.Y));

        do
        {
            map[p1.Y][offset + p1.X] = '#';
            p1 = (p1.X + d.X, p1.Y + d.Y);
        } while (p1 != p2);

        map[p2.Y][offset + p1.X] = '#';
    }

    floorY = Math.Max(points.MaxBy(p => p.Y).Y, floorY);
}

floorY += 2;

map[500 + offset][0] = '+';

while (true)
{
    int sx = 500 + offset;
    int sy = 0;

    while (Fall(sx, sy) != null)
    {
        (sx, sy) = Fall(sx, sy).Value;

        if (sy == floorY - 1)
        {
            break;
        }
    }

    map[sy][sx] = 'o';

    if (sy == 0)
    {
        break;
    }
}

Console.WriteLine(map.SelectMany(x => x).Count(x => x == 'o'));


(int, int)? Fall(int x, int y)
{
    if (y == floorY - 1) return (x, y);
    if (map[y + 1][x] == '.') return (x, y + 1);
    if (x > 0 && map[y + 1][x - 1] == '.') return (x - 1, y + 1);
    if (map[y + 1][x + 1] == '.') return (x + 1, y + 1);
    return null;
}

