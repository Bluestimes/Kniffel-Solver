using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateChance()
    {
        PrintDivider("Chance");

        const int maxScore = 6 * 5;
        double score = ChanceSimulateScore(1_000_000);

        WriteLine($"{"", -tab}Total Score: {Math.Round(score, 2)} ({dice.Sum()}) / {maxScore} points\n");
    }

    private static double ChanceSimulateScore(int numIterations)
    {
        int sum = 0;
        for (int j = 0; j < numIterations; j++)
        {
            int[] current = dice;

            for (int t = 0; t < restThrows; t++)
            {
                (int[] thrownDice, int[] keptDice) = GetThrownDiceBySum(current);

                // WriteLine($"{string.Join(" ", current)}  -  {string.Join(" ", fullHouse)}");
                // WriteLine($"Thrown: {string.Join(" ", thrownDice)}\t\tKept: {string.Join(" ", keptDice)}\n");

                // throw dice
                for (int i = 0; i < thrownDice.Length; i++)
                {
                    thrownDice[i] = random.Next(1, 7);
                }

                current = thrownDice.Concat(keptDice).ToArray();
                Array.Sort(current);
            }

            // sum up
            sum += current.Sum();

            // WriteLine(string.Join(" ", current));
        }

        return (double)sum / numIterations;
    }
}