using System;
using System.Collections;
using System.Collections.Generic;
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

    public static Population EvolvePopulation(Population pop)
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
            Individual indiv1 = SelectViaTournament(pop);
            Individual indiv2 = SelectViaTournament(pop);
            Individual newIndiv = Crossover(indiv1, indiv2);
            newPopulation.SaveIndividual(i, newIndiv);
        }

        for (int i = elitismOffset; i < newPopulation.GetSize(); i++)
        {
            Mutate(newPopulation.GetIndividual(i));
        }

        return newPopulation;
    }

    private static Individual Crossover(Individual indiv1, Individual indiv2)
    {
        Individual newSol = new Individual();
        for (int i = 0; i < indiv1.GetSize(); i++)
        {
            // Crossover
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
}