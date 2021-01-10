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

    public Action<int> CostChanged;

    public int Level => level;
    public int Cost => cost;

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
        var costFloatValue = (float) cost;
        costFloatValue *= costIncreaseRatio;
        cost = (int) costFloatValue;
        CostChanged?.Invoke(cost);
    }
}