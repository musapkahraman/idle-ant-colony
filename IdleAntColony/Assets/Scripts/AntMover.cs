using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AntMover : MonoBehaviour
{
    [SerializeField] private float loadingDuration;
    [SerializeField] private float unloadingDuration;
    private NavMeshAgent _navMeshAgent;
    private Status _status;
    private Vector3 _origin;
    private Vector3 _target;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Gather(int priority, Vector3 origin, Vector3 target)
    {
        _navMeshAgent.avoidancePriority = priority;
        _origin = origin;
        _target = target;
        _navMeshAgent.SetDestination(_target);
        _status = Status.GoingToResources;
    }

    private void Update()
    {
        if (!_navMeshAgent.hasPath)
        {
            switch (_status)
            {
                case Status.GoingToResources:
                    _status = Status.Loading;
                    StartCoroutine(LoadingCoroutine());
                    break;
                case Status.ComingBackHome:
                    _status = Status.Unloading;
                    StartCoroutine(UnloadingCoroutine());
                    break;
                case Status.Loading:
                    break;
                case Status.Unloading:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(loadingDuration);
        _status = Status.ComingBackHome;
        _navMeshAgent.SetDestination(_origin);
    }

    private IEnumerator UnloadingCoroutine()
    {
        yield return new WaitForSeconds(unloadingDuration);
        _status = Status.GoingToResources;
        _navMeshAgent.SetDestination(_target);
    }

    private enum Status
    {
        GoingToResources,
        ComingBackHome,
        Loading,
        Unloading
    }
}