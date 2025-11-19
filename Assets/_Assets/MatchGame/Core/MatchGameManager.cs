using System;
using System.Collections.Generic;
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

    private EventBus _eventBus;
    private List<MatchlingPresenter> _matchlingPresenters;


    private void Awake()
    {
        _eventBusManager.Initialize();
        _eventBus = _eventBusManager.EventBus;

        _eventBus.Subscribe<MatchlingMatchedEvent>(HandleOnMatchlingMatched);
        _eventBus.Subscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Subscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Subscribe<AllMatchlingsMatchedEvent>(HandleOnAllMatchlingsMatchedEvent);
        _eventBus.Subscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);

        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId", 0);

        StartLevel(currentLevelId);
    }

    private void ResetLevel()
    {
        foreach (var matchlingPresenter in _matchlingPresenters)
        {
            Destroy(matchlingPresenter.gameObject);
        }
        
    }

    private void StartLevel(int levelId)
    {
        _matchlingPresenters = new List<MatchlingPresenter>();

        LevelData levelData = _matchGameData.levelDataList[levelId % _matchGameData.levelDataList.Count];

        _collectionPresenter.Initialize(_eventBus);
        _levelCompletedView.Initialize(_eventBus);
        _levelFailedView.Initialize(_eventBus);
        _levelTimerPresenter.Initialize(_eventBus);
        _levelTimerPresenter.StartTimer(levelData.timeLimit);

        _background.Initialize(levelData.backgroundSprite);

        foreach (var matchlingPlacementData in levelData.matchlingPlacementDataList)
        {
            foreach (var matchlingPlacement in matchlingPlacementData.MatchlingPlacementList)
            {
                MatchlingPresenter newMatchlingPresenter =
                    Instantiate(_matchlingPresenterPrefab, _background.transform);

                _matchlingPresenters.Add(newMatchlingPresenter);

                newMatchlingPresenter.Initialize(_eventBus, matchlingPlacement, matchlingPlacementData.matchlingType,
                    _matchGameData.GetSprite(matchlingPlacementData.matchlingType));
            }
        }
    }

    private void HandleOnMatchlingMatched(MatchlingMatchedEvent e)
    {
        _matchlingPresenters.Remove(e.MatchlingPresenter);

        if (_matchlingPresenters.Count == 0)
        {
            _levelTimerPresenter.StopTimer();

            _eventBus.Publish(new AllMatchlingsMatchedEvent(_levelTimerPresenter.GetTimeLimit()));
        }
    }

    private void HandleOnNextLevelRequested(NextLevelRequestedEvent e)
    {
        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId", 0);

        StartLevel(currentLevelId);
    }


    private void HandleOnAllMatchlingsMatchedEvent(AllMatchlingsMatchedEvent e)
    {
        int currentLevelId = PlayerPrefs.GetInt("CurrentLevelId", 0);
        PlayerPrefs.SetInt("CurrentLevelId", currentLevelId + 1);

        _levelCompletedView.Appear(e.Timer);
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
        _eventBus.Unsubscribe<MatchlingMatchedEvent>(HandleOnMatchlingMatched);
        _eventBus.Unsubscribe<NextLevelRequestedEvent>(HandleOnNextLevelRequested);
        _eventBus.Unsubscribe<AllMatchlingsMatchedEvent>(HandleOnAllMatchlingsMatchedEvent);
        _eventBus.Unsubscribe<CollectionFilledEvent>(HandleOnCollectionFilledEvent);
        _eventBus.Unsubscribe<TimeRanOutEvent>(HandleOnTimeRanOutEvent);
    }
}