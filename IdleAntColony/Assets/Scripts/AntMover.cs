using System;
using UnityEngine;
using UnityEngine.AI;

public class AntMover : MonoBehaviour
{
    [SerializeField] private float gatheringDuration;
    private NavMeshAgent _navMeshAgent;
    private Status _status = Status.GoingToResources;
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
                case Status.Gathering:
                    break;
                case Status.GoingToResources:
                    _navMeshAgent.SetDestination(_origin);
                    _status = Status.ComingBackHome;
                    break;
                case Status.ComingBackHome:
                    _navMeshAgent.SetDestination(_target);
                    _status = Status.GoingToResources;
                    break;
                case Status.EmptyingHand:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private enum Status
    {
        Gathering,
        GoingToResources,
        ComingBackHome,
        EmptyingHand
    }
}