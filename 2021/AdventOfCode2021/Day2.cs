using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public class Day2
    {
        List<(string Name, int Value)> commands;

        [SetUp]
        public void SetUp()
        {
            commands = File.ReadAllLines("Day2.txt").Select(x => (x.Split()[0], int.Parse(x.Split()[1]))).ToList();
        }

        [Test]
        public void Part1()
        {
            var horizontal = 0;
            var depth = 0;

            foreach (var (Name, Value) in commands)
            {
                if (Name == "forward") horizontal += Value;
                if (Name == "down")    depth += Value;
                if (Name == "up")      depth -= Value;
            }

            Assert.That(horizontal * depth, Is.EqualTo(2272262));
        }

        [Test]
        public void Part2()
        {
            var horizontal = 0;
            var depth = 0;
            var aim = 0;

            foreach (var (Name, Value) in commands)
            {
                if (Name == "forward")
                {
                    horizontal += Value;
                    depth += aim * Value;
                }

                if (Name == "down") aim += Value;
                if (Name == "up")   aim -= Value;
            }

            Assert.That(horizontal * depth, Is.EqualTo(2134882034));
        }
    }
}