
var lines = File.ReadAllLines("Input.txt");
var currentDir = Dir.Root;

foreach (var line in lines)
{
    if (line.StartsWith("$ cd"))
    {
        if (line == "$ cd /")       currentDir = Dir.Root;
        else if (line == "$ cd ..") currentDir = currentDir.Parent;
        else
        {
            var name = line[2..].Split(" ")[1];
            var existingDir = currentDir.Dirs.FirstOrDefault(x => x.Name.EndsWith(name + "/"));

            if (existingDir == null) currentDir.Dirs.Add(new Dir(currentDir.Name + name + "/", currentDir));
            else                     currentDir = existingDir;
        }
    }
    else if (line.StartsWith("$ ls")) {}
    else
    {
        if (line.StartsWith("dir")) currentDir.Dirs.Add(new Dir(currentDir.Name + line.Split(" ")[1] + "/", currentDir));
        else                        currentDir.Files.Add(new F(line.Split(" ")[1], int.Parse(line.Split(" ")[0])));
    }
}

Console.WriteLine(TotalPart1(Dir.Root));

var freeSpace = 70000000 - Dir.Root.Size();
var spaceToFree = 30000000 - freeSpace;

Console.WriteLine(GetDirs(Dir.Root).OrderBy(x => x.Size()).First(x => x.Size() >= spaceToFree).Size());

int TotalPart1(Dir d)
{
    if (d == null) return 0;
    return (d.Size() <= 100000 ? d.Size() : 0) + d.Dirs.Sum(TotalPart1);
}

IEnumerable<Dir> GetDirs(Dir d)
{
    foreach (var subDir in d.Dirs.SelectMany(GetDirs))
    {
        yield return subDir;
    }

    yield return d;
}

internal record Dir(string Name, List<F> Files, List<Dir> Dirs, Dir Parent)
{
    public static Dir Root = new("/");
    public Dir(string Name) : this(Name, new List<F>(), new List<Dir>(), null) { }
    public Dir(string Name, Dir Parent) : this(Name, new List<F>(), new List<Dir>(), Parent) { }
    public int Size() => Files.Sum(x => x.Size) + Dirs.Sum(x => x.Size());
}

record F(string Name, int Size);