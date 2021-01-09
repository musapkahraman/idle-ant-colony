using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    [SerializeField] private GameObject antPrefab;
    public Transform origin;
    public Target target;
    private int _newAntAgentPriority;

    public void Spawn()
    {
        Instantiate(antPrefab).GetComponent<AntMovement>()
            .Gather(_newAntAgentPriority++, origin.position, target);
    }
}