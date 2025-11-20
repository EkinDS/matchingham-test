using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchGameManager : MonoBehaviour
{
    [SerializeField] private EventBusManager _eventBusManager;
    [SerializeField] private CollectionPresenter _collectionPresenter;
    [SerializeField] private MatchGameData _matchGameData;
    [SerializeField] private MatchlingPresenter _matchlingPresenterPrefab;
    [SerializeField] private Background _background;
    [SerializeField] private LevelCompletedView _levelCompletedView;
    [SerializeField] private LevelFailedView _levelFailedView;
    [SerializeField] private LevelTimerPresenter _levelTimerPresenter;
    [SerializeField] private MatchGoalCheckerPresenter _matchGoalCheckerPresenter;

    private EventBus _eventBus;
    private List<MatchlingPresenter> _matchlingPresenters;
    private bool _readyToFinishLevel;


    private void Awake()
    {
        _eventBusManager.Initialize();
        _eventBus = _eventBusManager.EventBus;

        _collectionPresenter.Initialize(_eventBus);
        _levelCompletedView.Initialize(_eventBus);
        _levelFailedView.Initialize(_eventBus);
        _levelTimerPresenter.Initialize(_eventBus);
        _matchGoalCheckerPresenter.Initialize(_eventBus, _matchGameData);
        _background.Initialize();

        _eventBus.Subscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Subscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Subscribe<AllMatchGoalsFulfilledEvent>(HandleOnAllMatchGoalsFulfilled);
        _eventBus.Subscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);
        _eventBus.Subscribe<MatchCompletedEvent>(HandleOnMatchCompletedEvent);

        ResetGame();
        LoadGame();
    }

    private void ResetGame()
    {
        _readyToFinishLevel = false;

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

        _collectionPresenter.ResetForLevel();
    }

    private void LoadGame()
    {
        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId", 0);

        LevelData levelData = _matchGameData.levelDataList[currentLevelId % _matchGameData.levelDataList.Count];
        _levelTimerPresenter.StartTimer(levelData.timeLimit);

        _background.ResetForLevel(levelData.backgroundSprite);
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

            newMatchlingPresenter.Initialize(_eventBus, matchlingPlacement, matchlingType,
                _matchGameData.GetSprite(matchlingType));
        }
    }


    private void HandleOnNextLevelRequested(NextLevelRequestedEvent e)
    {
        ResetGame();
        LoadGame();
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
        _eventBus.Unsubscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Unsubscribe<AllMatchGoalsFulfilledEvent>(HandleOnAllMatchGoalsFulfilled);
        _eventBus.Unsubscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Unsubscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);
        _eventBus.Unsubscribe<MatchCompletedEvent>(HandleOnMatchCompletedEvent);
    }
}