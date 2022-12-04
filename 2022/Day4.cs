using NUnit.Framework;

namespace AdventOfCode2022;

[TestFixture]
public class Day4
{
    private IEnumerable<(Assignment First, Assignment Second)> assignments;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        assignments = File.ReadAllLines("Input.txt")
                          .Select(pair => pair.Split(","))
                          .Select(x => (Assignment.Parse(x[0]), Assignment.Parse(x[1])));
    }

    [Test]
    public void Part1()
    {
        var fullyContainedAssignments = assignments.Where(a => a.First.FullyContains(a.Second) || a.Second.FullyContains(a.First));

        Assert.That(fullyContainedAssignments.Count(), Is.EqualTo(518));
    }

    [Test]
    public void Part2()
    {
        var overlappedAssignments = assignments.Where(a => !a.First.Overlap(a.Second));

        Assert.That(overlappedAssignments.Count(), Is.EqualTo(909));
    }

    private record Assignment(int Start, int End)
    {
        public bool Overlap(Assignment a) => End < a.Start || Start > a.End;
        public bool FullyContains(Assignment a) => Start <= a.Start && End >= a.End;
        public static Assignment Parse(string a) => new(int.Parse(a.Split("-")[0]), int.Parse(a.Split("-")[1]));
    }
}