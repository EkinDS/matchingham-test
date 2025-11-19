using System.Collections.Generic;
using UnityEngine;

public class MatchGameManager : MonoBehaviour
{
    [SerializeField] private EventBusManager _eventBusManager;
    [SerializeField] private CollectionPresenter _collectionPresenter;
    [SerializeField] private MatchGameData _matchGameData;
    [SerializeField] private MatchlingPresenter _matchlingPresenterPrefab;
    [SerializeField] private Background background;

    private EventBus _eventBus;
    private List<MatchlingPresenter> _matchlingPresenters;

    private void Awake()
    {
        _eventBusManager.Initialize();
        _eventBus = _eventBusManager.EventBus;

        _collectionPresenter.Initialize(_eventBus);

        SpawnLevel(1);
        
        _eventBus.Subscribe<MatchlingMatchedEvent>(HandleOnMatchlingMatched);
    }


    private void SpawnLevel(int levelId)
    {
        _matchlingPresenters = new List<MatchlingPresenter>();
        
        LevelData levelData = _matchGameData.levelDataList[levelId];

        background.Initialize(levelData.backgroundSprite);

        foreach (var matchlingPlacementData in levelData.matchlingPlacementDataList)
        {
            foreach (var matchlingPlacement in matchlingPlacementData.MatchlingPlacementList)
            {
                MatchlingPresenter newMatchlingPresenter = Instantiate(_matchlingPresenterPrefab, background.transform);

                _matchlingPresenters.Add(newMatchlingPresenter);
                
                newMatchlingPresenter.Initialize(_eventBus, matchlingPlacement, matchlingPlacementData.matchlingType, _matchGameData.GetSprite(matchlingPlacementData.matchlingType));
            }
        }
    }

    private void HandleOnMatchlingMatched(MatchlingMatchedEvent e)
    {
        _matchlingPresenters.Remove(e.MatchlingPresenter);

        if (_matchlingPresenters.Count == 0)
        {
            print("Level completed!");
        }
    }
}