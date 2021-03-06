﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Population(Individual[] individuals)
    {
        mIndividual = individuals;
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

    public Population OrderByFitness(bool descending = false)
    {
        if (descending)
        {
            return new Population(mIndividual.OrderBy(x => x.GetFitness()).Reverse().ToArray());
        }
        else
        {
            return new Population(mIndividual.OrderBy(x => x.GetFitness()).ToArray());
        }
    }
}