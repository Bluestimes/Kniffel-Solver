using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateKniffel(bool debug = false)
    {
        PrintDivider("Kniffel");

        const int m = 5;

        const double maxPossibleScore = 50;

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

            double totalScore = statNCount * 10;

            double eval = 100 * successProbability;

            WriteLine(
                $"{$"Kniffel - {GetNName(n)}: {Math.Round(totalScore, 2)} / {maxPossibleScore} points",-tab}Evaluation: {Math.Round(eval, 2)}{msg}");

            if (eval > bestNEval)
            {
                bestN = n;
                bestScore = totalScore;
                bestProbability = successProbability;
                bestNEval = eval;
            }
        }

        WriteLine(
            $"\n{$"Best eval for Kniffel: Dice {bestN}",-tab}Total score: {Math.Round(bestScore, 2)} (0 | 50) / {maxPossibleScore} points\n{"",-tab}Probability: {Math.Round(100 * bestProbability, 2)}%\n");
    }
}