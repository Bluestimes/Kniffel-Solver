using static System.Console;

namespace Kniffel;

public partial class Program
{
    private static void EvaluateFullHouse()
    {
        PrintDivider("Full House");

        const int diceCount = 5;
        const int fullHouseScore = 25;

        List<List<int>> allCombinationsList = new();
        GenerateCombinations(diceCount, new List<int>(), allCombinationsList);

        int[][] allCombinations = FullHouseListToArray(allCombinationsList);

        int[][] fullHouses = FilterFullHouses(allCombinations);

        int[] bestFullHouse = GetBestClosestFullHouse(fullHouses);

        double p = FullHouseSimulateProbability(bestFullHouse, 500_000);

        double score = fullHouseScore * p;

        WriteLine(
            $"{$"Best target: {string.Join(" ", bestFullHouse)}",-tab}Total Score: {Math.Round(score, 2)} (0 | {fullHouseScore}) / {fullHouseScore} points\n{"",-tab}Probability: {Math.Round(p * 100, 2)}%\n");
    }

    private static double FullHouseSimulateProbability(int[] fullHouse, int numIterations)
    {
        Array.Sort(fullHouse);

        int successes = 0;

        for (int j = 0; j < numIterations; j++)
        {
            int[] current = dice;

            for (int t = 0; t < restThrows; t++)
            {
                (int[] thrownDice, int[] keptDice) = GetThrownDice(current, fullHouse);

                // WriteLine($"{string.Join(" ", current)}  -  {string.Join(" ", fullHouse)}");
                // WriteLine($"Thrown: {string.Join(" ", thrownDice)}\t\tKept: {string.Join(" ", keptDice)}\n");

                // throw dice
                for (int i = 0; i < thrownDice.Length; i++)
                {
                    thrownDice[i] = random.Next(1, 7);
                }

                current = thrownDice.Concat(keptDice).ToArray();
                Array.Sort(current);

                if (current.SequenceEqual(fullHouse) || IsFullHouse(current))
                {
                    successes++;
                    break;
                }
            }
            // WriteLine();
        }

        return (double)successes / numIterations;
    }

    private static int[] GetBestClosestFullHouse(int[][] fullHouses)
    {
        int[][] closestFullHouses = FindClosestArrays(dice, fullHouses);

        // for (int i = 0; i < closestFullHouses.Length; i++)
        // {
        //     Console.WriteLine(string.Join(" ", closestFullHouses[i]));
        // }

        int bestIndex = -1;
        int highestSum = 0;
        for (int i = 0; i < closestFullHouses.Length; i++)
        {
            // get sum
            int sum = 0;
            for (int j = 0; j < closestFullHouses[i].Length; j++)
            {
                sum += closestFullHouses[i][j];
            }

            if (sum > highestSum)
            {
                highestSum = sum;
                bestIndex = i;
            }
        }

        return closestFullHouses[bestIndex];
    }

    private static int CalculateArraySimilarity(int[] arr1, int[] arr2)
    {
        List<int> arr2List = arr2.ToList();
        int similarity = 0;

        foreach (int n in arr1)
        {
            if (arr2List.Contains(n))
            {
                arr2List.Remove(n);
                similarity++;
            }
        }

        // Console.WriteLine($"{string.Join(" ", arr1)}  -  {string.Join(" ", arr2)}  |  {similarity}");

        return similarity;
    }

    private static int[][] FindClosestArrays(int[] target, int[][] arrays)
    {
        List<int> closestIndices = new();
        int closestSimilarity = 0;

        for (int i = 0; i < arrays.Length; i++)
        {
            int similarity = CalculateArraySimilarity(target, arrays[i]);

            if (similarity == closestSimilarity)
                closestIndices.Add(i);
            else if (similarity > closestSimilarity)
            {
                closestSimilarity = similarity;
                closestIndices = new() { i };
            }
        }

        int[][] result = new int[closestIndices.Count][];
        for (int i = 0; i < closestIndices.Count; i++)
        {
            result[i] = arrays[closestIndices[i]];
        }

        return result;
    }

    private static int[][] RemoveDuplicates(int[][] arrays)
    {
        List<int[]> result = new();
        foreach (int[] a in arrays)
        {
            // check if contains
            bool contains = result.Select(el => !a.Where((t, j) => t != el[j]).Any()).Any(isCopy => isCopy);

            if (!contains) result.Add(a);
        }

        return result.ToArray();
    }

    private static int[][] FilterFullHouses(int[][] allCombinations)
    {
        foreach (int[] c in allCombinations)
        {
            Array.Sort(c);
        }

        allCombinations = RemoveDuplicates(allCombinations);

        // collect full houses
        int[][] fullHouses = allCombinations.Where(IsFullHouse).ToArray();


        // fullHouses = RemoveDuplicates(fullHouses);

        // foreach (int[] fh in fullHouses)
        // {
        //     Console.WriteLine(string.Join(" ", fh));
        // }

        return fullHouses;
    }

    private static bool IsFullHouse(int[] combination)
    {
        if (combination.Length != 5) throw new ArgumentException("Dice combination must have exactly 5 elements.");

        var groupedDice = combination.GroupBy(die => die);

        return groupedDice.Count() == 2 && groupedDice.All(group => group.Count() == 2 || group.Count() == 3);
    }

    private static int[][] FullHouseListToArray(List<List<int>> list)
    {
        int[][] res = new int[list.Count][];
        for (int i = 0; i < list.Count; i++)
        {
            List<int> combination = list[i];
            int[] combinationArr = combination.ToArray();
            res[i] = combinationArr;
        }

        return res;
    }

    private static void GenerateCombinations(int diceCount, List<int> currentCombination,
        List<List<int>> allCombinations)
    {
        if (diceCount == 0)
        {
            allCombinations.Add(new List<int>(currentCombination));
            return;
        }

        for (int i = 1; i <= 6; i++)
        {
            currentCombination.Add(i);
            GenerateCombinations(diceCount - 1, currentCombination, allCombinations);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }
}