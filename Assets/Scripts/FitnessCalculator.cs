using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessCalculator
{
    public static Color32[] Solution = { };

    public static void SetSolution(Color32[] newSolution)
    {
        Solution = newSolution;
    }

    public static int CalculateFitness(Individual individual)
    {
        int fitness = 0;
        for (int i = 0; i < individual.GetSize() && i < Solution.Length; i++)
        {
            var solColor = Solution[i];
            var indColor = individual.GetGene(i);

            if (solColor.Equals(indColor))
            {
                fitness++;
            }
        }
        return fitness;
    }

    public static int GetMaxFitness()
    {
        int maxFitness = Solution.Length;
        return maxFitness;
    }
}