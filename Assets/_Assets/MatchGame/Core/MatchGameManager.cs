using System;
using UnityEngine;
using UnityEngine.UI;

public class MatchGameManager : MonoBehaviour
{
    [SerializeField] private CollectionPresenter _collectionPresenter;
    [SerializeField] private MatchGameData _matchGameData;
    [SerializeField] private MatchlingPresenter _matchlingPresenterPrefab;
    [SerializeField] private Image backgroundImage;

    private void Awake()
    {
        _collectionPresenter.Initialize();
        
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
                MatchlingPresenter newMatchlingPresenter = Instantiate(_matchlingPresenterPrefab, backgroundImage.transform);

                newMatchlingPresenter.Initialize(matchlingPlacement, _matchGameData.GetSprite( matchlingPlacementData.matchlingType));
            }
        }
    }
}
