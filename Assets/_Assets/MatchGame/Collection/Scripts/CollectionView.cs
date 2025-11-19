using System.Collections.Generic;
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
}