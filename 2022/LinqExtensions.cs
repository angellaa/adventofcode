using System.Text;

namespace AdventOfCode2022;

public static class LinqExtensions
{
    public static Stack<char> ToStack(this string s)
    {
        var result = new Stack<char>();

        foreach (var a in s)
        {
            result.Push(a);
        }

        return result;
    }

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> list, Func<T, bool> predicate)
    {
        var chunk = new List<T>();

        foreach (var item in list)
        {
            if (predicate(item))
            {
                yield return chunk.ToList();
                chunk.Clear();
                continue;
            }

            chunk.Add(item);
        }

        yield return chunk;
    }
}