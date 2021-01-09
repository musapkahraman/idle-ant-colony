using System;
using UnityEngine;
using UnityEngine.AI;

public class AntMover : MonoBehaviour
{
    [SerializeField] private float gatheringDuration;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform target;
    private NavMeshAgent _navMeshAgent;
    private Status _status = Status.GoingToResources;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _navMeshAgent.SetDestination(target.position);
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
                    _navMeshAgent.SetDestination(origin.position);
                    _status = Status.ComingBackHome;
                    break;
                case Status.ComingBackHome:
                    _navMeshAgent.SetDestination(target.position);
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