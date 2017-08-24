using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{

   private Individual[] mIndividual;

    public Population(int populationSize, bool initialise)
    {
        mIndividual = new Individual[populationSize];
        if (initialise)
        {
            for (int i = 0; i < GetSize(); i++)
            {
                Individual newIndividual = new Individual();
                newIndividual.GenerateIndividual();
                SaveIndividual(i, newIndividual);
            }
        }
    }

    public Individual GetIndividual(int index)
    {
        return mIndividual[index];
    }

    public Individual GetFittest()
    {
        Individual fittest = mIndividual[0];
        for (int i = 0; i < GetSize(); i++)
        {
            if (fittest.GetFitness() <= GetIndividual(i).GetFitness())
            {
                fittest = GetIndividual(i);
            }
        }
        return fittest;
    }

    public int GetSize()
    {
        return mIndividual.Length;
    }

    public void SaveIndividual(int index, Individual indiv)
    {
        mIndividual[index] = indiv;
    }
}