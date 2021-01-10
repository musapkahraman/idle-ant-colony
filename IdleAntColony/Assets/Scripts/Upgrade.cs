using System;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : Stat
{
    [SerializeField] private Bank bank;
    [SerializeField] private int level = 1;
    [SerializeField] private int cost = 1;
    [Range(0.1f, 1f)] [SerializeField] private float baseCostIncrease = 0.15f;

    public Action<int> CostChanged;

    public int Level => level;
    public int Cost => cost;

    private void OnValidate()
    {
        if (level < 1) level = 1;
        if (cost < 1) cost = 1;
        baseCostIncrease = Mathf.Clamp(baseCostIncrease, 0.1f, 1f);
    }

    public override int GetStat()
    {
        return level;
    }

    public bool IncreaseLevel()
    {
        if (bank.Spend(cost))
        {
            level++;
            OnStatChanged(level);
            IncreaseUpgradeCost();
            return true;
        }

        return false;
    }

    private void IncreaseUpgradeCost()
    {
        float costIncreaseRatio = baseCostIncrease + (float) level / (level - 1);
        var costFloatValue = (float) cost;
        costFloatValue *= costIncreaseRatio;
        cost = Mathf.RoundToInt(costFloatValue);
        CostChanged?.Invoke(cost);
    }
}