using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public InputField PopInput, UniRateInput, MutRateInput, TournamentSizeInput, GenStepInput, PopDisplayInput;

    public GameObject PopulationView;

    public Toggle ElitismToggle;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PopInput.text = string.Empty + Program.Instance.PopulationSize;
        PopInput.onEndEdit.AddListener(SetPopulationSize);

        UniRateInput.text = string.Empty + GeneticAlgorithm.UniformRate;
        UniRateInput.onEndEdit.AddListener(SetUniformRate);

        MutRateInput.text = string.Empty + GeneticAlgorithm.MutationRate;
        MutRateInput.onEndEdit.AddListener(SetMutationRate);

        TournamentSizeInput.text = string.Empty + GeneticAlgorithm.TournamentSize;
        TournamentSizeInput.onEndEdit.AddListener(SetTournamentSize);

        GenStepInput.text = string.Empty + Program.Instance.GenStepSize;
        GenStepInput.onEndEdit.AddListener(SetGenerationStepSize);

        ElitismToggle.isOn = GeneticAlgorithm.Elitism;
        ElitismToggle.onValueChanged.AddListener(SetElitism);

        PopDisplayInput.text = string.Empty + Program.Instance.PopulationDisplayAmount;
        PopDisplayInput.onValueChanged.AddListener(SetPopulationDisplayAmount);

        SwitchPopulationView();
    }

    public void SwitchPopulationView()
    {
        switch (PopulationView.activeSelf)
        {
            case true:
                PopulationView.SetActive(false);
                break;
            case false:
                PopulationView.SetActive(true);

                if (!Program.Instance.EvolutionRunning())
                {
                    Program.Instance.ShowPopulation(Program.Instance.PopulationDisplayAmount);
                }
                break;
        }
    }

    public void SetPopulationSize(int size)
    {
        if (size < 2)
        {
            size = 2;
        }

        PopInput.text = string.Empty + size;

        Program.Instance.PopulationSize = size;

        SetPopulationDisplayAmount(Mathf.Clamp(Program.Instance.PopulationDisplayAmount, 1, size));
    }

    public void SetUniformRate(double rate)
    {
        GeneticAlgorithm.UniformRate = rate;
    }

    public void SetMutationRate(double rate)
    {
        GeneticAlgorithm.MutationRate = rate;
    }

    public void SetTournamentSize(int size)
    {
        GeneticAlgorithm.TournamentSize = size;
    }

    public void SetElitism(bool value)
    {
        GeneticAlgorithm.Elitism = value;
    }

    public void SetGenerationStepSize(int size)
    {
        Program.Instance.GenStepSize = size;
    }

    public void SetPopulationSize(string size)
    {
        int val;
        if (int.TryParse(size, out val))
        {
            SetPopulationSize(val);
        }
    }

    public void SetUniformRate(string rate)
    {
        double val;
        if (double.TryParse(rate, out val))
        {
            SetUniformRate(val);
        }
    }

    public void SetMutationRate(string rate)
    {
        double val;
        if (double.TryParse(rate, out val))
        {
            SetMutationRate(val);
        }
    }

    public void SetTournamentSize(string size)
    {
        int val;
        if (int.TryParse(size, out val))
        {
            SetTournamentSize(val);
        }
    }

    public void SetGenerationStepSize(string size)
    {
        int val;
        if (int.TryParse(size, out val))
        {
            SetGenerationStepSize(val);
        }
    }

    public void SetPopulationDisplayAmount(int amount)
    {
        amount = Mathf.Clamp(amount, 1, Program.Instance.PopulationSize);

        PopDisplayInput.text = String.Empty + amount;

        Program.Instance.PopulationDisplayAmount = amount;

        Program.Instance.ShowPopulation(amount);
    }

    public void SetPopulationDisplayAmount(string amount)
    {
        int val;
        if (int.TryParse(amount, out val))
        {
            SetPopulationDisplayAmount(val);
        }
    }
}
