using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day4
{
    private List<int> numbers;
    private readonly List<List<List<int>>> boards = new();

    [SetUp]
    public void SetUp()
    {
        var input = File.ReadAllLines("Day4.txt").ToList();
        var numberOfBoards = (input.Count - 1) / 6;

        numbers = input[0].Split(",").Select(int.Parse).ToList();

        for (var i = 0; i < numberOfBoards; i++)
        {
            var board = new List<List<int>>();

            for (var row = 0; row < 5; row++)
            {
                board.Add(input[2 + 6 * i + row].Split().Where(x => x.Trim() != "").Select(int.Parse).ToList());
            }

            boards.Add(board);
        }
    }

    [Test]
    public void Part1()
    {
        Assert.That(Bingo(), Is.EqualTo(50008));

        int Bingo()
        {
            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    for (var i = 0; i < 5; i++)
                    for (var j = 0; j < 5; j++)
                        if (board[i][j] == number)
                            board[i][j] = -1;
                }

                foreach (var board in boards)
                {
                    for (var i = 0; i < 5; i++)
                        if (board[i].Sum() == -5 || board.Select(x => x[i]).Sum() == -5)
                            return board.SelectMany(x => x).Where(x => x != -1).Sum() * number;
                }
            }

            return 0;
        }
    }
    
    [Test]
    public void Part2()
    {
        var finalScore = 0;

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                for (var r = 0; r < 5; r++)
                for (var c = 0; c < 5; c++)
                    if (board[r][c] == number)
                        board[r][c] = -1;
            }

            var winningBoards = new List<List<List<int>>>();

            foreach (var board in boards)
            {
                for (var r = 0; r < 5; r++)
                    if (board[r].Sum() == -5 || board.Select(x => x[r]).Sum() == -5)
                    {
                        finalScore = board.SelectMany(x => x).Where(x => x != -1).Sum() * number;
                        winningBoards.Add(board);
                    }
            }

            foreach (var board in winningBoards)
            {
                boards.Remove(board);
            }
        }

        Assert.That(finalScore, Is.EqualTo(17408));
    }
}