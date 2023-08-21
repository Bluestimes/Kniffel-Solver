using System.ComponentModel;
using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static readonly Random random = new();

    private const int tab = 40;

    private static int[] dice;
    private static int restThrows;

    private static void Main()
    {
        RequestInput();

        EvaluateNs();
        EvaluateMOfAKind(3);
        EvaluateMOfAKind(4);
        EvaluateFullHouse();
        EvaluateSmallStraight();
        EvaluateLargeStraight();
        EvaluateKniffel();
        EvaluateChance();

        WriteLine("\n");
    }

    private static (int[] thrownDice, int[] keptDice) GetThrownDice(int[] current, int[] target)
    {
        List<int> targetList = target.ToList();
        List<int> currentList = current.ToList();

        List<int> keptDice = new();

        for (int i = targetList.Count - 1; i >= 0; i--)
        {
            if (!currentList.Contains(targetList[i])) continue;

            keptDice.Add(targetList[i]);

            currentList.Remove(targetList[i]);
            targetList.Remove(targetList[i]);
        }

        // Write($"Dice to be thrown: {string.Join(" ", diceList)}\n");
        // Write($"Dice to be kept: {string.Join(" ", keptDice)}");

        return (currentList.ToArray(), keptDice.ToArray());
    }

    private static (int[] thrownDice, int[] keptDice) GetThrownDiceBySum(int[] current)
    {
        Array.Sort(current);

        int threshold = Math.Max(4, current.Min());

        List<int> thrownDice = new();
        List<int> keptDice = new();

        foreach (int d in current)
        {
            if (d < threshold) thrownDice.Add(d);
            else keptDice.Add(d);
        }

        return (thrownDice.ToArray(), keptDice.ToArray());
    }
    
    private static int CountN(int n) => dice.Count(d => d == n);

    private static string GetNName(int n) => n switch
    {
        1 => "Aces",
        2 => "Twos",
        3 => "Threes",
        4 => "Fours",
        5 => "Fives",
        6 => "Sixes",
        _ => throw new Exception("")
    };

    private static bool ParseDiceInput(string input)
    {
        char[] chars = input.ToCharArray();

        dice = new int[5];

        if (chars.Length == 0)
        {
            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = random.Next(1, 7);
            }

            Array.Sort(dice);

            WriteLine($"Generated dice: {string.Join(" ", dice)}");

            return true;
        }

        if (chars.Length != 5)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("There must be exactly 5 inputs given");
            ResetColor();
            return false;
        }

        for (int i = 0; i < dice.Length; i++)
        {
            int num = chars[i] - '0';

            if (num is < 1 or > 6)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("The dice must be in the range of 1 to 6");
                ResetColor();
                return false;
            }

            dice[i] = num;
        }

        Array.Sort(dice);

        return true;
    }

    private static void RequestInput()
    {
        while (true)
        {
            Write("Enter dice: ");
            if (!ParseDiceInput(ReadLine()!)) continue;

            Write("\nEnter number of rest throws: ");
            restThrows = int.Parse(ReadLine()!);
            WriteLine();
            break;
        }
    }

    private static void PrintDivider(string msg)
    {
        const int length = 70;
        WriteLine($"{msg} {string.Concat(Enumerable.Repeat("-", length - 1 - msg.Length))}");
    }
}