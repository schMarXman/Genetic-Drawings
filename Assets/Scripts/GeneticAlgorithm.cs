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
                return null;
            case 2:
                // TwoPoint
                return null;
            case 3:
                // Shuffle
                return null;
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
        int crossoverpoint = int(random(indiv1.GetSize()));

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
        int crossoverpoint1 = int(random(indiv1.GetSize()));
        int crossoverpoint2 = int(random(indiv1.GetSize()));

        for (int i = 0; i < indiv1.GetSize(); i++)
        {   // Crossover 2-Point
            if(crossoverpoint1 > crossoverpoint2){
                int tmp = crossoverpoint1;
                crossoverpoint1 = crossoverpoint2;
                crossoverpoint2 = tmp;
            }            
            if (i < crossoverpoint1) newSol.SetGene(i, indiv1.GetGene(i));
                else if (i > crossoverpoint1 && i < crossoverpoint2) newSol.SetGene(i, indiv2.GetGene(i));
                else if(i>crossoverpoint2) newSol.SetGene(i, indiv1.GetGene(i));
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

    private static Individual Select(Population pop, int selection)
    {
        switch (selection)
        {
            default:
            case 0:
                // tournament
                return SelectViaTodurnament(pop);
            case 1:
                // 
                return null;
            case 2:
                //
                return null;
        }
    }

    private static Individual SelectViaTodurnament(Population pop)
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