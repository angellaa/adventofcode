using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using NUnit.Framework;

namespace AdventOfCode2020
{
    [TestFixture]
    public class AdventOfCode
    {
        public class Day1
        {
            [Test]
            public void Part1()
            {
                var report = File.ReadAllLines("Day1.txt").Select(int.Parse).ToList();

                var result = from x in report
                             from y in report
                             where x + y == 2020
                             select x * y;

                Assert.That(result.First(), Is.EqualTo(1010884));
            }
        
            [Test]
            public void Part2()
            {
                var report = File.ReadAllLines("Day1.txt").Select(int.Parse).ToList();
            
                var result = from x in report
                             from y in report
                             from z in report
                             where x + y + z == 2020
                             select x * y * z;

                Assert.That(result.First(), Is.EqualTo(253928438));
            }
        }

        public class Day2
        {
            [Test]
            public void Part1()
            {
                var list = File.ReadAllLines("Day2.txt").ToList();
                var validPasswords = list.Count(entry => PasswordEntry1.Parse(entry).Valid());

                Assert.That(validPasswords, Is.EqualTo(467));
            }
            
            [Test]
            public void Part2()
            {
                var list = File.ReadAllLines("Day2.txt").ToList();
                var validPasswords = list.Count(entry => PasswordEntry2.Parse(entry).Valid());
                
                Assert.That(validPasswords, Is.EqualTo(441));
            }
            
            private record PasswordEntry1(int LowestNumber, int HighestNumber, char Letter, string Password)
            {
                public static PasswordEntry1 Parse(string s)
                {
                    var parts = s.Split();
                    var min = int.Parse(parts[0].Split('-')[0]);
                    var max = int.Parse(parts[0].Split('-')[1]);
                    var letter = parts[1][0];
                    var password = parts[2];
                    
                    return new PasswordEntry1(min, max, letter, password);
                }

                public bool Valid()
                {
                    var count = Password.Count(x => x == Letter);
                    return count >= LowestNumber && count <= HighestNumber;
                }
            }
            
            private record PasswordEntry2(int Position1, int Position2, char Letter, string Password)
            {
                public static PasswordEntry2 Parse(string s)
                {
                    var parts = s.Split();
                    var position1 = int.Parse(parts[0].Split('-')[0]) - 1;
                    var position2 = int.Parse(parts[0].Split('-')[1]) - 1;
                    var letter = parts[1][0];
                    var password = parts[2];
                    
                    return new PasswordEntry2(position1, position2, letter, password);
                }

                public bool Valid()
                {
                    var firstLetter = Password[Position1];
                    var secondLetter = Password[Position2];

                    return firstLetter == Letter && secondLetter != Letter || 
                           firstLetter != Letter && secondLetter == Letter;
                }
            }
        }
        
        public class Day3
        {
            [Test]
            public void Day3_Part1()
            {
                var input = File.ReadAllLines("Day3.txt").ToList();
                var trees = CountTrees(input, 3, 1);

                Assert.That(trees, Is.EqualTo(198));
            }
        
            [Test]
            public void Day3_Part2()
            {
                var input = File.ReadAllLines("Day3.txt").ToList();
                var treesProduct = 1L * 
                                   CountTrees(input, 1, 1) *
                                   CountTrees(input, 3, 1) *
                                   CountTrees(input, 5, 1) *
                                   CountTrees(input, 7, 1) *
                                   CountTrees(input, 1, 2);

                Assert.That(treesProduct, Is.EqualTo(5140884672));
            }
            
            private static int CountTrees(List<string> input, int right, int down)
            {
                var trees = 0;
                var x = 0;

                for (var index = 0; index < input.Count; index += down)
                {
                    var row = input[index];
                    if (row[x] == '#') trees++;
                    x = (x + right) % input[0].Length;
                }

                return trees;
            }
        }

        public class Day4
        {
            [Test]
            public void Day4_Part1()
            {
                var passports = new List<Dictionary<string, string>>();
                var newPassport = new Dictionary<string, string>();
                
                foreach (var line in File.ReadAllLines("Day4.txt"))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        passports.Add(newPassport);
                        newPassport = new Dictionary<string, string>();
                        continue;
                    }

                    foreach (var pair in line.Split(" "))
                    {
                        var keyValue = pair.Split(':');
                        newPassport[keyValue[0]] = keyValue[1];
                    }
                }

                var valids = 0;
                
                foreach (var passport in passports)
                {
                    if (passport.ContainsKey("byr") &&
                        passport.ContainsKey("iyr") &&
                        passport.ContainsKey("eyr") &&
                        passport.ContainsKey("hgt") &&
                        passport.ContainsKey("hcl") &&
                        passport.ContainsKey("ecl") &&
                        passport.ContainsKey("pid"))
                    {
                        valids++;
                    }
                }

                Assert.That(valids, Is.EqualTo(190));
            }

            [Test]
            public void Day4_Part2()
            {
                var passports = new List<Dictionary<string, string>>();
                var newPassport = new Dictionary<string, string>();

                foreach (var line in File.ReadAllLines("Day4.txt"))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        passports.Add(newPassport);
                        newPassport = new Dictionary<string, string>();
                        continue;
                    }

                    foreach (var pair in line.Split(" "))
                    {
                        var keyValue = pair.Split(':');
                        newPassport[keyValue[0]] = keyValue[1];
                    }
                }

                var valids = 0;

                foreach (var passport in passports)
                {
                    if (passport.ContainsKey("byr") &&
                        passport.ContainsKey("iyr") &&
                        passport.ContainsKey("eyr") &&
                        passport.ContainsKey("hgt") &&
                        passport.ContainsKey("hcl") &&
                        passport.ContainsKey("ecl") &&
                        passport.ContainsKey("pid"))
                    {
                        if (Between(passport["byr"], 1920, 2002) &&
                            Between(passport["iyr"], 2010, 2020) &&
                            Between(passport["eyr"], 2020, 2030) &&
                            ValidHeight(passport["hgt"]) &&
                            ValidHairColor(passport["hcl"]) &&
                            ValidEyeColor(passport["ecl"]) &&
                            ValidPid(passport["pid"]))
                        {
                            valids++;
                        }
                    }
                }

                Assert.That(valids, Is.EqualTo(121));
            }
            
            private static bool Between(string s, int min, int max) => int.TryParse(s, out var d) && d >= min && d <= max;
            private bool ValidHeight(string hgt) => hgt.EndsWith("cm") ? Between(hgt[..^2], 150, 193) : Between(hgt[..^2], 59, 76);

            private bool ValidHairColor(string hcl) => hcl.StartsWith("#") && hcl[1..].Length == 6 && hcl[1..].All(x => char.IsDigit(x) || (x >= 'a' && x <= 'f'));

            private bool ValidEyeColor(string ecl) => ecl is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth";

            private bool ValidPid(string pid) => pid.Length == 9 && pid.All(char.IsDigit);
        }

        public class Day5
        {
            [Test]
            public void Day5_Part1()
            {
                int maxSeatId = int.MinValue;
                
                foreach (var line in File.ReadAllLines("Day5.txt"))
                {
                    var a = 0;
                    var b = 128;
                    
                    foreach (var r in line[..7])
                    {
                        if (r == 'F') b = (a + b) / 2;
                        else a = (a + b) / 2;
                    }

                    var row = a;

                    a = 0;
                    b = 8;
                    
                    foreach (var c in line[7..])
                    {
                        if (c == 'L') b = (a + b) / 2;
                        else a = (a + b) / 2;
                    }

                    var col = a;
                    var seatId = row * 8 + col;

                    if (seatId > maxSeatId)
                    {
                        maxSeatId = seatId;
                    }
                }

                Assert.That(maxSeatId, Is.EqualTo(978));
            }
            
            [Test]
            public void Day5_Part2()
            {
                var seats = new List<int>();
                
                foreach (var line in File.ReadAllLines("Day5.txt"))
                {
                    var a = 0;
                    var b = 128;
                    
                    foreach (var r in line[..7])
                    {
                        if (r == 'F') b = (a + b) / 2;
                        else a = (a + b) / 2;
                    }

                    var row = a;

                    a = 0;
                    b = 8;
                    
                    foreach (var c in line[7..])
                    {
                        if (c == 'L') b = (a + b) / 2;
                        else a = (a + b) / 2;
                    }

                    var col = a;
                    var seatId = row * 8 + col;
                    seats.Add(seatId);
                }

                var v = seats.OrderBy(x => x).ToArray();

                for (var i = 0; i < 1031; i++)
                    if (v.Contains(i-1) && v.Contains(i+1) && !v.Contains(i))
                        Console.WriteLine(i);
            }
        }

        public class Day6
        {
            [Test]
            public void Day6_Part1()
            {
                var answers = new HashSet<char>();
                var result = 0;
                foreach (var line in File.ReadAllLines("Day6.txt"))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        result += answers.Count();
                        answers = new HashSet<char>();
                        continue;
                    }

                    foreach (var c in line)
                    {
                        answers.Add(c);
                    }
                }

                Assert.That(result, Is.EqualTo(6161));
            }
        
            [Test]
            public void Day6_Part2()
            {
                var answers = new HashSet<char>();
                var result = 0;
                var firstLine = true;
            
                foreach (var line in File.ReadAllLines("Day6.txt"))
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        result += answers.Count();
                        answers = new HashSet<char>();
                        firstLine = true;
                        continue;
                    }

                    if (firstLine)
                    {
                        answers = new HashSet<char>(line);
                        firstLine = false;
                    }
                    else
                    {
                        answers = new HashSet<char>(answers.Intersect(new HashSet<char>(line)));
                    }
                }

                Assert.That(result, Is.EqualTo(2971));
            }
        }

        public class Day7
        {
            [Test]
            public void Day7_Part1()
            {
                var lines = File.ReadAllLines("Day7.txt");
                var n = lines.Length;
                var matrix = new long[n, n];

                var bagNames = lines.Select(x => x.Split(" bags contain ")[0]).ToList();

                BuildMatrix(lines, matrix, bagNames);

                var index = bagNames.IndexOf("shiny gold");
                var visited = new HashSet<int> {index};
                var toVisit = new Queue<int>();
                toVisit.Enqueue(index);

                while (toVisit.Count > 0)
                {
                    var indexToVisit = toVisit.Dequeue();
                    visited.Add(indexToVisit);

                    var indexes = Enumerable.Range(0, n).Where(i => matrix[i, indexToVisit] > 0);
                    foreach (var i in indexes)
                    {
                        if (!visited.Contains(i))
                        {
                            toVisit.Enqueue(i);
                        }
                    }
                }

                Console.WriteLine(visited.Count - 1);
            }

            private static void BuildMatrix(string[] lines, long[,] matrix, List<string> bagNames)
            {
                foreach (var line in lines)
                {
                    if (line.Contains("bags contain no other bags"))
                    {
                        continue;
                    }

                    var bagName = line.Split(" bags contain ")[0];

                    foreach (var subLine in line.Split(" bags contain ")[1].Split(", "))
                    {
                        var parts = subLine.Split(" ");
                        var c = int.Parse(parts[0]);
                        var subBagName = parts[1] + " " + parts[2];

                        matrix[bagNames.IndexOf(bagName), bagNames.IndexOf(subBagName)] = c;
                    }
                }
            }

            [Test]
            public void Day7_Part2()
            {
                var lines = File.ReadAllLines("Day7.txt");
                var n = lines.Length;
                var matrix = new long[n, n];

                var bagNames = lines.Select(x => x.Split(" bags contain ")[0]).ToList();

                BuildMatrix(lines, matrix, bagNames);

                var index = bagNames.IndexOf("shiny gold");

                var bagsCount = CountBags(index, n, matrix, new Dictionary<long, long>());

                Assert.That(bagsCount, Is.EqualTo(20189));
            }

            private static long CountBags(int index, int n, long[,] matrix, Dictionary<long, long> visited)
            {
                if (visited.ContainsKey(index))
                {
                    return visited[index];
                }
            
                if (Enumerable.Range(0, n).All(i => matrix[index, i] == 0))
                {
                    visited[index] = 0;
                    return 0;
                }

                var indexes = Enumerable.Range(0, n).Where(i => matrix[index, i] > 0);

                var result = indexes.Sum(i => matrix[index, i] + matrix[index, i] * CountBags(i, n, matrix, visited));
                visited[index] = result;
                return result;
            }
        }

        public class Day8
        {
            [Test]
            public void Day8_Part1()
            {
                var lines = File.ReadAllLines("Day8.txt");
                var n = lines.Length;
                int p = 0;
                var runs = new bool[n];
                var accumulator = 0;

                while (!runs[p])
                {
                    if (lines[p].Contains("nop "))
                    {
                        runs[p] = true;
                        p++;
                    }
                    else if (lines[p].StartsWith("acc "))
                    {
                        runs[p] = true;
                        accumulator += int.Parse(lines[p].Split(" ")[1]);
                        p++;
                    }
                    else
                    {
                        runs[p] = true;
                        p += int.Parse(lines[p].Split(" ")[1]);
                    }
                }

                Assert.That(accumulator, Is.EqualTo(1548));
            }

            [Test]
            public void Day8_Part2()
            {
                var lines = File.ReadAllLines("Day8.txt");
                var n = lines.Length;

                for (var i = 0; i < n; i++)
                {
                    var code = lines.ToArray();
                    if (code[i].Contains("jmp")) code[i] = code[i].Replace("jmp", "nop");
                    else if (code[i].Contains("nop")) code[i] = code[i].Replace("nop", "jmp");

                    var (terminate, accumulator) = Terminate(code);
                    if (terminate)
                    {
                        Assert.That(accumulator, Is.EqualTo(1375));
                        Assert.Pass();
                    }
                }

                Assert.Fail("There is no solution");
            }
            
            private static (bool, int) Terminate(string[] lines)
            {
                int n = lines.Length;
                int p = 0;
                var runs = new bool[n];
                var accumulator = 0;
            
                while (!runs[p])
                {
                    if (lines[p].Contains("nop "))
                    {
                        runs[p] = true;
                        p++;
                    }
                    else if (lines[p].StartsWith("acc "))
                    {
                        runs[p] = true;
                        accumulator += int.Parse(lines[p].Split(" ")[1]);
                        p++;
                    }
                    else
                    {
                        runs[p] = true;
                        p += int.Parse(lines[p].Split(" ")[1]);
                    }

                    if (p == n) return (true, accumulator);
                }

                return (false, 0);
            }
        }

        public class Day9
        {
            [Test]
            public void Day9_Part1()
            {
                var nums = File.ReadAllLines("Day9.txt").Select(BigInteger.Parse).ToList();

                for (var i = 25; i < nums.Count; i++)
                {
                    var valid = false;
                
                    for (var j = i - 25; j < i; j++)
                    for (var k = j + 1; k < i; k++)
                        if (nums[j] + nums[k] == nums[i])
                        {
                            valid = true;
                            break;
                        }

                    if (!valid)
                    {
                        Assert.That(nums[i], Is.EqualTo(new BigInteger(27911108)));
                        Assert.Pass();
                    }
                }
            
                Assert.Fail("Not found");
            }
        
            [Test]
            public void Day9_Part2()
            {
                var nums = File.ReadAllLines("Day9.txt").Select(BigInteger.Parse).ToList();
                BigInteger n = 27911108;

                var size = 2;

                while (true)
                {
                    for (var i = 0; i < nums.Count; i++)
                    {
                        var range = nums.Skip(i).Take(size).ToList();
                        var sum = range.Aggregate<BigInteger, BigInteger>(0, (current, p) => current + p);

                        if (sum == n)
                        {
                            var result = range.Min() + range.Max();
                            Assert.That(result, Is.EqualTo(new BigInteger(4023754)));
                            Assert.Pass();
                        }
                    }

                    size++;
                }
            }            
        }

        public class Day10
        {
            [Test]
            public void Day10_Part1()
            {
                var adapters = File.ReadAllLines("Day10.txt").Select(int.Parse).OrderBy(x => x).ToList();

                var c = 0;
                var ones = 0;
                var threes = 0;
            
                foreach (var adapter in adapters)
                {
                    if (adapter - c == 1) ones++;
                    if (adapter - c == 3) threes++;
                    c = adapter;
                }

                Assert.That(ones * (threes + 1), Is.EqualTo(1656));
            }

            [Test]
            public void Day10_Part2()
            {
                var nums = File.ReadAllLines("Day10.txt").Select(int.Parse).OrderBy(x => x).ToList();
                nums.Insert(0, 0);
                nums.Add(nums.Max() + 3);
                
                var n = nums.Count;

                var counts = new BigInteger[n];
                counts[n - 1] = 1;
            
                for (var i = n - 1; i > 0; i--)
                {
                    if (i - 1 >= 0 && i - 1 < n && nums[i] - nums[i - 1] <= 3) { counts[i - 1] += counts[i]; }
                    if (i - 2 >= 0 && i - 2 < n && nums[i] - nums[i - 2] <= 3) { counts[i - 2] += counts[i]; }
                    if (i - 3 >= 0 && i - 3 < n && nums[i] - nums[i - 3] <= 3) { counts[i - 3] += counts[i]; }
                }
            
                Assert.That(counts[0], Is.EqualTo(BigInteger.Parse("56693912375296")));
            }
        }

        public class Day11
        {
            [Test]
            public void Day11_Part1()
            {
                var nums = File.ReadAllLines("Day11.txt");
                var n = nums.Length;
                var m = nums[0].Length;
                var map = new char[n, m];

                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    map[i, j] = nums[i][j];

                bool more = true;

                while (more)
                {
                    var newMap = (char[,]) map.Clone();
                    var previousMap = (char[,]) map.Clone();

                    // Rule 1
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] == 'L' && AdjacentSeats(map, n, m, i, j) == 0)
                            newMap[i, j] = '#';

                    map = newMap;
                    newMap = (char[,]) map.Clone();

                    // Rule 2
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] == '#' && AdjacentSeats(map, n, m, i, j) >= 4)
                            newMap[i, j] = 'L';

                    map = newMap;

                    // Check if continue or not
                    more = false;
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] != previousMap[i, j])
                            more = true;
                }

                var occupied = 0;
                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    if (map[i, j] == '#')
                        occupied++;

                Assert.That(occupied, Is.EqualTo(2494));
            }

            [Test]
            public void Day11_Part2()
            {
                var nums = File.ReadAllLines("Day11.txt");
                var n = nums.Length;
                var m = nums[0].Length;
                var map = new char[n, m];

                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    map[i, j] = nums[i][j];

                bool more = true;

                while (more)
                {
                    var newMap = (char[,]) map.Clone();
                    var previousMap = (char[,]) map.Clone();

                    // Rule 1
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] == 'L' && AdjacentSeats2(map, n, m, i, j) == 0)
                            newMap[i, j] = '#';

                    map = newMap;
                    newMap = (char[,]) map.Clone();

                    // Rule 2
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] == '#' && AdjacentSeats2(map, n, m, i, j) >= 5)
                            newMap[i, j] = 'L';

                    map = newMap;

                    // Check if continue or not
                    more = false;
                    for (var i = 0; i < n; i++)
                    for (var j = 0; j < m; j++)
                        if (map[i, j] != previousMap[i, j])
                            more = true;
                }

                var occupied = 0;
                for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    if (map[i, j] == '#')
                        occupied++;

                Assert.That(occupied, Is.EqualTo(2306));
            }
            
            private int AdjacentSeats(char[,] map, int n, int m, int r, int c)
            {
                var result = 0;
                for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                    if (i != 0 || j != 0)
                        result += Occupied(map, n, m, r, c, i, j);
                return result;
            }
            
            private int AdjacentSeats2(char[,] map, int n, int m, int r, int c)
            {
                var result = 0;
                for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                    if (i != 0 || j != 0)
                        result += Occupied2(map, n, m, r, c, i, j);
                return result;
            }

            private int Occupied(char[,] map, int n, int m, int r, int c, int dr, int dc)
            {
                r += dr;
                c += dc;
                if (r < 0 || r >= n) return 0;
                if (c < 0 || c >= m) return 0;
                return map[r, c] == '#' ? 1 : 0;
            }

            private int Occupied2(char[,] map, int n, int m, int r, int c, int dr, int dc)
            {
                while (true)
                {
                    r += dr;
                    c += dc;
                    if (r < 0 || r >= n) return 0;
                    if (c < 0 || c >= m) return 0;
                    if (map[r, c] == 'L') return 0;
                    if (map[r, c] == '#') return 1;
                }
            }
        }

        public class Day12
        {
            [Test]
            public void Day12_Part1()
            {
                var directions = File.ReadAllLines("Day12.txt");

                var p = (x: 0, y: 0);
                var dirs = new List<(int, int)>
                {
                    (-1, +0),
                    (+0, +1),
                    (+1, +0),
                    (+0, -1)
                };
                var d = 2;

                Console.WriteLine(p);
                foreach (var direction in directions)
                {
                    var l = direction[0];
                    var n = int.Parse(direction.Substring(1));
                    if (l == 'N') p.y += n;
                    if (l == 'S') p.y -= n;
                    if (l == 'W') p.x -= n;
                    if (l == 'E') p.x += n;
                    if (l == 'F') p = (p.x + n * dirs[d].Item1, p.y + n * dirs[d].Item2);
                    if (l == 'L') d = (d - (n / 90) + 4) % 4;
                    if (l == 'R') d = (d + (n / 90)) % 4;
                    //Console.WriteLine(direction + " " + p + " direction = " + dirs[d]);
                }

                Assert.That(Math.Abs(p.x) + Math.Abs(p.y), Is.EqualTo(858));
            }

            [Test]
            public void Day12_Part2()
            {
                var directions = File.ReadAllLines("Day12.txt");

                var p = (x: 0, y: 0);
                var waypoint = (x: 10, y: 1);

                foreach (var direction in directions)
                {
                    var l = direction[0];
                    var n = int.Parse(direction.Substring(1));
                    if (l == 'N') waypoint.y += n;
                    if (l == 'S') waypoint.y -= n;
                    if (l == 'W') waypoint.x -= n;
                    if (l == 'E') waypoint.x += n;
                    if (l == 'F') p = (p.x + n * waypoint.x, p.y + n * waypoint.y);
                    if (l == 'L')
                    {
                        var times = n / 90;
                        for (var i = 0; i < times; i++)
                            waypoint = TurnLeft(waypoint);
                    }

                    if (l == 'R')
                    {
                        var times = n / 90;
                        for (var i = 0; i < times; i++)
                            waypoint = TurnRight(waypoint);
                    }
                }

                Assert.That(Math.Abs(p.x) + Math.Abs(p.y), Is.EqualTo(39140));
            }
            
            private (int, int) TurnLeft((int x, int y) waypoint) => (-waypoint.y, waypoint.x);
            private (int, int) TurnRight((int x, int y) waypoint) => (waypoint.y, -waypoint.x);
        }

        public class Day13
        {
            [Test]
            public void Day13_Part1()
            {
                var lines = File.ReadAllLines("Day13.txt");
                var start = int.Parse(lines[0]);
                var ids = lines[1].Split(',').Where(x => x != "x").Select(int.Parse).OrderBy(x => x).ToList();

                var minId = 0;
                int min = int.MaxValue;

                foreach (var id in ids)
                {
                    var next = id * ((start / id) + 1);
                    var diff = next - start;
                    if (diff < min)
                    {
                        min = diff;
                        minId = id;
                    }
                }

                Assert.That(minId * min, Is.EqualTo(2947));
            }

            [TestCase("17,x,13,19", 3417)]
            [TestCase("67,7,59,61", 754018)]
            [TestCase("67,x,7,59,61", 779210)]
            [TestCase("67,7,x,59,61", 1261476)]
            [TestCase("1789,37,47,1889", 1202161486)]
            public void Day13_Part2_Examples(string input, long expected)
            {
                Test_Day13_Part2(input, expected);
            }

            [Test]
            public void Day13_Part2()
            {
                Test_Day13_Part2(File.ReadAllLines("Day13.txt")[1], 526090562196173L);
            }

            private void Test_Day13_Part2(string input, long expected)
            {
                var ids = new List<int>();
                var t = new List<int>();
                var parts = input.Split(',');
                for (var i = 0; i < parts.Length; i++)
                {
                    if (parts[i] != "x")
                    {
                        ids.Add(int.Parse(parts[i]));
                        t.Add(i);
                    }
                }

                long id = ids[0];
                long step = t[0];

                while (!Valid(ref step))
                {
                    id += step;
                }

                Assert.That(id, Is.EqualTo(expected));

                bool Valid(ref long step)
                {
                    var result = true;
                    step = 1;

                    for (var i = 0; i < ids.Count; i++)
                    {
                        if ((id + t[i]) % ids[i] != 0)
                        {
                            result = false;
                            continue;
                        }

                        step *= ids[i];
                    }

                    return result;
                }
            }
        }

        public class Day14
        {
            [Test]
            public void Day14_Part1()
            {
                var lines = File.ReadAllLines("Day14.txt");

                var memory = new Dictionary<long, long>();
                var mask = "";

                foreach (var line in lines)
                {
                    if (line.StartsWith("mask"))
                    {
                        mask = line.Split("=")[1].Trim();
                        continue;
                    }

                    var index = long.Parse(line.Split(" = ")[0][4..^1]);
                    var right = long.Parse(line.Split(" = ")[1]);

                    var orMask = Convert.ToInt64(mask.Replace("X", "0"), 2);
                    var andMask = Convert.ToInt64(mask.Replace("X", "1"), 2);

                    memory[index] = (right | orMask) & andMask;
                }

                Assert.That(memory.Values.Sum(), Is.EqualTo(8471403462063));
            }

            [Test]
            public void Day14_Part2()
            {
                var lines = File.ReadAllLines("Day14.txt");

                var memory = new Dictionary<long, long>();
                var mask = "";

                foreach (var line in lines)
                {
                    if (line.StartsWith("mask"))
                    {
                        mask = line.Split("=")[1].Trim();
                        continue;
                    }

                    var index = long.Parse(line.Split(" = ")[0][4..^1]);
                    var right = long.Parse(line.Split(" = ")[1]);

                    var newMask = mask.ToCharArray();
                    var indexMask = Convert.ToString(index, 2).PadLeft(36, '0');

                    for (var i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '0') newMask[i] = indexMask[i];
                        if (mask[i] == '1') newMask[i] = '1';
                    }

                    var n = newMask.Count(x => x == 'X');
                    var indexes = newMask.Select((x, i) => (x, i)).Where(x => x.x == 'X').Select(x => x.i).ToList();

                    for (var i = 0; i < Math.Pow(2, n); i++)
                    {
                        var target = newMask.ToArray();

                        var p = Convert.ToString(i, 2).PadLeft(n, '0');

                        for (var j = 0; j < p.Length; j++)
                        {
                            if (p[j] == '0') target[indexes[j]] = '0';
                            else target[indexes[j]] = '1';
                        }

                        var address = Convert.ToInt64(new string(target), 2);

                        memory[address] = right;
                    }
                }

                Assert.That(memory.Values.Sum(), Is.EqualTo(2667858637669));
            }
        }

        public class Day15
        {
            [TestCase("0,3,6", 436)]
            [TestCase("1,3,2", 1)]
            [TestCase("2,1,3", 10)]
            [TestCase("1,2,3", 27)]
            [TestCase("2,3,1", 78)]
            [TestCase("3,2,1", 438)]
            [TestCase("3,1,2", 1836)]
            [TestCase("19,0,5,1,10,13", 1015)]
            public void Day15_Part1(string input, int expected)
            {
                var nums = input.Split(',').Select(int.Parse).ToList();

                for (var i = nums.Count; i < 2020; i++)
                {
                    var last = nums.Last();
                    var lastIndex = nums.SkipLast(1).ToList().LastIndexOf(last);

                    if (lastIndex == -1)
                    {
                        nums.Add(0);
                        continue;
                    }

                    nums.Add(i - lastIndex - 1);
                }


                Assert.That(nums.Last(), Is.EqualTo(expected));
            }

            [TestCase("0,3,6", 175594)]
            [TestCase("1,3,2", 2578)]
            [TestCase("2,1,3", 3544142)]
            [TestCase("1,2,3", 261214)]
            [TestCase("2,3,1", 6895259)]
            [TestCase("3,2,1", 18)]
            [TestCase("3,1,2", 362)]
            [TestCase("19,0,5,1,10,13", 201)]
            public void Day15_Part2(string input, int expected)
            {
                var nums = input.Split(',').Select(int.Parse).ToList();
                var lastIndex = new Dictionary<int, int>();

                for (var i = 0; i < nums.Count; i++)
                {
                    lastIndex[nums[i]] = i;
                }

                var value = 0;
                var lastValue = nums.Last();

                for (var i = nums.Count; i < 30000000; i++)
                {
                    lastIndex[lastValue] = i - 1;
                    lastValue = value;

                    if (lastIndex.ContainsKey(value))
                    {
                        value = i - lastIndex[value];
                    }
                    else
                    {
                        value = 0;
                    }
                }


                Assert.That(lastValue, Is.EqualTo(expected));
            }
        }

        public class Day16
        {
            [Test]
            public void Day16_Part1()
            {
                var input = File.ReadAllText("Day16.txt");
                var parts = input.Split("\r\n\r\n");
                var classesParts = parts[0];
                var nearbyTicketsPart = parts[2];
                var ranges = new List<(int, int)>();
                foreach (var c in classesParts.Split("\r\n"))
                {
                    parts = c.Split(new[] {": ", " or "}, StringSplitOptions.RemoveEmptyEntries);
                    ranges.Add((int.Parse(parts[1].Split("-")[0]), int.Parse(parts[1].Split("-")[1])));
                    ranges.Add((int.Parse(parts[2].Split("-")[0]), int.Parse(parts[2].Split("-")[1])));
                }

                var tickets = nearbyTicketsPart.Split("\r\n").Skip(1).ToList();
                var result = 0;
                foreach (var ticket in tickets)
                {
                    var nums = ticket.Split(",").Select(int.Parse);
                    foreach (var num in nums)
                    {
                        if (!InRange(num, ranges))
                            result += num;
                    }
                }

                Assert.That(result, Is.EqualTo(27850));
            }

            [Test]
            public void Day16_Part2()
            {
                var input = File.ReadAllText("Day16.txt");
                var parts = input.Split("\r\n\r\n");
                var classesParts = parts[0];
                var myTicket = parts[1].Split("\r\n")[1].Split(",").Select(int.Parse).ToList();
                var nearbyTicketsPart = parts[2];
                var fields = new List<Field>();
                foreach (var c in classesParts.Split("\r\n"))
                {
                    parts = c.Split(new[] {": ", " or "}, StringSplitOptions.RemoveEmptyEntries);
                    var field = new Field()
                    {
                        Name = parts[0],
                        Range1 = (int.Parse(parts[1].Split("-")[0]), int.Parse(parts[1].Split("-")[1])),
                        Range2 = (int.Parse(parts[2].Split("-")[0]), int.Parse(parts[2].Split("-")[1]))
                    };
                    fields.Add(field);
                }

                var validTickets = new List<List<int>>();
                var tickets = nearbyTicketsPart.Split("\r\n").Skip(1).ToList();
                var ranges = fields.Select(x => x.Range1).Union(fields.Select(x => x.Range2)).ToList();

                foreach (var ticket in tickets)
                {
                    var nums = ticket.Split(",").Select(int.Parse).ToList();

                    if (nums.All(num => InRange(num, ranges)))
                    {
                        validTickets.Add(nums);
                    }
                }

                var fieldMap = new Dictionary<int, List<int>>();

                for (var i = 0; i < fields.Count; i++)
                {
                    fieldMap[i] = new List<int>();

                    for (var j = 0; j < fields.Count; j++)
                    {
                        if (validTickets.Select(x => x[j]).All(x => fields[i].Valid(x)))
                        {
                            fieldMap[i].Add(j);
                        }
                    }
                }

                var finalMap = new Dictionary<int, int>();

                for (var i = 0; i < fields.Count; i++)
                {
                    var fm = fieldMap.OrderBy(x => x.Value.Count).First();

                    var next = fm.Value.First();
                    finalMap[fm.Key] = next;

                    fieldMap.Remove(fm.Key);

                    foreach (var f in fieldMap)
                    {
                        f.Value.Remove(next);
                    }
                }

                var departureIndexes = new List<int>();

                for (var i = 0; i < fields.Count; i++)
                {
                    if (fields[i].Name.StartsWith("departure"))
                    {
                        departureIndexes.Add(finalMap[i]);
                    }
                }

                var result =
                    departureIndexes.Aggregate(1L, (current, departureIndex) => current * myTicket[departureIndex]);

                Assert.That(result, Is.EqualTo(491924517533));
            }

            private class Field
            {
                public string Name;
                public (int, int) Range1;
                public (int, int) Range2;

                public bool Valid(int n)
                {
                    return (n >= Range1.Item1 && n <= Range1.Item2) ||
                           (n >= Range2.Item1 && n <= Range2.Item2);
                }
            }

            private bool InRange(int n, IList<(int, int)> ranges)
            {
                return ranges.Any(range => n >= range.Item1 && n <= range.Item2);
            }
        }
    }
}