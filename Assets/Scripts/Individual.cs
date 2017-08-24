using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Individual
{
    public static int DefaultGeneLength = 1;
    private Color32[] mGenes = new Color32[DefaultGeneLength];
    private int mFitness = 0;

    public void GenerateIndividual()
    {
        for (int i = 0; i < GetSize(); i++)
        {
            var gene = FitnessCalculator.Solution[Random.Range(0, FitnessCalculator.Solution.Length - 1)];
            mGenes[i] = gene;
        }
    }


    public static void SetDefaultGeneLength(int length)
    {
        DefaultGeneLength = length;
    }

    public Color32 GetGene(int index)
    {
        return mGenes[index];
    }

    public void SetGene(int index, Color32 value)
    {
        mGenes[index] = value;
        mFitness = 0;
    }

    public int GetSize()
    {
        return mGenes.Length;
    }

    public int GetFitness()
    {
        if (mFitness == 0)
        {
            mFitness = FitnessCalculator.CalculateFitness(this);
        }
        return mFitness;
    }

    public override string ToString()
    {
        string geneString = String.Empty;
        for (int i = 0; i < GetSize(); i++)
        {
            geneString += GetGene(i) + " ";
        }
        return geneString;
    }

    public Color32[] GetGenes()
    {
        return mGenes;
    }
}
