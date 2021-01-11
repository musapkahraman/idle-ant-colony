using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target[] targets;
    private Target _activeTarget;
    private int _index;

    private void Awake()
    {
        BringNextTarget();
    }

    public Target GetActiveTarget()
    {
        return _activeTarget;
    }

    public bool BringNextTarget()
    {
        if (_activeTarget) Destroy(_activeTarget.gameObject);

        if (_index < targets.Length)
        {
            _activeTarget = Instantiate(targets[_index++]);
            return true;
        }

        Debug.Log("Game is finished!");
        return false;
    }
}