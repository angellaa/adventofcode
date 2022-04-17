using NUnit.Framework;

namespace AdventOfCode2021;

[TestFixture]
public class Day21
{
    private List<string> input;
    private int pos1;
    private int pos2;
    private int score1;
    private int score2;
    private int die;
    private int rolls;

    [SetUp]
    public void SetUp()
    {
        input = File.ReadAllLines("Day21.txt").ToList();
        pos1 = int.Parse(input[0].Split(":")[1]) - 1;
        pos2 = int.Parse(input[1].Split(":")[1]) - 1;
    }

    [Test]
    public void Part1()
    {
        while (score2 <= 1000)
        {
            pos1 = (pos1 + Roll()) % 10;
            score1 += (pos1 + 1);

            if (score1 >= 1000) break;

            pos2 = (pos2 + Roll()) % 10;
            score2 += (pos2 + 1);
        }

        Assert.That(rolls * Math.Min(score1, score2), Is.EqualTo(900099));
    }
    public int Roll()
    {
        return NextDice() + NextDice() + NextDice();

        int NextDice()
        {
            rolls++;
            die++;
            if (die > 100) die = 1;
            return die;
        }
    }
    
    [Test]
    public void Part2()
    {
        var state = new State(pos1, pos2, 0, 0, 0);
        var win = Win(state);

        Assert.That(Math.Max(win.Item1, win.Item2), Is.EqualTo(306719685234774));
    }

    private record State(int P1, int P2, int Score1, int Score2, int Turn)
    {
        public State Next(int moves) => Turn == 0 ? Player1Next(moves) : Player2Next(moves);

        private State Player1Next(int moves)
        {
            var nextP1 = (P1 + moves) % 10;

            return new State(nextP1, P2, Score1 + nextP1 + 1, Score2, 1);
        }

        private State Player2Next(int moves)
        {
            var nextP2 = (P2 + moves) % 10;

            return new State(P1, nextP2, Score1, Score2 + nextP2 + 1, 0);
        }
    }

    private readonly Dictionary<State, (long, long)> cachedWins = new();

    private (long, long) Win(State s)
    {
        if (cachedWins.ContainsKey(s)) return cachedWins[s];
        if (s.Score1 >= 21) return (1, 0);
        if (s.Score2 >= 21) return (0, 1);

        var states = new State[10];
        var wins = new (long Wins1, long Wins2)[10];

        for (var i = 3; i <= 9; i++)
        {
            states[i] = s.Next(i);
            wins[i] = Win(states[i]);
        }

        wins[4] = (wins[4].Wins1 * 3, wins[4].Wins2 * 3);
        wins[5] = (wins[5].Wins1 * 6, wins[5].Wins2 * 6);
        wins[6] = (wins[6].Wins1 * 7, wins[6].Wins2 * 7);
        wins[7] = (wins[7].Wins1 * 6, wins[7].Wins2 * 6);
        wins[8] = (wins[8].Wins1 * 3, wins[8].Wins2 * 3);

        var nextWins1 = wins.Sum(x => x.Wins1);
        var nextWins2 = wins.Sum(x => x.Wins2);

        cachedWins[s] = (nextWins1, nextWins2);

        return (nextWins1, nextWins2);
    }
}