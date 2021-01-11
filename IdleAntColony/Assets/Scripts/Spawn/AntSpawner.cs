using System.Collections;
using System.Collections.Generic;
using IdleAnt.Movement;
using IdleAnt.Stats;
using UnityEngine;

namespace IdleAnt.Spawn
{
    public class AntSpawner : MonoBehaviour, IAntDestroyedListener
    {
        [SerializeField] private Upgrade workersUpgrade;
        [SerializeField] private GameObject antPrefab;
        public Transform nest;
        private readonly List<int> _spawnedAnts = new List<int>();
        private int _newAntAgentPriority;
        private TargetSpawner _targetSpawner;

        private void Awake()
        {
            _targetSpawner = GetComponent<TargetSpawner>();
        }

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

        public void OnAntDestroyed(int antInstanceId)
        {
            _spawnedAnts.Remove(antInstanceId);
            if (_spawnedAnts.Count == 0 && _targetSpawner.BringNextTarget()) Start();
        }

        public void OnWorkersUpgradeButtonClicked()
        {
            if (workersUpgrade.IncreaseLevel())
                SpawnFrom(transform.position);
        }

        private void SpawnFrom(Vector3 point)
        {
            var ant = Instantiate(antPrefab, point, Quaternion.identity);
            _spawnedAnts.Add(ant.GetInstanceID());
            ant.GetComponent<AntMovement>()
                .Work(this, _newAntAgentPriority++, nest.position, _targetSpawner.GetActiveTarget());
        }
    }
}