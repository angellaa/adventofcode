﻿using NUnit.Framework;

namespace AdventOfCode2022
{
    [TestFixture]
    public class Day9
    {
        List<string> input;

        [SetUp]
        public void SetUp()
        {
            input = File.ReadAllLines("Day9.txt").ToList();
        }

        [Test]
        public void Part1()
        {
            Assert.That(-1, Is.EqualTo(0));
        }

        [Test]
        public void Part2()
        {
            Assert.That(-1, Is.EqualTo(0));
        }
    }
}