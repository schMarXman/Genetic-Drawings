using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour
{
    public static Program Instance;

    public SpriteRenderer SourceSprite;

    public Image TestSprite;

    public Text MaxFitnessLabel, CurrentFitnessLabel, GenerationLabel;

    //private SpriteRenderer mDrawSprite;
    public Image mDrawSprite;

    public Dropdown crossoverDropdown, selectionDropdown;

    private Population mPopulation;

    private int mGenerationCount = 0;

    public int PopulationSize = 200;

    public int GenStepSize = 10;

    public int PopulationDisplayAmount = 50;

    public Transform PopDrawerContentParent;

    public GameObject IndividualPrefab;

    private Coroutine mEvoCoroutine;

    private List<GameObject> mPopulationPrefabs = new List<GameObject>();

    void Awake()
    {
        Instance = this;

        Screen.SetResolution(1000, 500, false);
    }

    void Start()
    {
        //mDrawSprite = Instantiate(TestSprite.gameObject).GetComponent<Image>();
        //mDrawSprite = Instantiate(SourceSprite.gameObject).GetComponent<SpriteRenderer>();

        //mDrawSprite.transform.SetParent(TestSprite.transform.parent);
        //mDrawSprite.transform.position += Vector3.right * 1;

        var colors = TestSprite.sprite.texture.GetPixels32();

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        Texture2D tex = TestSprite.sprite.texture;
        Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        newTex.SetPixels32(colors);
        mDrawSprite.sprite = Sprite.Create(newTex, TestSprite.sprite.rect, new Vector2(0.5f, 0.5f));

        mDrawSprite.sprite.texture.filterMode = FilterMode.Point;

        mDrawSprite.sprite.texture.Apply();

        Individual.DefaultGeneLength = colors.Length;

        //mPopulation = new Population(PopulationSize, true);

        mGenerationCount = 0;

        FitnessCalculator.SetSolution(TestSprite.sprite.texture.GetPixels32());
        MaxFitnessLabel.text = "Maximum fitness: " + FitnessCalculator.GetMaxFitness();

    }

    public bool EvolutionRunning()
    {
        return mEvoCoroutine != null;
    }

    public void StartEvolution()
    {
        if (mEvoCoroutine == null)
        {
            mEvoCoroutine = StartCoroutine(Evolution());
        }
    }

    public void StopEvolution()
    {
        if (mEvoCoroutine != null)
        {
            StopCoroutine(mEvoCoroutine);
            mEvoCoroutine = null;

            ShowPopulation(mPopulation, PopulationDisplayAmount);
        }
    }

    public void StepForward()
    {
        if (mEvoCoroutine == null)
        {
            if (mPopulation == null)
            {
                mPopulation = new Population(PopulationSize, true);
            }

            for (int i = 0; i < GenStepSize; i++)
            {
                mPopulation = GeneticAlgorithm.EvolvePopulation(mPopulation, selectionDropdown.value, crossoverDropdown.value);
            }

            mGenerationCount += GenStepSize;

            var fittest = mPopulation.GetFittest();
            SetColors(fittest.GetGenes());

            ShowPopulation(mPopulation, PopulationDisplayAmount);

            CurrentFitnessLabel.text = "Fitness: " + fittest.GetFitness();
            GenerationLabel.text = "Generation: " + mGenerationCount;
        }
    }

    public void ShowCurrentPopulation(int amount)
    {
        ShowPopulation(mPopulation, PopulationDisplayAmount);
    }

    public void ShowPopulation(Population pop, int amount)
    {
        if (UIController.Instance.PopulationView.activeSelf && pop != null)
        {
            if (mPopulationPrefabs.Count != 0)
            {
                for (int i = 0; i < mPopulationPrefabs.Count; i++)
                {
                    Destroy(mPopulationPrefabs[i]);
                }

                mPopulationPrefabs.Clear();
            }

            for (int i = 0; i < amount; i++)
            {
                var image = Instantiate(IndividualPrefab).GetComponent<Image>();
                var text = image.GetComponentInChildren<Text>();

                image.sprite = CreateSprite();
                image.transform.SetParent(PopDrawerContentParent);

                var indi = pop.GetIndividual(i);

                text.text = "#" + (i + 1) + " F: " + indi.GetFitness();

                image.sprite.texture.SetPixels32(indi.GetGenes());
                image.sprite.texture.filterMode = FilterMode.Point;
                image.sprite.texture.Apply();

                mPopulationPrefabs.Add(image.gameObject);
            }
        }
    }

    public void ShowOrderedPopulation()
    {
        if (mPopulation != null)
        {
            var orderedPop = mPopulation.OrderByFitness(true);
            ShowPopulation(orderedPop, PopulationDisplayAmount);
        }
    }

    public void SelectSourceImage(Image image)
    {
        if (!EvolutionRunning())
        {
            TestSprite.sprite = image.sprite;
            mDrawSprite.sprite = CreateSprite();
            Reset();
            MaxFitnessLabel.text = "Maximum fitness: " + FitnessCalculator.GetMaxFitness();
        }
    }

    private Sprite CreateSprite()
    {
        var colors = TestSprite.sprite.texture.GetPixels32();

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        Texture2D tex = TestSprite.sprite.texture;
        Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        newTex.SetPixels32(colors);
        return Sprite.Create(newTex, TestSprite.sprite.rect, new Vector2(0.5f, 0.5f));
    }

    public void Reset()
    {
        if (mEvoCoroutine == null)
        {
            var colors = TestSprite.sprite.texture.GetPixels32();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.white;
            }

            SetColors(colors);

            Individual.DefaultGeneLength = colors.Length;

            FitnessCalculator.SetSolution(TestSprite.sprite.texture.GetPixels32());
            mPopulation = null;

            mGenerationCount = 0;

            CurrentFitnessLabel.text = "Fitness: ";
            GenerationLabel.text = "Generation: " + mGenerationCount;
        }
    }

    public void TestSuite()
    {

        for (int steps = 1; steps < 3; steps++)
        {
            GenStepSize = 500 * steps;
            StringBuilder str = new StringBuilder();
            // selections
            for (int i = 0; i < 3; i++)
            {
                selectionDropdown.value = i;
                // crossover
                for (int j = 0; j < 5; j++)
                {
                    int avg = 0;

                    for (int av = 0; av < 3; av++)
                    {
                        crossoverDropdown.value = j;
                        Reset();
                        StepForward();
                        avg += mPopulation.GetFittest().GetFitness();
                    }

                    avg /= 3;

                    str.Append((500*steps).ToString() + " " + i.ToString() + " " + j.ToString() + " " + avg.ToString());
                    str.Append(Environment.NewLine);
                    var img = mDrawSprite.sprite.texture.EncodeToPNG();
                    System.IO.File.WriteAllBytes("C:\\tmp\\" + i.ToString() + "." + j.ToString() + "." + (500 * steps).ToString() + ".png", img);
                }
            }

            System.IO.File.WriteAllText("C:\\temp\\" + (500 * steps).ToString() + ".txt", str.ToString());
        }
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    IEnumerator Evolution()
    {
        if (mPopulation == null)
        {
            mPopulation = new Population(PopulationSize, true);
        }

        while (mPopulation.GetFittest().GetFitness() < FitnessCalculator.GetMaxFitness())
        {
            mGenerationCount++;

            //if (mGenerationCount % 100 == 0)
            //{
            var fittest = mPopulation.GetFittest();
            var fitness = fittest.GetFitness();
            Debug.Log("Generation: " + mGenerationCount + " Fittest: " + fitness);

            CurrentFitnessLabel.text = "Fitness: " + fitness;
            GenerationLabel.text = "Generation: " + mGenerationCount;

            mPopulation = GeneticAlgorithm.EvolvePopulation(mPopulation, selectionDropdown.value, crossoverDropdown.value);

            SetColors(fittest.GetGenes());
            //}
            //ShowPopulation(200);

            yield return null;
        }

        Debug.Log("Solution found!");
        Debug.Log("Generation: " + mGenerationCount);
        Debug.Log("Genes:");
        Debug.Log(mPopulation.GetFittest());
    }

    // test
    void TestColor()
    {
        var colors = mDrawSprite.sprite.texture.GetPixels32();

        for (int i = 0; i < 200; i++)
        {
            colors[Random.Range(0, colors.Length - 1)] = Color.black;
        }

        mDrawSprite.sprite.texture.SetPixels32(colors);
        mDrawSprite.sprite.texture.Apply();
    }

    void SetColors(Color32[] colors)
    {
        mDrawSprite.sprite.texture.SetPixels32(colors);
        mDrawSprite.sprite.texture.Apply();
    }
}
