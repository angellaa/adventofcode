
var lines = File.ReadAllLines("Input.txt");
var currentDir = Dir.Root;
var ls = false;

foreach (var line in lines)
{
    if (line.StartsWith("$ cd"))
    {
        if (line == "$ cd /") currentDir = Dir.Root;
        else if (line == "$ cd ..") currentDir = currentDir.Parent ?? Dir.Root;
        else
        {
            var dirName = line[2..].Split(" ")[1];

            var dir = currentDir.Dirs.FirstOrDefault(x => x.Name.EndsWith(dirName + "/"));

            if (dir == null)
            {
                var newDir = new Dir(currentDir.Name + dirName + "/", currentDir);
                currentDir.Dirs.Add(newDir);
            }
            else
            {
                currentDir = dir;
            }
        }
        ls = false;
    }
    else if (line.StartsWith("$ ls"))
    {
        ls = true;
    }
    else if (ls)
    {
        if (line.StartsWith("dir"))
        {
            currentDir.Dirs.Add(new Dir(currentDir.Name + line.Split(" ")[1] + "/", currentDir));
        }
        else
        {
            currentDir.Files.Add(new F(line.Split(" ")[1], int.Parse(line.Split(" ")[0])));
        }
    }
}

Console.WriteLine(TotalPart1(Dir.Root));

var freeSpace = 70000000 - Dir.Root.Size();
var needSaved = 30000000 - freeSpace;
var allDirs = GetDirs(Dir.Root);

Console.WriteLine(allDirs.OrderBy(x => x.Size()).First(x => x.Size() >= needSaved).Size());

int TotalPart1(Dir d)
{
    if (d == null) return 0;

    var total = 0;
    var size = d.Size();
    if (size <= 100000) total += size;

    total += d.Dirs.Sum(TotalPart1);

    return total;
}

IEnumerable<Dir> GetDirs(Dir d)
{
    foreach (var subDir in d.Dirs.SelectMany(GetDirs))
    {
        yield return subDir;
    }

    yield return d;
}


record Dir(string Name, List<F> Files, List<Dir> Dirs, Dir Parent)
{
    public static Dir Root = new("/");
    public Dir(string Name) : this(Name, new List<F>(), new List<Dir>(), null) { }
    public Dir(string Name, Dir Parent) : this(Name, new List<F>(), new List<Dir>(), Parent) { }
    public int Size() => Files.Sum(x => x.Size) + Dirs.Sum(x => x.Size());
}

record F(string Name, int Size);