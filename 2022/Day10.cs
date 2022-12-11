
//var lines = File.ReadAllLines("Input.txt");

//var cycle = 0;
//var x = 1;
//var signalStrength = 0;
//var screen = new string('.', 40 * 6).ToCharArray();

//foreach (var line in lines)
//{
//    if (line == "noop")
//    {
//        Clock();
//    }
//    else
//    {
//        Clock();
//        Clock();

//        x += int.Parse(line.Split()[1]);
//    }
//}

//void Clock()
//{
//    cycle++;

//    if (Math.Abs((cycle-1) % 40 - x) <= 1)
//    {
//        screen[cycle-1] = '#';
//    }

//    if ((cycle - 20) % 40 == 0)
//    {
//        signalStrength += cycle * x;
//    }
//}

//// Part 1
//Console.WriteLine(signalStrength);

//// Part 2
//foreach (var line in screen.Chunk(40))
//{
//    Console.WriteLine(new string(line));
//}

