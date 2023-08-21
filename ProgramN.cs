using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateNs()
    {
        PrintDivider("1, 2, 3");

        for (int i = 1; i <= 6; i++)
        {
            int Ns = CountN(i);

            int rest = dice.Length - Ns;
            double addStatNs = (1 - MathF.Pow(5f / 6, restThrows)) * rest;

            double totalScore = (Ns + addStatNs) * i;
            double maxPossibleScore = i * 5;

            double eval = totalScore / maxPossibleScore * 100;

            WriteLine($"{$"{GetNName(i)}: {Math.Round(totalScore, 2)} / {maxPossibleScore} points",-tab}Evaluation: {Math.Round(eval, 2)}");
        }

        WriteLine("\n");
    }
}