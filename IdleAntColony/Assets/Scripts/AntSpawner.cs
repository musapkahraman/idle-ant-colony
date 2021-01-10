using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    [SerializeField] private Upgrade workersUpgrade;
    [SerializeField] private GameObject antPrefab;
    public Transform origin;
    public Target target;
    private int _newAntAgentPriority;

    public void Spawn()
    {
        if (workersUpgrade.IncreaseLevel())
            Instantiate(antPrefab).GetComponent<AntMovement>()
                .Work(_newAntAgentPriority++, origin.position, target);
    }
}