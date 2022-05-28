﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AIProject.Classes;
using AIProject.Classes.Extensions;

namespace AIProject.Classes;


public class NeuralNetwork
{

    private readonly Random _randomizer = new Random();
    //Wartości dla każdej warstwy -> Values[i][j] gdzie i -> numer warstwy,
    //j -> numer neuronu
    public double[][] Values { get; set; }

    //Biasy dla kazdej warstwy -> Biases[i][j] gdzie i -> a numer warstwy,
    //j -> numer neuronu
    public double[][] Biases { get; set; }

    //Waga dla każdej warstwy -> Weights[i][j][k] gdzie i-> numer warstwy, 
    //j -> numer neuronu w warstwie i, k-> numer neuronu w warstwie i+1
    //Jeśli nasza sieć ma N warstw to -> Weights[N-1][][]
    public double[][][] Weights { get; set; }
    
    public double LearningRate = 0.001;

    public NeuralNetwork(int[] layers)
    {
        Values = new double[layers.Length][];
        Biases = new double[layers.Length][];
        Weights = new double[layers.Length-1][][];

        for (int i = 0; i < layers.Length; i++)
        {
            Values[i] = new double[layers[i]];
            Biases[i] = new double[layers[i]];
        }
        for (int i = 0; i < layers.Length-1 ; i++)
        {
            Weights[i] = new double[layers[i]][];
            
            for (int j = 0; j < Weights[i].Length; j++)
            {
                Weights[i][j] = new double[layers[i+1]];
            }
        }
    }    /// <summary>
    /// Inicjalizuje wagi i biasy w przedziale od min do max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void InitializeWeightsAndBiases(int min, int max)
    {
        for (int i = 0; i < Weights.GetLength(0); i++)
            for (int j = 0; j < Weights[i].Length; j++)
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    Weights[i][j][k] = _randomizer.NextDouble(min, max);
                }
    }

    /// <summary>
    /// Przyjmuje tablicę wartości dla pierwszej warstwy
    /// </summary>
    /// <param name="input"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void ReadInput(int[] input)
    {
        if (input.Length != Values.GetLength(0))
            throw new ArgumentOutOfRangeException("input");

        for (int i = 0; i < input.Length; i++)
        {
            Values[0][i] = input[i];
        }
    }
    public void Train()
    {
        for (int i = 1; i < Values.GetLength(0); i++)
            for (int j=0; j < Weights[i].Length; j++)
            {
                Values[i][j] = Sum(Values[i - 1], Weights[i - 1]);
            }
    }
    public double Sum(double[] values, double[][] weights) => values.Select((v, i) => v * weights[i][0]).Sum();

}
