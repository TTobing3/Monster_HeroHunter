using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class WeightRandomPick<T>
{

    /// <summary> 전체 아이템의 가중치 합 </summary>
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
    private readonly Dictionary<T, double> normalizedItemWeightDict; // 확률이 정규화된 아이템 목록

    /// <summary> 가중치 합이 계산되지 않은 상태인지 여부 </summary>
    private bool isDirty;
    private double _sumOfWeights;

    /// <summary> WeightRandomPick 객체 </summary>
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

    /// <summary> 새로운 아이템-가중치 쌍 추가 </summary>
    public void Add(T item, double weight)
    {
        itemWeightDict.Add(item, weight);
        isDirty = true;
    }

  
    /// <summary> 랜덤 뽑기 </summary>
    public T GetRandomPick()
    {
        // 랜덤 계산
        double chance = randomInstance.NextDouble(); // [0.0, 1.0)
        chance *= SumOfWeights;

        return GetRandomPick(chance);
    }

    /// <summary> 직접 랜덤 값을 지정하여 뽑기 </summary>
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
    /// <summary> 모든 아이템의 가중치 합 계산해놓기 </summary>
    private void CalculateSumIfDirty()
    {
        if (!isDirty) return;
        isDirty = false;

        _sumOfWeights = 0.0;
        foreach (var pair in itemWeightDict)
        {
            _sumOfWeights += pair.Value;
        }

        // 정규화 딕셔너리도 업데이트
        UpdateNormalizedDict();
    }

    /// <summary> 정규화된 딕셔너리 업데이트 </summary>
    private void UpdateNormalizedDict()
    {
        normalizedItemWeightDict.Clear();
        foreach (var pair in itemWeightDict)
        {
            normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
        }
    }
}