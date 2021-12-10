using NUnit.Framework;

namespace AdventOfCode2021
{
    [TestFixture]
    public class Day9
    {
        List<string> input;
        bool[,] visited;
        int n, m;

        [SetUp]
        public void SetUp()
        {
            input = File.ReadAllLines("Day9.txt").ToList();
            n = input.Count;
            m = input[0].Length;
        }

        [Test]
        public void Part1()
        {
            var result = 0;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    result += Risk(i, j);

            Assert.That(result, Is.EqualTo(478));
        }

        private int Risk(int i, int j)
        {
            var value = Value(i, j);

            if (value < Value(i-1, j) &&
                value < Value(i+1, j) &&
                value < Value(i, j-1) &&
                value < Value(i, j+1))
            {
                return 1 + value;
            }

            return 0;
        }

        private int Value(int i, int j)
        {
            if (i < 0 || i >= n || j < 0 || j >= m) return int.MaxValue;

            return input[i][j] - '0';
        }

        [Test]
        public void Part2()
        {
            long result = 1;
            var basins = new List<long>();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    basins.Add(Size(i, j));

            var largest = basins.OrderByDescending(x => x).Take(3);

            foreach (var large in largest) result *= large;
            
            Assert.That(result, Is.EqualTo(1327014));
        }

        private int Size(int i, int j)
        {
            var value = Value(i, j);

            if (value < Value(i - 1, j) &&
                value < Value(i + 1, j) &&
                value < Value(i, j - 1) &&
                value < Value(i, j + 1))
            {
                visited = new bool[n, m];
                return Basin(i, j);
            }

            return 1;
        }

        private bool Visited(int i, int j)
        {
            if (i < 0 || i >= n || j < 0 || j >= m) return true;

            return visited[i,j];
        }

        private int Basin(int i, int j)
        {
            var value = Value(i, j);
            if (value == 9 || Visited(i, j)) return 0;

            visited[i,j] = true;

            return 1 + Basin(i - 1, j) + Basin(i + 1, j) + Basin(i, j - 1) + Basin(i, j + 1);
        }
    }
}