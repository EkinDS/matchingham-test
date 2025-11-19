using UnityEngine;
using UnityEngine.UI;

public class MatchGameManager : MonoBehaviour
{
    [SerializeField] private EventBusManager _eventBusManager;
    [SerializeField] private CollectionPresenter _collectionPresenter;
    [SerializeField] private MatchGameData _matchGameData;
    [SerializeField] private MatchlingPresenter _matchlingPresenterPrefab;
    [SerializeField] private Image backgroundImage;

    private EventBus _eventBus;

    private void Awake()
    {
        _eventBusManager.Initialize();
        _eventBus = _eventBusManager.EventBus;

        _collectionPresenter.Initialize(_eventBus);

        SpawnMatchlings(0);
    }


    private void SpawnMatchlings(int levelId)
    {
        LevelData levelData = _matchGameData.levelDataList[levelId];

        backgroundImage.sprite = levelData.backgroundSprite;

        foreach (var matchlingPlacementData in levelData.matchlingPlacementDataList)
        {
            foreach (var matchlingPlacement in matchlingPlacementData.MatchlingPlacementList)
            {
                MatchlingPresenter newMatchlingPresenter =
                    Instantiate(_matchlingPresenterPrefab, backgroundImage.transform);

                newMatchlingPresenter.Initialize(_eventBus, matchlingPlacement, matchlingPlacementData.matchlingType, _matchGameData.GetSprite(matchlingPlacementData.matchlingType));
            }
        }
    }
}