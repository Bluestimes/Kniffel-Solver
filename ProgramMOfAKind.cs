using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateMOfAKind(int m, bool debug = false)
    {
        PrintDivider($"{m} of a kind");

        double maxPossibleScore = m * 6;

        int bestN = 0;
        double bestScore = 0;
        double bestProbability = 0;
        double bestNEval = 0;
        for (int n = 1; n <= 6; n++)
        {
            int Ns = CountN(n);
            double statNCount = Ns;
            double add = 0;

            double rest = dice.Length - m;
            if (statNCount < m)
            {
                // add statistical chance of getting more n
                rest = dice.Length - Ns;
                add = rest * (1 - MathF.Pow(5f / 6, restThrows));
            }

            // get probability of succeeding threshold of 3
            int NsNeeded = Math.Max(m - Ns, 0);

            double successProbability = MOfAKindSimulateProbability((int)rest, restThrows, NsNeeded, 1_000_000);

            string msg = debug
                ? $"\nNs needed: {NsNeeded}\tProbability of getting that: {100 * successProbability}\n"
                : "";

            statNCount += add;
            statNCount = Math.Min(statNCount, m);

            double totalScore = statNCount * n;

            double eval = totalScore / maxPossibleScore * (successProbability * 0.75 + 0.25) * 100;

            WriteLine(
                $"{$"{m} of a kind - {GetNName(n)}: {Math.Round(totalScore, 2)} / {maxPossibleScore} points",-tab}Evaluation: {Math.Round(eval, 2)}{msg}");

            if (eval > bestNEval)
            {
                bestN = n;
                bestScore = totalScore;
                bestProbability = successProbability;
                bestNEval = eval;
            }
        }

        WriteLine(
            $"\n{$"Best eval for {m} of a kind: Dice {bestN}",-tab}Total score: {Math.Round(bestScore, 2)} (0 | {bestN * m}) / {maxPossibleScore} points\n{"",-tab}Probability: {Math.Round(100 * bestProbability, 2)}%\n");
    }

    private static int MOfAKindSimulateDiceThrow(int numDice)
    {
        int countSixes = 0;
        for (int i = 0; i < numDice; i++)
        {
            if (random.Next(1, 7) == 6)
                countSixes++;
        }

        return countSixes;
    }

    private static double MOfAKindSimulateProbability(int X, int Y, int Z, int numSimulations)
    {
        if (Z <= 0) return 1; // Base case: Z = 0

        if (Y <= 0 || X <= 0) return 0; // Base case: not enough dice or throws

        int successCount = 0;
        for (int i = 0; i < numSimulations; i++)
        {
            int remainingDice = X;
            for (int j = 0; j < Y; j++)
            {
                int sixes = MOfAKindSimulateDiceThrow(remainingDice);
                remainingDice -= sixes;
                if (X - remainingDice >= Z)
                {
                    successCount++;
                    break;
                }
            }
        }

        return (double)successCount / numSimulations;
    }
}