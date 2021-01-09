using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AntMover : MonoBehaviour
{
    [SerializeField] private float chewingInterval = 0.5f;
    [SerializeField] private float loadingDuration = 4f;
    [SerializeField] private float unloadingDuration = 1f;
    private NavMeshAgent _navMeshAgent;
    private Status _status;
    private Vector3 _origin;
    private Target _target;
    private Transform _targetPiece;
    private EatingEffects _eatingEffects;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _eatingEffects = GetComponent<EatingEffects>();
    }

    public void Gather(int priority, Vector3 origin, Target target)
    {
        _navMeshAgent.avoidancePriority = priority;
        _origin = origin;
        _target = target;
        GoToTheNextPiece();
    }

    private void GoToTheNextPiece()
    {
        if (_target.GetNextPiece(_origin, out var piece))
        {
            _targetPiece = piece;
            _navMeshAgent.SetDestination(_targetPiece.position);
            _status = Status.GoingToResources;
        }
        else
        {
            Destroy(gameObject);
        }
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
        var elapsedTime = 0f;
        float lossScale = chewingInterval / loadingDuration;
        while (elapsedTime < loadingDuration)
        {
            if (_targetPiece) _eatingEffects.Chew(_targetPiece, lossScale);
            yield return new WaitForSeconds(chewingInterval);
            elapsedTime += chewingInterval;
        }

        _status = Status.ComingBackHome;
        _navMeshAgent.SetDestination(_origin);
    }

    private IEnumerator UnloadingCoroutine()
    {
        yield return new WaitForSeconds(unloadingDuration);
        GoToTheNextPiece();
    }

    private enum Status
    {
        GoingToResources,
        ComingBackHome,
        Loading,
        Unloading
    }
}