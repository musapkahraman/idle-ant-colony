using System;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    [SerializeField] private Bank bank;
    [SerializeField] private int level = 1;
    [SerializeField] private int cost = 1;
    [Range(1f, 10f)] [SerializeField] private float costIncreaseRatio = 1.4f;

    public Action<int> LevelChanged;

    public Action<int> PriceChanged;

    private void OnValidate()
    {
        if (level < 1) level = 1;
        if (cost < 1) cost = 1;
        costIncreaseRatio = Mathf.Clamp(costIncreaseRatio, 1f, 10f);
    }

    public bool IncreaseLevel()
    {
        if (bank.Spend(cost))
        {
            level++;
            LevelChanged?.Invoke(level);
            IncreaseUpgradeCost();
            return true;
        }

        return false;
    }

    private void IncreaseUpgradeCost()
    {
        var priceFloatValue = (float) cost;
        priceFloatValue *= costIncreaseRatio;
        cost = (int) priceFloatValue;
        PriceChanged?.Invoke(cost);
    }
}