using System.Collections;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    [SerializeField] private Upgrade workersUpgrade;
    [SerializeField] private GameObject antPrefab;
    public Transform nest;
    public Target target;
    private int _newAntAgentPriority;

    private void Start()
    {
        StartCoroutine(InitiateCurrentColony());

        IEnumerator InitiateCurrentColony()
        {
            for (var i = 0; i < workersUpgrade.Level; i++)
            {
                SpawnFrom(nest.position);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void OnWorkersUpgradeButtonClicked()
    {
        if (workersUpgrade.IncreaseLevel())
            SpawnFrom(transform.position);
    }

    private void SpawnFrom(Vector3 point)
    {
        Instantiate(antPrefab, point, Quaternion.identity).GetComponent<AntMovement>()
            .Work(_newAntAgentPriority++, nest.position, target);
    }
}