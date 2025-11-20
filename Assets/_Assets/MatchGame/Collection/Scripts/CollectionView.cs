using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectionView : MonoBehaviour
{
    [SerializeField] private List<Transform> _collectionTargets;
    [SerializeField] private Transform _collectionCarrierPrefab;

    private List<MatchlingPresenter> _matchlingPresenters;
    private int _capacity;
    private List<Tween> _tweens;
    private Coroutine _coroutine;
    private CollectionPresenter _collectionPresenter; //bunu eventle de halledebilirim

    public void Initialize()
    {
        _collectionPresenter = GetComponent<CollectionPresenter>();
        _tweens = new List<Tween>();
    }

    public void ResetForLevel(int capacity)
    {
        _matchlingPresenters = new List<MatchlingPresenter>();
        _capacity = capacity;
    }

    public void RearrangeMatchlingPresenters(List<MatchlingPresenter> matchlingPresenters)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        foreach (var tween in _tweens)
        {
            tween.Kill();
        }
        
        _tweens = new List<Tween>();

        _coroutine = StartCoroutine(RearrangeMatchlingPresentersOverTime(matchlingPresenters));
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

            _matchlingPresenters.Remove(matchlingPresenter);
        }
    }

    public bool HasSpace()
    {
        return _matchlingPresenters.Count < _capacity;
    }

    public void AddMatchlingPresenter(MatchlingPresenter matchlingPresenter)
    {
        _matchlingPresenters.Add(matchlingPresenter);
    }

    private IEnumerator RearrangeMatchlingPresentersOverTime(List<MatchlingPresenter> matchlingPresenters)
    {
        for (var i = 0; i < matchlingPresenters.Count; i++)
        {
            var matchlingPresenter = matchlingPresenters[i];

            Tween newTween = matchlingPresenter.transform.parent
                .DOLocalMove(_collectionTargets[i].transform.localPosition, 0.5F).SetEase(Ease.Linear);
            
            _tweens.Add(newTween);
        }

        yield return new WaitForSeconds(0.6F);
        
        _collectionPresenter.MatchIfPossible();
    }
}