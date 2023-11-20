using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WeightRandomPick<T>
{

    /// <summary> ��ü �������� ����ġ �� </summary>
    public double SumOfWeights
    {
        get
        {
            CalculateSumIfDirty();
            return _sumOfWeights;
        }
    }

    private System.Random randomInstance;

    private readonly Dictionary<T, double> itemWeightDict;
    private readonly Dictionary<T, double> normalizedItemWeightDict; // Ȯ���� ����ȭ�� ������ ���

    /// <summary> ����ġ ���� ������ ���� �������� ���� </summary>
    private bool isDirty;
    private double _sumOfWeights;

    /// <summary> WeightRandomPick ��ü </summary>
    public WeightRandomPick()
    {
        randomInstance = new System.Random();
        itemWeightDict = new Dictionary<T, double>();
        normalizedItemWeightDict = new Dictionary<T, double>();
        isDirty = true;
        _sumOfWeights = 0.0;
    }

    public WeightRandomPick(int randomSeed)
    {
        randomInstance = new System.Random(randomSeed);
        itemWeightDict = new Dictionary<T, double>();
        normalizedItemWeightDict = new Dictionary<T, double>();
        isDirty = true;
        _sumOfWeights = 0.0;
    }

    /// <summary> ���ο� ������-����ġ �� �߰� </summary>
    public void Add(T item, double weight)
    {
        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

  
    /// <summary> ���� �̱� </summary>
    public T GetRandomPick()
    {
        // ���� ���
        double chance = randomInstance.NextDouble(); // [0.0, 1.0)
        chance *= SumOfWeights;

        return GetRandomPick(chance);
    }

    /// <summary> ���� ���� ���� �����Ͽ� �̱� </summary>
    public T GetRandomPick(double randomValue)
    {
        if (randomValue < 0.0) randomValue = 0.0;
        if (randomValue > SumOfWeights) randomValue = SumOfWeights - 0.00000001;

        double current = 0.0;
        foreach (var pair in itemWeightDict)
        {
            current += pair.Value;

            if (randomValue < current)
            {
                return pair.Key;
            }
        }

        throw new Exception($"Unreachable - [Random Value : {randomValue}, Current Value : {current}]");
        //return itemPairList[itemPairList.Count - 1].item; // Last Item
    }
    /// <summary> ��� �������� ����ġ �� ����س��� </summary>
    private void CalculateSumIfDirty()
    {
        if (!isDirty) return;
        isDirty = false;

        _sumOfWeights = 0.0;
        foreach (var pair in itemWeightDict)
        {
            _sumOfWeights += pair.Value;
        }

        // ����ȭ ��ųʸ��� ������Ʈ
        UpdateNormalizedDict();
    }

    /// <summary> ����ȭ�� ��ųʸ� ������Ʈ </summary>
    private void UpdateNormalizedDict()
    {
        normalizedItemWeightDict.Clear();
        foreach (var pair in itemWeightDict)
        {
            normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
        }
    }
}