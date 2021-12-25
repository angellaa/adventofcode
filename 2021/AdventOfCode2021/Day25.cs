using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day25
{
    List<List<char>> input;
    int rows;
    int cols;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day25.txt").Select(x => x.ToList()).ToList();
        rows = input.Count;
        cols = input[0].Count;
    }

    [Test]
    public void Part1()
    {
        bool equal;
        int steps = 0;
                do
                {

            var original = input.Select(x => x.ToList()).ToList();
            equal = true;

            MoveRight(input);
            MoveDown(input);
            for (int r = 0; r < rows; r++)
            {
                if (!original[r].SequenceEqual(input[r]))
                {
                    equal = false;
                    break;
                }
            }
            steps++;


        } while (!equal);

        Print(input);

        Assert.That(steps, Is.EqualTo(0));
    }

    private void Print(List<List<char>> input)
    {
        var original = input.Select(x => x.ToList()).ToList();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Console.Write(input[r][c]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private void MoveRight(List<List<char>> input)
    {
        var original = input.Select(x => x.ToList()).ToList();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var nextCol = (c + 1) % cols;

                if (original[r][c] == '>' && original[r][nextCol] == '.')
                {
                    input[r][nextCol] = '>';
                    input[r][c] = '.';
                }
            }
        }
    }

    private void MoveDown(List<List<char>> input)
    {
        var original = input.Select(x => x.ToList()).ToList();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var nextRow = (r + 1) % rows;

                if (original[r][c] == 'v' && original[nextRow][c] == '.')
                {
                    input[nextRow][c] = 'v';
                    input[r][c] = '.';
                }
            }
        }
    }

    [Test]
    public void Part2()
    {
        Assert.That(-1, Is.EqualTo(0));
    }
}