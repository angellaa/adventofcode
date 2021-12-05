using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public class Day4
    {
        List<string> input;
        List<int> numbers;
        List<List<List<int>>> boards = new();
        int n;

        [SetUp]
        public void SetUp()
        {
            input = File.ReadAllLines("Day4.txt").ToList();

            numbers = input[0].Split(",").Select(x => int.Parse(x)).ToList();
            n = (input.Count - 1) / 6;            

            for (int i = 0; i < n; i++)
            {
                var board = new List<List<int>>();

                for (int j = 0; j < 5; j++)
                {
                    board.Add(input[2 + 6 * i + j].Split().Where(x => x.Trim() != "").Select(x => int.Parse(x)).ToList());
                }

                boards.Add(board);
            }
        }

        [Test]
        public void Part1()
        {
            Assert.That(Bingo(), Is.EqualTo(50008));
        }


        [Test]
        public void Part2()
        {
            Assert.That(Bingo2(), Is.EqualTo(17408));
        }

        private int Bingo()
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
                            return board.SelectMany(x => x).Where(x => x != -1).Sum() * number;                }
            }

            return 0;
        }

        private int Bingo2()
        {
            var result = 0;

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    for (var i = 0; i < 5; i++)
                        for (var j = 0; j < 5; j++)
                            if (board[i][j] == number)
                                board[i][j] = -1;
                }

                var winningBoards = new List<List<List<int>>>();

                foreach (var board in boards)
                {
                    for (var i = 0; i < 5; i++)
                        if (board[i].Sum() == -5 || board.Select(x => x[i]).Sum() == -5)
                        {
                            result = board.SelectMany(x => x).Where(x => x != -1).Sum() * number;
                            winningBoards.Add(board);
                        }
                }

                foreach (var board in winningBoards)
                {
                    boards.Remove(board);
                }
            }

            return result;
        }
    }
}