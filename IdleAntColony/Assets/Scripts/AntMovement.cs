using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    [SerializeField] private Bank bank;
    [SerializeField] private Upgrade speedUpgrade;
    [SerializeField] private float baseLoadingDuration = 4f;
    [SerializeField] private int chewCount = 8;
    [SerializeField] private float unloadingDuration = 1f;
    [SerializeField] private float loadCapacity = 1f;
    private AntFoodInteraction _antFoodInteraction;
    private float _baseSpeed;
    private float _loadingDuration;
    private Vector3 _homePosition;
    private int _loadedFood;
    private NavMeshAgent _navMeshAgent;
    private Status _status = Status.Idle;
    private Target _target;
    private Transform _targetPiece;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _baseSpeed = _navMeshAgent.speed;
        _antFoodInteraction = GetComponent<AntFoodInteraction>();
    }

    private void Start()
    {
        OnSpeedLevelChanged(speedUpgrade.Level);
    }

    private void OnEnable()
    {
        speedUpgrade.StatChanged += OnSpeedLevelChanged;
    }

    private void OnDisable()
    {
        speedUpgrade.StatChanged -= OnSpeedLevelChanged;
    }

    private void OnSpeedLevelChanged(int speedLevel)
    {
        _navMeshAgent.speed = _baseSpeed * (1 + 0.1f * speedLevel);
        float work = _baseSpeed * baseLoadingDuration;
        _loadingDuration = work / _navMeshAgent.speed;
    }

    private void Update()
    {
        if (!_navMeshAgent.hasPath) OnNavigationEnded();
    }

    public void Work(int priority, Vector3 origin, Target target)
    {
        _navMeshAgent.avoidancePriority = priority;
        _homePosition = origin;
        _target = target;
        GoToTheNextPiece();
    }

    private void GoToTheNextPiece()
    {
        if (_target.GetNextPiece(_homePosition, out var piece))
        {
            _targetPiece = piece;
            _navMeshAgent.SetDestination(_targetPiece.position);
            _status = Status.GoingToResources;
        }
        else if (_loadedFood > 0)
        {
            ComeBackHome();
        }
        else if (_status != Status.Idle)
        {
            Destroy(gameObject);
        }
    }

    private void ComeBackHome()
    {
        _status = Status.ComingBackHome;
        _navMeshAgent.SetDestination(_homePosition);
    }

    private void OnNavigationEnded()
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
            case Status.Idle:
                ComeBackHome();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator LoadingCoroutine()
    {
        var elapsedTime = 0f;
        float chewingInterval = _loadingDuration / chewCount;
        float lossScale = chewingInterval / _loadingDuration;
        while (elapsedTime < _loadingDuration)
        {
            if (_targetPiece) _antFoodInteraction.Chew(_targetPiece, lossScale, chewingInterval);
            yield return new WaitForSeconds(chewingInterval);
            elapsedTime += chewingInterval;
        }

        if (++_loadedFood < loadCapacity)
            GoToTheNextPiece();
        else
            ComeBackHome();
    }

    private IEnumerator UnloadingCoroutine()
    {
        yield return new WaitForSeconds(unloadingDuration);
        bank.ExchangeFoodPiece(_loadedFood);
        _loadedFood = 0;
        GoToTheNextPiece();
    }

    private enum Status
    {
        GoingToResources,
        ComingBackHome,
        Loading,
        Unloading,
        Idle
    }
}