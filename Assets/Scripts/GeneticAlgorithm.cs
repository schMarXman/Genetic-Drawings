﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// Genetic code based on: http://www.theprojectspot.com/tutorial-post/creating-a-genetic-algorithm-for-beginners/3
public class GeneticAlgorithm
{
    public static double UniformRate = 0.5;
    //private static double MutationRate = 0.015;
    public static double MutationRate = 0.001;
    public static int TournamentSize = 5;
    public static bool Elitism = true;
    public static int K = 2;

    public static Population EvolvePopulation(Population pop, int selection, int crossover)
    {
        Population newPopulation = new Population(pop.GetSize(), false);

        if (Elitism)
        {
            newPopulation.SaveIndividual(0, pop.GetFittest());
        }

        int elitismOffset;
        if (Elitism)
        {
            elitismOffset = 1;
        }
        else
        {
            elitismOffset = 0;
        }

        for (int i = elitismOffset; i < pop.GetSize(); i++)
        {
            Individual indiv1 = Select(pop, selection);
            Individual indiv2 = Select(pop, selection);
            Individual newIndiv = Crossover(indiv1, indiv2, crossover);
            newPopulation.SaveIndividual(i, newIndiv);
        }

        for (int i = elitismOffset; i < newPopulation.GetSize(); i++)
        {
            Mutate(newPopulation.GetIndividual(i));
        }

        return newPopulation;
    }

    private static Individual Crossover(Individual indiv1, Individual indiv2, int crossover)
    {
        switch (crossover)
        {
            default:
            case 0:
                return UniformCrossover(indiv1, indiv2);
            case 1:
                // OnePoint
                return OnepointCrossover(indiv1, indiv2);
            case 2:
                // KPoint
                return KPointCrossover(indiv1, indiv2, K);
            case 3:
                return RandomRespectfulCrossover(indiv1, indiv2);

        }
    }

    private static Individual UniformCrossover(Individual indiv1, Individual indiv2)
    {
        Individual newSol = new Individual();
        for (int i = 0; i < indiv1.GetSize(); i++)
        {
            // Crossover uniform
            if (UnityEngine.Random.value <= UniformRate)
            {
                newSol.SetGene(i, indiv1.GetGene(i));
            }
            else
            {
                newSol.SetGene(i, indiv2.GetGene(i));
            }
        }
        return newSol;
    }

    private static Individual OnepointCrossover(Individual indiv1, Individual indiv2)
    {
        Individual newSol = new Individual();

        int crossoverpoint = UnityEngine.Random.Range(0, indiv1.GetSize() - 1);


        for (int i = 0; i < indiv1.GetSize(); i++)
        {   // Crossover 1-Point            
            if (i > crossoverpoint) newSol.SetGene(i, indiv1.GetGene(i));
            else newSol.SetGene(i, indiv2.GetGene(i));
        }
        return newSol;
    }

    private static Individual TwopointCrossover(Individual indiv1, Individual indiv2)
    {
        Individual newSol = new Individual();


        int crossoverpoint1 = UnityEngine.Random.Range(0, indiv1.GetSize() - 1);
        int crossoverpoint2 = UnityEngine.Random.Range(0, indiv1.GetSize() - 1);


        for (int i = 0; i < indiv1.GetSize(); i++)
        {   // Crossover 2-Point
            if (crossoverpoint1 > crossoverpoint2)
            {
                int tmp = crossoverpoint1;
                crossoverpoint1 = crossoverpoint2;
                crossoverpoint2 = tmp;
            }
            if (i < crossoverpoint1) newSol.SetGene(i, indiv1.GetGene(i));
            else if (i > crossoverpoint1 && i < crossoverpoint2) newSol.SetGene(i, indiv2.GetGene(i));
            else if (i > crossoverpoint2) newSol.SetGene(i, indiv1.GetGene(i));
        }
        return newSol;
    }

    //private static Individual kpointCrossover(Individual indiv1, Individual indiv2, int k)
    //{
    //    int[] points = new int[k];
    //    Individual newSol = new Individual();
    //    for (int j = 0; j < k; j++)
    //    {
    //        points[j] = UnityEngine.Random.Range(0, indiv1.GetSize() - 1);
    //    }
    //    points = points.OrderBy(x => x).ToArray();

    //    for (int i = 0; i < indiv1.GetSize(); i++)
    //    {   // Crossover k-Point          
    //        for (int j = 0; j < k; j++)
    //        {
    //            if (j == k - 1)
    //            {
    //                if (i < points[j])
    //                {
    //                    if (i % 2 == 0) newSol.SetGene(i, indiv1.GetGene(i));
    //                    else newSol.SetGene(i, indiv2.GetGene(i));
    //                }
    //            }
    //            else if (i < points[j] && i < points[j + 1])
    //            {
    //                if (i % 2 == 0) newSol.SetGene(i, indiv1.GetGene(i));
    //                else newSol.SetGene(i, indiv2.GetGene(i));
    //            }
    //        }
    //    }
    //    return newSol;
    //}

    private static Individual KPointCrossover(Individual indiv1, Individual indiv2, int k)
    {
        Individual newIndiv = new Individual();

        List<int> crossoverPoints = new List<int>();

        for (int i = 0; i <= k; i++)
        {
            crossoverPoints.Add(Random.Range(0, indiv1.GetSize() - 1));
        }

        Individual currentlySelectedIndiv = indiv1;

        for (int i = 0; i < indiv1.GetSize(); i++)
        {
            newIndiv.SetGene(i, currentlySelectedIndiv.GetGene(i));

            if (crossoverPoints.Contains(i))
            {
                if (currentlySelectedIndiv == indiv1)
                {
                    currentlySelectedIndiv = indiv2;
                }
                else if (currentlySelectedIndiv == indiv2)
                {
                    currentlySelectedIndiv = indiv1;
                }
            }
        }

        return newIndiv;
    }

    private static Individual RandomRespectfulCrossover(Individual indiv1, Individual indiv2)
    {
        Individual child = new Individual();
        //Without creating similarity vector
        for (int i = 0; i < indiv1.GetSize(); i++)
        {
            Color32 gene1 = indiv1.GetGene(i);

            if (gene1.Equals(indiv2.GetGene(i)))
            {
                if (gene1.Equals(new Color32(255, 255, 255, 255)))
                {
                    child.SetGene(i, new Color32(255, 255, 255, 255));
                }
                else
                {
                    child.SetGene(i, new Color32(0, 0, 0, 255));
                }
            }
            else
            {
                child.SetGene(i, Random.value > 0.5f ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 255));
            }
        }

        return child;
    }

    private static void Mutate(Individual indiv)
    {
        for (int i = 0; i < indiv.GetSize(); i++)
        {
            if (UnityEngine.Random.value <= MutationRate)
            {
                var gene = FitnessCalculator.Solution[Random.Range(0, FitnessCalculator.Solution.Length - 1)];
                indiv.SetGene(i, gene);
            }
        }
    }

    private static Individual Select(Population pop, int selection)
    {
        switch (selection)
        {
            default:
            case 0:
                // tournament
                return SelectViaTournament(pop);
            case 1:
                // roulette
                return SelectViaRoulette(pop);
            case 2:
                // truncate
                return SelectViaTruncation(pop);
        }
    }

    private static Individual SelectViaTournament(Population pop)
    {
        Population tournament = new Population(TournamentSize, false);
        for (int i = 0; i < TournamentSize; i++)
        {
            //int randomId = (int)(UnityEngine.Random.value * pop.GetSize());
            int randomId = Random.Range(0, pop.GetSize());
            tournament.SaveIndividual(i, pop.GetIndividual(randomId));
        }
        Individual fittest = tournament.GetFittest();
        return fittest;
    }
    private static Individual SelectViaRoulette(Population pop)
    {
        int pickAt = Random.Range(0, GetFitnessSum(pop));
        int current = 0;
        for (int i = 0; i < pop.GetSize(); i++)
        {
            var indiv = pop.GetIndividual(i);
            current += indiv.GetFitness();
            if (current > pickAt)
            {
                return indiv;
            }
        }

        return null;
    }

    private static Individual SelectViaTruncation(Population pop)
    {
        Population sortedPop = pop.OrderByFitness();

        //string test = String.Empty;
        //for (int i = 0; i < sortedPop.GetSize(); i++)
        //{
        //    test += sortedPop.GetIndividual(i).GetFitness() + " ";
        //}
        //Debug.Log(test);

        int size = sortedPop.GetSize();

        // Selects randomly one of the fittest 30%
        return sortedPop.GetIndividual(Mathf.RoundToInt(Random.Range(size * 0.7f, size - 1)));
    }

    private static int GetFitnessSum(Population pop)
    {
        int fSum = 0;

        for (int i = 0; i < pop.GetSize(); i++)
        {
            fSum += pop.GetIndividual(i).GetFitness();
        }

        return fSum;
    }
}