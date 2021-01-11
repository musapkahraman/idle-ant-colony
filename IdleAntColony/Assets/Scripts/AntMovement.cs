using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntMovement : MonoBehaviour
{
    [SerializeField] private Bank bank;
    [SerializeField] private Upgrade speedUpgrade;
    [SerializeField] private Upgrade powerUpgrade;
    [SerializeField] private float baseLoadingDuration = 4f;
    [SerializeField] private int chewCount = 8;
    [SerializeField] private float unloadingDuration = 1f;
    private readonly List<Transform> _loadedPieces = new List<Transform>();
    private IAntDestroyedListener _antDestroyedListener;
    private AntFoodInteraction _antFoodInteraction;
    private float _baseSpeed;
    private Vector3 _homePosition;
    private float _loadingDuration;
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

    private void Update()
    {
        if (!_navMeshAgent.hasPath) OnNavigationEnded();
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

    public void Work(IAntDestroyedListener antDestroyedListener, int priority, Vector3 origin, Target target)
    {
        _antDestroyedListener = antDestroyedListener;
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
        else if (_loadedPieces.Count > 0)
        {
            ComeBackHome();
        }
        else if (_status != Status.Idle)
        {
            _antDestroyedListener.OnAntDestroyed(gameObject.GetInstanceID());
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

        CompleteLoading();

        if (_loadedPieces.Count < powerUpgrade.Level)
            GoToTheNextPiece();
        else
            ComeBackHome();
    }

    private void CompleteLoading()
    {
        _targetPiece.parent = transform;
        var pieceTransform = _targetPiece.transform;
        pieceTransform.localScale = Vector3.one / 2;
        pieceTransform.localPosition = Vector3.up / 2 + Vector3.up / 4 * _loadedPieces.Count;
        _loadedPieces.Add(_targetPiece);
    }

    private IEnumerator UnloadingCoroutine()
    {
        yield return new WaitForSeconds(unloadingDuration);
        bank.ExchangeFoodPiece(_loadedPieces.Count);
        foreach (var piece in _loadedPieces) Destroy(piece.gameObject);
        _loadedPieces.Clear();
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