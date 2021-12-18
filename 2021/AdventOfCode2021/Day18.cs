using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day18
{
    private List<SmallFishNumber> numbers;

    [SetUp]
    public void SetUp()
    {
        numbers = File.ReadAllLines("Day18.txt").Select(SmallFishNumber.Parse).ToList();
    }

    [Test]
    public void Part1()
    {
        var sum = Add(numbers[0], numbers[1]);
        Reduce(sum);

        for (var i = 2; i < numbers.Count; i++)
        {
            sum = Add(sum, numbers[i]);
            Reduce(sum);
        }

        Assert.That(sum.Magnitude, Is.EqualTo(3793));
    }

    [Test]
    public void Part2()
    {
        var max = int.MinValue;

        for (var i = 0; i < numbers.Count; i++)
        for (var j = 0; j < numbers.Count; j++)
        {
            if (i == j) continue;

            var first = SmallFishNumber.Parse(numbers[i].ToString());
            var second = SmallFishNumber.Parse(numbers[j].ToString());

            var sum = Add(first, second);
            Reduce(sum);

            max = Math.Max(max, sum.Magnitude);
        }

        Assert.That(max, Is.EqualTo(4695));
    }

    [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
    [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [TestCase("[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void Explode(string s, string expected)
    {
        var fish = SmallFishNumber.Parse(s);

        var exploded = Explode(fish);

        Assert.That(fish.ToString(), Is.EqualTo(expected));
        Assert.That(exploded, Is.True);
    }

    [TestCase("[[[[0,7],4],[[7,8],[0,13]]],[1,1]]")]
    [TestCase("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void ShouldNotExplode(string s)
    {
        var fish = SmallFishNumber.Parse(s);

        Assert.That(Explode(fish), Is.False);
    }

    [TestCase("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void ShouldNotSplit(string s)
    {
        var fish = SmallFishNumber.Parse(s);

        Assert.That(Split(fish), Is.False);
    }

    [TestCase("[[[[0,7],4],[15,[0,13]]],[1,1]]", "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]")]
    [TestCase("[[[[0,7],4],[[7,8],[0,13]]],[1,1]]", "[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]")]
    public void Split(string s, string expected)
    {
        var fish = SmallFishNumber.Parse(s);

        Split(fish);

        Assert.That(fish.ToString(), Is.EqualTo(expected));
    }

    [TestCase("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", "[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]")]
    public void Add(string f1, string f2, string expected)
    {
        var fish1 = SmallFishNumber.Parse(f1);
        var fish2 = SmallFishNumber.Parse(f2);

        var result = Add(fish1, fish2);

        Assert.That(result.ToString(), Is.EqualTo(expected));
    }

    [TestCase("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void Reduce(string s, string expected)
    {
        var fish = SmallFishNumber.Parse(s);

        Reduce(fish);

        Assert.That(fish.ToString(), Is.EqualTo(expected));
    }

    [TestCase("[[1,2],[[3,4],5]]", 143)]
    [TestCase("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
    [TestCase("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
    [TestCase("[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]", 4140)]
    [TestCase("[[[[7,8],[6,6]],[[6,0],[7,7]]],[[[7,8],[8,8]],[[7,9],[0,6]]]]", 3993)]
    public void Magnitude(string s, int expected)
    {
        var fish = SmallFishNumber.Parse(s);

        Assert.That(fish.Magnitude, Is.EqualTo(expected));
    }
    
    private static SmallFishNumber Add(Number fish1, Number fish2) => new(fish1, fish2);

    private static void Reduce(SmallFishNumber fish)
    {
        do
        {
            while (Explode(fish))
            {
                // Explode until possible
            }

        } while (Split(fish));

    }

    private static bool Split(SmallFishNumber fish)
    {
        var fishToSplit = FindFishToSplit(fish);

        if (fishToSplit == null)
        {
            return false;
        }

        var newFish = new SmallFishNumber(
            new RegularNumber(fishToSplit.Magnitude / 2), 
            new RegularNumber((int)Math.Ceiling(fishToSplit.Magnitude / 2.0)))
        {
            Parent = fishToSplit.Parent
        };

        if (fishToSplit.Parent.Left == fishToSplit)
        {
            fishToSplit.Parent.Left = newFish;
        }

        if (fishToSplit.Parent.Right == fishToSplit)
        {
            fishToSplit.Parent.Right = newFish;
        }

        return true;
    }

    private static Number FindFishToSplit(SmallFishNumber fish)
    {
        if (fish.Left is SmallFishNumber x)
        {
            var leftFishToSplit = FindFishToSplit(x);

            if (leftFishToSplit != null)
            {
                return leftFishToSplit;
            }
        }

        if (fish.Left is RegularNumber { Magnitude: >= 10 } leftFish)
        {
            return leftFish;
        }

        if (fish.Right is RegularNumber { Magnitude: >= 10 } rightFish)
        {
            return rightFish;
        }

        if (fish.Right is SmallFishNumber y)
        {
            var rightFishToSplit = FindFishToSplit(y);

            if (rightFishToSplit != null)
            {
                return rightFishToSplit;
            }
        }

        return null;
    }

    private static bool Explode(SmallFishNumber fish)
    {
        var explodingFish = FindFishToExplode(fish);

        if (explodingFish == null)
        {
            return false; // no explosion occurred
        }

        var parent = explodingFish.Parent;

        var firstLeftRegularFish = FindFirstLeftRegularFish(explodingFish);
        var firstRightRegularFish = FindFirstRightRegularFish(explodingFish);

        if (firstLeftRegularFish != null)
        {
            firstLeftRegularFish.Magnitude += ((RegularNumber)explodingFish.Left).Magnitude;
        }

        if (firstRightRegularFish != null)
        {
            firstRightRegularFish.Magnitude += ((RegularNumber)explodingFish.Right).Magnitude;
        }

        if (parent.Left == explodingFish)
        {
            parent.Left = new RegularNumber(0) { Parent = parent };
        }
        else if (parent.Right == explodingFish)
        {
            parent.Right = new RegularNumber(0) { Parent = parent };
        }

        return true;
    }

    private static RegularNumber FindFirstLeftRegularFish(Number number) =>
        number switch
        {
            RegularNumber r => r,
            SmallFishNumber { Parent: null } s => null,                      
            SmallFishNumber s when (s.Parent.Right == s) => FindRightRegularFish(s.Parent.Left),
            SmallFishNumber s => FindFirstLeftRegularFish(s.Parent),
            _ => null
        };

    private static RegularNumber FindFirstRightRegularFish(Number number) =>
        number switch
        {
            RegularNumber r => r,
            SmallFishNumber { Parent: null } s => null,
            SmallFishNumber s when (s.Parent.Left == s) => FindLeftRegularFish(s.Parent.Right),
            SmallFishNumber s => FindFirstRightRegularFish(s.Parent),
            _ => null
        };

    private static RegularNumber FindLeftRegularFish(Number number) =>
        number switch
        {
            RegularNumber r => r,
            SmallFishNumber s => FindLeftRegularFish(s.Left),
            _ => null
        };

    private static RegularNumber FindRightRegularFish(Number number) =>
        number switch
        {
            RegularNumber r => r,
            SmallFishNumber s => FindRightRegularFish(s.Right),
            _ => null
        };

    private static SmallFishNumber FindFishToExplode(SmallFishNumber f, int depth = 0)
    {
        if (depth == 4) return f;

        if (f.Left is SmallFishNumber lf)
        {
            var l1 = FindFishToExplode(lf, depth + 1);
            if (l1 != null) return l1;
        }

        if (f.Right is SmallFishNumber rf)
        {
            var r1 = FindFishToExplode(rf, depth + 1);
            if (r1 != null) return r1;
        }

        return null;
    }

    private abstract class Number
    {
        public virtual int Magnitude { get; set; }

        public SmallFishNumber Parent { get; set; } = null;
    }

    private class RegularNumber : Number
{
        public RegularNumber(int value)
        {
            Magnitude = value;
        }

        public override string ToString() => Magnitude.ToString();
}

    private class SmallFishNumber : Number
    {
        public SmallFishNumber(Number left, Number right)
        {
            Left = left;
            Right = right;
            Left.Parent = this;
            Right.Parent = this;
        }

        public Number Left { get; set; }
        public Number Right { get; set; }

        public override int Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

        public override string ToString() => $"[{Left},{Right}]";

        public static SmallFishNumber Parse(string s)
        {
            var stack = new Stack<SmallFishNumber>();

            do
            {
                var b = s.IndexOf("]");
                var a = s[..b].LastIndexOf("[");
                var pair = s.Substring(a + 1, b - a - 1);
                var leftString = pair.Split(",")[0];
                var rightString = pair.Split(",")[1];

                Number right = rightString == "_" ? stack.Pop() : new RegularNumber(int.Parse(rightString));
                Number left = leftString == "_" ? stack.Pop() : new RegularNumber(int.Parse(leftString));

                var parent = new SmallFishNumber(left, right);

                stack.Push(parent);

                s = s[..a] + "_" + s[(b + 1)..];
            } 
            while (s != "_");

            return stack.Pop();
        }
    }
}