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
            commands = File.ReadAllLines("Day2.txt")
                           .Select(x => x.Split())
                           .Select(x => (x[0], int.Parse(x[1])))
                           .ToList();
        }

        [Test]
        public void Part1()
        {
            var horizontalPosition = 0;
            var depth = 0;

            foreach (var (name, value) in commands)
            {
                if (name == "forward") horizontalPosition += value;
                if (name == "down")    depth += value;
                if (name == "up")      depth -= value;
            }

            Assert.That(horizontalPosition * depth, Is.EqualTo(2272262));
        }

        [Test]
        public void Part2()
        {
            var horizontalPosition = 0;
            var depth = 0;
            var aim = 0;

            foreach (var (name, value) in commands)
            {
                if (name == "forward")
                {
                    horizontalPosition += value;
                    depth += aim * value;
                }

                if (name == "down") aim += value;
                if (name == "up")   aim -= value;
            }

            Assert.That(horizontalPosition * depth, Is.EqualTo(2134882034));
        }
    }
}