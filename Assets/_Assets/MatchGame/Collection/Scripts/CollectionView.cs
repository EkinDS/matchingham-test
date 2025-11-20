using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectionView : MonoBehaviour
{
    [SerializeField] private List<Transform> _collectionTargets;
    [SerializeField] private Transform _collectionCarrierPrefab;
    


    public void RearrangeMatchlingPresenters(List<MatchlingPresenter> matchlingPresenters)
    {
        for (var i = 0; i < matchlingPresenters.Count; i++)
        {
            var matchlingPresenter = matchlingPresenters[i];

            matchlingPresenter.transform.parent.DOLocalMove(_collectionTargets[i].transform.localPosition, 0.5F)
                .SetEase(Ease.Linear).OnComplete(() => { matchlingPresenter.OnPlacedInCollection(); });
        }
    }

    public void PlaceMatchling(MatchlingPresenter matchlingPresenter, int slotIndex)
    {
        Transform newCollectionCarrierTransform = Instantiate(_collectionCarrierPrefab,
            _collectionTargets[slotIndex].transform.position, Quaternion.identity, transform).transform;
        matchlingPresenter.MoveToCollectionSlot(newCollectionCarrierTransform, Vector2.zero, 100F);
    }

    public void Match(List<MatchlingPresenter> matchlingPresentersToMatch)
    {
        float matchPositionX = matchlingPresentersToMatch[1].transform.parent.localPosition.x;
        float matchPositionY = matchlingPresentersToMatch[1].transform.parent.localPosition.y + 100F;

        foreach (var matchlingPresenter in matchlingPresentersToMatch)
        {
            matchlingPresenter.Match(transform, matchPositionX, matchPositionY);
        }
    }
}