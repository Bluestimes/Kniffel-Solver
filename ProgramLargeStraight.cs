using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateLargeStraight()
    {
        PrintDivider("Large straight");

        const int largeStraightScore = 40;

        int[][] largeStraights = new int[2][];
        largeStraights[0] = new[] { 1, 2, 3, 4, 5 };
        largeStraights[1] = new[] { 2, 3, 4, 5, 6 };

        int[] bestLargeStraight = GetBestClosestLargeStraight(largeStraights);

        double p = LargeStraightSimulateProbability(bestLargeStraight, 1_000_000);

        double score = p * largeStraightScore;

        WriteLine(
            $"{$"Best target: {string.Join(" ", bestLargeStraight)}",-tab}Total Score: {Math.Round(score, 2)} (0 | {largeStraightScore}) / {largeStraightScore} points\n{"",-tab}Probability: {Math.Round(p * 100, 2)}%\n");
    }

    private static int[] GetBestClosestLargeStraight(int[][] largeStraights)
    {
        int[][] closestLargeStraights = FindClosestArrays(dice, largeStraights);

        // for (int i = 0; i < closestFullHouses.Length; i++)
        // {
        //     Console.WriteLine(string.Join(" ", closestFullHouses[i]));
        // }

        int bestIndex = -1;
        int highestSum = 0;
        for (int i = 0; i < closestLargeStraights.Length; i++)
        {
            // get sum
            int sum = 0;
            for (int j = 0; j < closestLargeStraights[i].Length; j++)
            {
                sum += closestLargeStraights[i][j];
            }

            if (sum > highestSum)
            {
                highestSum = sum;
                bestIndex = i;
            }
        }

        return closestLargeStraights[bestIndex];
    }

    private static bool IsLargeStraight(int[] diceCombination)
    {
        for (int i = 0; i < diceCombination.Length - 1; i++)
        {
            if (diceCombination[i] != diceCombination[i + 1] - 1) return false;
        }

        return true;
    }

    private static double LargeStraightSimulateProbability(int[] straight, int numIterations)
    {
        int successes = 0;

        for (int j = 0; j < numIterations; j++)
        {
            int[] current = dice;

            for (int t = 0; t < restThrows; t++)
            {
                (int[] thrownDice, int[] keptDice) = GetThrownDice(current, straight);

                // WriteLine($"{string.Join(" ", current)}  -  {string.Join(" ", fullHouse)}");
                // WriteLine($"Thrown: {string.Join(" ", thrownDice)}\t\tKept: {string.Join(" ", keptDice)}\n");

                // throw dice
                for (int i = 0; i < thrownDice.Length; i++)
                {
                    thrownDice[i] = random.Next(1, 7);
                }

                current = thrownDice.Concat(keptDice).ToArray();
                Array.Sort(current);

                if (current.SequenceEqual(straight) || IsLargeStraight(current))
                {
                    successes++;
                    break;
                }
            }
            // WriteLine();
        }

        return (double)successes / numIterations;
    }
}