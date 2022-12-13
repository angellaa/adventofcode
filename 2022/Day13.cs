using System.Text.RegularExpressions;
using AdventOfCode2022;

var pairs = File.ReadAllLines("Input.txt").ChunkBy(x => x == "").ToList();

var result = 0;
var index = 0;

foreach (var pair in pairs)
{
    index++;
    var left = pair.First();
    var right = pair.Last();

    if (IsInOrder(left, right) == true)
    {
        Console.WriteLine($"Right with index {index}");
        result += index;
    }

    Console.WriteLine();
}

Console.WriteLine(result);

bool? IsInOrder(string left, string right)
{
    Console.WriteLine($"Compare {left} vs {right}");

    if (TryParseValue(left, out var leftNumber) && TryParseValue(right, out var rightNumber))
    {
        if (leftNumber == rightNumber) return null;

        return leftNumber < rightNumber;
    }
    
    left = WrapAllIntegers(left);
    right = WrapAllIntegers(right);

    var leftList = Unwrap(left);
    var rightList = Unwrap(right);

    for (var i = 0; i < Math.Min(leftList.Count, rightList.Count); i++)
    {
        var inOrder = IsInOrder(leftList[i], rightList[i]);

        if (inOrder.HasValue) 
        {
            return inOrder;
        }
    }

    if (leftList.Count < rightList.Count)
    {
        return true;
    }

    if (rightList.Count < leftList.Count)
    {
        return false;
    }

    return null;
}

bool TryParseValue(string input, out int value)
{
    value = -1;

    if (int.TryParse(input, out var number))
    {
        value = number;
        return true;
    }

    var m = Regex.Match(input, @"^\[(\d+)\]$");

    if (m.Success)
    {
        value = int.Parse(m.Groups[1].Value);
        return true;
    }

    return false;
}

List<string> Unwrap(string packet)
{
    var result = packet;

    if (packet == "[]") return new List<string>();

    if (int.TryParse(packet, out _)) return new List<string> { packet };

    if (!packet.Contains(',')) return new List<string>() { result[1..^1] };

    var m = Regex.Match(result, @"^\[([\d+|,]+)\]$");

    if (m.Success)
    {
        return m.Groups[1].Value.Split(",").ToList();
    }

    var unwrappedList = new List<string>();

    var startIndex = 0;
    var count = 0;
    result = result[1..^1];

    for (var i = 0; i < result.Length; i++)
    {
        if (result[i] == '[') count++;
        if (result[i] == ']') count--;
        if (count == 0 && result[i] == ',')
        {
            unwrappedList.Add(result[startIndex..i]);
            startIndex = i + 1;
        }
    }

    if (count == 0)
    {
        unwrappedList.Add(result[startIndex..]);
    }

    return unwrappedList.Count == 0 ? Unwrap(result[1..^1]) : unwrappedList;
}

string WrapAllIntegers(string packet)
{
    var result = packet;

    for (var i = 0; i < 3; i++)
    {
        result = Regex.Replace(result, @"\[(\d+),", m => $"[[{m.Groups[1].Value}],");
        result = Regex.Replace(result, @",(\d+)\]", m => $",[{m.Groups[1].Value}]]");
        result = Regex.Replace(result, @",(\d+),", m => $",[{m.Groups[1].Value}],");
    }

    return result;
}
