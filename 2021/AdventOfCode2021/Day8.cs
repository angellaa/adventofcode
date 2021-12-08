using NUnit.Framework;
using System.Diagnostics;

namespace AdventOfCode2022
{
    [TestFixture]
    public class Day8
    {       
        [Test]
        public void Part1_Example()
        {
            var input = Load(Input1);

            var count = input.SelectMany(x => x.Display).Count(x => x.Count() is 2 or 3 or 4 or 7);

            Assert.That(count, Is.EqualTo(26));
        }

        [Test]
        public void Part1_Real()
        {
            var input = Load(File.ReadAllText("Day8.txt"));

            var count = input.SelectMany(x => x.Display).Count(x => x.Count() is 2 or 3 or 4 or 7);

            Assert.That(count, Is.EqualTo(534));
        }

        [Test]
        public void Part2_Example()
        {
            var input = Load(Input1);

            var result = Crack(input);

            Assert.That(result, Is.EqualTo(61229));
        }

        [Test]
        public void Part2_Real()
        {
            var input = Load(File.ReadAllText("Day8.txt"));

            var result = Crack(input);

            Assert.That(result, Is.EqualTo(1070188));
        }
        private static int Crack(List<(List<string> Digits, List<string> Display)> input)
        {
            int result = 0;

            foreach (var (digits, display) in input)
            {
                var one = Sorted(digits.First(x => x.Count() == 2));
                var seven = Sorted(digits.First(x => x.Count() == 3));
                var four = Sorted(digits.First(x => x.Count() == 4));
                var eight = Sorted(digits.First(x => x.Count() == 7));

                var digitsWithFiveSegments = digits.Where(x => x.Count() == 5).ToList();
                var digitsWithSixSegments = digits.Where(x => x.Count() == 6).ToList();

                digitsWithSixSegments.Add(one);

                var three = Sorted(Shared(digitsWithFiveSegments) + one);
                var nine = Sorted(new string(digitsWithSixSegments.Where(x => Except(new List<string>() { x, three }).Count() == 1).First()));

                var a = seven.Except(one).First();
                var b = Except(new List<string> { nine, three }).First();
                var f = Shared(digitsWithSixSegments).First();
                var c = one.First(x => x != f);
                var d = Except(new List<string> { four, $"{b}{c}{f}" }).First();
                var g = Except(new List<string> { nine, $"{a}{b}{c}{d}{f}" }).First();
                var e = Except(new List<string> { "abcdefg", $"{a}{b}{c}{d}{f}{g}" }).First();

                var zero = Sorted($"{a}{b}{c}{e}{f}{g}");
                var two = Sorted($"{a}{c}{d}{e}{g}");
                var five = Sorted($"{a}{b}{d}{f}{g}");
                var six = Sorted($"{a}{b}{d}{e}{f}{g}");

                var number = "";
                foreach (var digit in display.Select(x => Sorted(x)))
                {
                    if (digit == zero) number += "0";
                    else if (digit == one) number += "1";
                    else if (digit == two) number += "2";
                    else if (digit == three) number += "3";
                    else if (digit == four) number += "4";
                    else if (digit == five) number += "5";
                    else if (digit == six) number += "6";
                    else if (digit == seven) number += "7";
                    else if (digit == eight) number += "8";
                    else if (digit == nine) number += "9";
                    else Debugger.Break();
                }

                result += int.Parse(number);
            }

            return result;
        }

        static string Sorted(string s) => new(s.OrderBy(x => x).ToArray());

        static string Shared(List<string> digits)
        {
            return new string("abcdefg".Where(x => digits.All(c => c.Contains(x))).ToArray());
        }

        static string Except(List<string> digits)
        {
            var shared = Shared(digits);

            return new string("abcdefg".Where(x => !shared.Contains(x) && digits.Any(d => d.Contains(x))).ToArray());
        }

        static List<(List<string> Digits, List<string> Display)> Load(string input)
        {
            List<(List<string> dDgits, List<string> Display)> result = new();

            foreach (var line in input.Split("\n"))
            {
                var digits = line.Split("|")[0].Split().Where(x => x.Trim() != "").ToList();
                var display = line.Split("|")[1].Split().Where(x => x.Trim() != "").ToList();

                result.Add((digits, display));
            }

            return result;
        }

        static string Input1 = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
    }
}