using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target[] targets;
    private Target _activeTarget;

    private void Awake()
    {
        _activeTarget = Instantiate(targets[0]);
    }

    public Target GetActiveTarget()
    {
        return _activeTarget;
    }
}