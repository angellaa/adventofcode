
var paths = File.ReadAllLines("Input.txt").ToList();

var map = new List<char[]>();

for (var i = 0; i < 600; i++)
    map.Add(new string('.', 600).ToCharArray());

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
            map[p1.Y][p1.X] = '#';
            p1 = (p1.X + d.X, p1.Y + d.Y);
        } while (p1 != p2);

        map[p2.Y][p1.X] = '#';
    }
}

map[500][0] = '+';

while (true)
{
    var sx = 500;
    var sy = 0;

    while (Fall(sx, sy) != null)
    {
        (sx, sy) = Fall(sx, sy).Value;
        if (sy > 520)
        {
            Console.WriteLine(map.SelectMany(x => x).Count(x => x == 'o'));

            Console.ReadKey();
        }
    }

    map[sy][sx] = 'o';
}

(int, int)? Fall(int x, int y)
{
    if (y > 550) return null;
    if (map[y + 1][x] == '.') return (x, y + 1);
    if (x > 0 && map[y + 1][x - 1] == '.') return (x - 1, y + 1);
    if (map[y + 1][x + 1] == '.') return (x + 1, y + 1);
    return null;
}

