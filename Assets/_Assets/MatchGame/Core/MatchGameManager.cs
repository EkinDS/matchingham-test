using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchGameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EventBusManager _eventBusManager;
    [SerializeField] private CollectionPresenter _collectionPresenter;
    [SerializeField] private MatchGameData _matchGameData;
    [SerializeField] private MatchlingPresenter _matchlingPresenterPrefab;
    [SerializeField] private Background _background;
    [SerializeField] private LevelCompletedView _levelCompletedView;
    [SerializeField] private LevelFailedView _levelFailedView;
    [SerializeField] private LevelTimerPresenter _levelTimerPresenter;
    [SerializeField] private MatchGoalCheckerPresenter _matchGoalCheckerPresenter;
    [SerializeField] private SpriteService _spriteService;

    private EventBus _eventBus;
    private List<MatchlingPresenter> _matchlingPresenters;
    private bool _readyToFinishLevel;
    private Coroutine _loadGameCoroutine;

    private void Awake()
    {
        _eventBusManager.Initialize();
        _eventBus = _eventBusManager.EventBus;

        _collectionPresenter.Initialize(_eventBus);
        _levelCompletedView.Initialize(_eventBus);
        _levelFailedView.Initialize(_eventBus);
        _levelTimerPresenter.Initialize(_eventBus);
        _matchGoalCheckerPresenter.Initialize(_eventBus, _spriteService);
        _background.Initialize();

        _eventBus.Subscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Subscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Subscribe<AllMatchGoalsFulfilledEvent>(HandleOnAllMatchGoalsFulfilled);
        _eventBus.Subscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);
        _eventBus.Subscribe<MatchCompletedEvent>(HandleOnMatchCompletedEvent);

        ResetGame();
        _loadGameCoroutine = StartCoroutine(LoadGameCoroutine());
    }

    private void ResetGame()
    {
        _readyToFinishLevel = false;

        if (_loadGameCoroutine != null)
        {
            StopCoroutine(_loadGameCoroutine);
            _loadGameCoroutine = null;
        }

        if (_matchlingPresenters != null)
        {
            foreach (var matchlingPresenter in _matchlingPresenters)
            {
                if (matchlingPresenter)
                {
                    Destroy(matchlingPresenter.gameObject);
                }
            }
        }

        _matchlingPresenters = new List<MatchlingPresenter>();

        _spriteService.UnloadAll();

        _collectionPresenter.ResetForLevel();
    }


    private IEnumerator LoadGameCoroutine()
    {
        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId", 0);

        LevelData levelData = _matchGameData.levelDataList[currentLevelId % _matchGameData.levelDataList.Count];

        var typesNeeded = new HashSet<MatchlingType>();

        if (levelData.matchlingPlacementDataList != null)
        {
            foreach (var placementData in levelData.matchlingPlacementDataList)
            {
                typesNeeded.Add(placementData.matchlingType);
            }
        }

        if (levelData.matchGoalList != null)
        {
            foreach (var goal in levelData.matchGoalList)
            {
                typesNeeded.Add(goal.matchlingType);
            }
        }

        _levelTimerPresenter.StartTimer(levelData.timeLimit, currentLevelId);

        yield return StartCoroutine(_spriteService.LoadMatchlingSprites(typesNeeded));
        yield return StartCoroutine(_spriteService.LoadBackgroundSprite(levelData.backgroundType));


        Sprite bgSprite = _spriteService.GetBackgroundSprite(levelData.backgroundType);
        _background.ResetForLevel(bgSprite);

        _matchGoalCheckerPresenter.ResetForLevel(levelData);

        SpawnMatchlings(levelData);
    }



    private void SpawnMatchlings(LevelData levelData)
    {
        var allPlacements = new List<(MatchlingPlacement placement, MatchlingType type)>();

        foreach (var matchlingPlacementData in levelData.matchlingPlacementDataList)
        {
            foreach (var matchlingPlacement in matchlingPlacementData.MatchlingPlacementList)
            {
                allPlacements.Add((matchlingPlacement, matchlingPlacementData.matchlingType));
            }
        }

        var orderedPlacements = allPlacements.OrderBy(x => x.placement.order).ToList();

        foreach (var entry in orderedPlacements)
        {
            var matchlingPlacement = entry.placement;
            var matchlingType = entry.type;

            MatchlingPresenter newMatchlingPresenter = Instantiate(_matchlingPresenterPrefab, _background.transform);

            _matchlingPresenters.Add(newMatchlingPresenter);

            Sprite sprite = _spriteService.GetMatchlingSprite(matchlingType);

            newMatchlingPresenter.Initialize(_eventBus, matchlingPlacement, matchlingType, sprite);
        }
    }


    private void HandleOnNextLevelRequested(NextLevelRequestedEvent e)
    {
        ResetGame();
        _loadGameCoroutine = StartCoroutine(LoadGameCoroutine());
    }

    private void HandleOnAllMatchGoalsFulfilled(AllMatchGoalsFulfilledEvent e)
    {
        if (_readyToFinishLevel)
        {
            return;
        }

        _readyToFinishLevel = true;

        _levelTimerPresenter.StopTimer();

        PlayerPrefs.SetInt("CurrentLevelId", PlayerPrefs.GetInt("CurrentLevelId", 0) + 1);
    }

    private void HandleOnMatchCompletedEvent(MatchCompletedEvent e)
    {
        if (!_readyToFinishLevel)
        {
            return;
        }

        _levelCompletedView.Appear(_levelTimerPresenter.GetTimeLimit());
    }

    private void HandleOnCollectionFilledEvent(CollectionFilledEvent e)
    {
        _levelFailedView.Appear();
    }

    private void HandleOnTimeRanOutEvent(TimeRanOutEvent e)
    {
        _levelFailedView.Appear();
    }

    private void OnDestroy()
    {
        if (_eventBus == null)
        {
            return;
        }

        _eventBus.Unsubscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Unsubscribe<AllMatchGoalsFulfilledEvent>(HandleOnAllMatchGoalsFulfilled);
        _eventBus.Unsubscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Unsubscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);
        _eventBus.Unsubscribe<MatchCompletedEvent>(HandleOnMatchCompletedEvent);
    }
}
