using System;
using UnityEngine;

public abstract class Stat : ScriptableObject
{
    public abstract int GetStat();
    public event Action<int> StatChanged;

    protected void OnStatChanged(int value)
    {
        StatChanged?.Invoke(value);
    }
}