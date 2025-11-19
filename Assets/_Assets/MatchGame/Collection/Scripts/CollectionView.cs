using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectionView : MonoBehaviour
{
    [SerializeField] private List<Transform> _collectionTargets;


    public void RearrangeMatchlingPresenters(List<MatchlingPresenter> matchlingPresenters)
    {
        for (var i = 0; i < matchlingPresenters.Count; i++)
        {
            var matchlingPresenter = matchlingPresenters[i];

            matchlingPresenter.MoveToCollectionSlot(transform, _collectionTargets[i].localPosition, 100);
        }
    }

    public void Match(List<MatchlingPresenter> matchlingPresentersToMatch)
    {
        float matchPositionX = matchlingPresentersToMatch[1].transform.localPosition.x;
        float matchPositionY = matchlingPresentersToMatch[1].transform.localPosition.y + 200F;
        
        foreach (var matchlingPresenter in matchlingPresentersToMatch)
        {
            matchlingPresenter.Match(matchPositionX, matchPositionY);
        }
    }
}