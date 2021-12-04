using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public class Day3
    {
        List<string> binaryNumbers;
        int bits;
        int n;

        [SetUp]
        public void SetUp()
        {
            binaryNumbers = File.ReadAllLines("Day3.txt").ToList();
            n = binaryNumbers.Count;
            bits = binaryNumbers[0].Length;
        }

        [Test]
        public void Part1()
        {
            var gamma = new string(Enumerable.Range(0, bits)
                                  .Select(i =>
                                    binaryNumbers.Count(c => c[i] == '1') > (n / 2) ? '1' : '0'
                                  ).ToArray());

            var epsilon = new string(gamma.Select(x => x == '1' ? '0' : '1').ToArray());

            Assert.That(Convert.ToInt64(gamma, 2) * Convert.ToInt64(epsilon, 2), Is.EqualTo(3912944));
        }

        [Test]
        public void Part2()
        {
            var numbers = binaryNumbers.ToList();

            for (var i = 0; i < bits; i++)
            {
                var most = numbers.Count(c => c[i] == '1') >= numbers.Count(c => c[i] == '0') ? '1' : '0';
                numbers.RemoveAll(x => x[i] != most);
                if (numbers.Count == 1) break;
            }

            var oxygen = numbers.First();

            numbers = binaryNumbers.ToList();

            for (var i = 0; i < bits; i++)
            {
                var least = numbers.Count(c => c[i] == '1') < numbers.Count(c => c[i] == '0') ? '1' : '0';
                numbers.RemoveAll(x => x[i] != least);
                if (numbers.Count == 1) break;
            }

            var co2 = numbers.First();

            Assert.That(Convert.ToInt64(oxygen, 2) * Convert.ToInt64(co2, 2), Is.EqualTo(4996233));
        }
    }
}