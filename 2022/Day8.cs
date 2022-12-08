
var trees = File.ReadAllLines("Input.txt").Select(x => x.Select(c => c - '0').ToList()).ToList();
var n = trees.Count;
var visibles= n * 4 - 4;

for (var i = 1; i < n - 1; i++)
for (var j = 1; j < n - 1; j++)
{
    if (IsVisible(i, j)) visibles++;
}

var max = 0;

for (var i = 0; i < n; i++)
for (var j = 0; j < n; j++)
{
    var score = ScenicScore(i, j);

    if (score > max) max = score;
}

Console.WriteLine(visibles);
Console.WriteLine(max);

bool IsVisible(int i, int j)
{
    var leftVisible = true;
    var rightVisible = true;
    var topVisible = true;
    var bottomVisible = true;

    for (var k = 0; k < j; k++) if (trees[i][j] <= trees[i][k]) leftVisible = false;
    for (var k = n - 1; k > j; k--) if (trees[i][j] <= trees[i][k]) rightVisible = false;

    for (var k = 0; k < i; k++) if (trees[i][j] <= trees[k][j]) topVisible = false;
    for (var k = n - 1; k > i; k--) if (trees[i][j] <= trees[k][j]) bottomVisible = false;

    return leftVisible || rightVisible || topVisible || bottomVisible;
}

int ScenicScore(int i, int j)
{
    var leftScore = 0;
    var rightScore = 0;
    var topScore = 0;
    var bottomScore = 0;

    for (var k = j - 1; k >= 0; k--)
    {
        leftScore++;
        if (trees[i][j] <= trees[i][k]) break;
    }

    for (var k = j + 1; k < n; k++)
    {
        rightScore++;
        if (trees[i][j] <= trees[i][k]) break;
    }

    for (var k = i - 1; k >= 0; k--)
    {
        topScore++;
        if (trees[i][j] <= trees[k][j]) break;
    }

    for (var k = i + 1; k < n; k++)
    {
        bottomScore++;
        if (trees[i][j] <= trees[k][j]) break;
    }

    return leftScore * rightScore * topScore * bottomScore;
}



