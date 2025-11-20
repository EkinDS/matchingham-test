using System.Collections;
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

            matchlingPresenter.transform.parent.DOLocalMove(_collectionTargets[i].transform.localPosition, 0.5F).SetEase(Ease.Linear).OnComplete(() =>
            {
                //matchlingPresenter.OnPlacedInCollection();
            });
            //matchlingPresenter.MoveToCollectionSlot(transform, _collectionTargets[i].localPosition, 100F);
        }
    }

    public void PlaceMatchling(MatchlingPresenter matchlingPresenter, int slotIndex)
    {
        Transform newCollectionCarrierTransform = Instantiate(_collectionCarrierPrefab, _collectionTargets[slotIndex].transform.position, Quaternion.identity, transform).transform;
        matchlingPresenter.MoveToCollectionSlot(newCollectionCarrierTransform, Vector2.zero, 100F);
    }

    public void Match(List<MatchlingPresenter> matchlingPresentersToMatch)
    {
        StartCoroutine(PerformMatchAnimations(matchlingPresentersToMatch));
    }

    private IEnumerator PerformMatchAnimations(List<MatchlingPresenter> matchlingPresentersToMatch)
    {
        Transform newCollectionCarrierTransform = Instantiate(_collectionCarrierPrefab, matchlingPresentersToMatch[1].transform.position * 2 - matchlingPresentersToMatch[0].transform.position, Quaternion.identity, transform).transform;
        matchlingPresentersToMatch[2].MoveToCollectionSlot(newCollectionCarrierTransform, Vector2.zero, 100F);

        yield return new WaitForSeconds(0.6F);

        float matchPositionX = matchlingPresentersToMatch[1].transform.parent.localPosition.x;
        float matchPositionY = matchlingPresentersToMatch[1].transform.parent.localPosition.y + 100F;

        foreach (var matchlingPresenter in matchlingPresentersToMatch)
        {
            matchlingPresenter.Match(transform, matchPositionX, matchPositionY);
        }

        //matchlingPresentersToMatch.Last().Match();
    }
}