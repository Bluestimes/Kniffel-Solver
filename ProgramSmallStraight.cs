using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateSmallStraight()
    {
        PrintDivider("Small straight");

        const int smallStraightScore = 30;

        int[][] smallStraights = new int[3][];
        smallStraights[0] = new[] { 1, 2, 3, 4 };
        smallStraights[1] = new[] { 2, 3, 4, 5 };
        smallStraights[2] = new[] { 3, 4, 5, 6 };

        int[] bestSmallStraight = GetBestClosestSmallStraight(smallStraights);

        double p = SmallStraightSimulateProbability(bestSmallStraight, 1_000_000);

        double score = p * smallStraightScore;

        WriteLine(
            $"{$"Best target: {string.Join(" ", bestSmallStraight)}",-tab}Total Score: {Math.Round(score, 2)} (0 | {smallStraightScore}) / {smallStraightScore} points\n{"",-tab}Probability: {Math.Round(p * 100, 2)}%\n");
    }

    private static int[] GetBestClosestSmallStraight(int[][] smallStraights)
    {
        int[][] closestSmallStraights = FindClosestArrays(dice, smallStraights);

        // for (int i = 0; i < closestFullHouses.Length; i++)
        // {
        //     Console.WriteLine(string.Join(" ", closestFullHouses[i]));
        // }

        int bestIndex = -1;
        int highestSum = 0;
        for (int i = 0; i < closestSmallStraights.Length; i++)
        {
            // get sum
            int sum = 0;
            for (int j = 0; j < closestSmallStraights[i].Length; j++)
            {
                sum += closestSmallStraights[i][j];
            }

            if (sum > highestSum)
            {
                highestSum = sum;
                bestIndex = i;
            }
        }

        return closestSmallStraights[bestIndex];
    }

    private static bool ContainsSmallStraight(int[] dice)
    {
        // Sort the dice array
        Array.Sort(dice);

        // Check for consecutive numbers
        int count = 1;
        for (int i = 1; i < dice.Length; i++)
        {
            if (dice[i] == dice[i - 1] + 1)
            {
                count++;
                if (count >= 4) // Found a small straight
                    return true;
            }
            else if (dice[i] != dice[i - 1]) // Reset count if non-consecutive number is encountered
                count = 1;
        }

        return false;
    }

    private static double SmallStraightSimulateProbability(int[] straight, int numIterations)
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

                if (ContainsSmallStraight(current))
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