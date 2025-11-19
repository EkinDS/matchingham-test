using System.Collections.Generic;
using UnityEngine;

public class CollectionPresenter : MonoBehaviour
{
    private EventBus _eventBus;
    private CollectionView _collectionView;
    private CollectionModel _collectionModel;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _collectionView = GetComponent<CollectionView>();

        _eventBus.Subscribe<MatchlingSelectedEvent>(HandleMatchlingSelected);
        _eventBus.Subscribe<MatchlingPlacedInCollectionEvent>(HandleOnMatchlingPlacedInCollection);
    }

    public void ResetForLevel()
    {
        _collectionModel = new CollectionModel();
    }

    private void HandleMatchlingSelected(MatchlingSelectedEvent e)
    {
        if (_collectionModel.TryToAddMatchlingToCollection(e.MatchlingPresenter))
        {
            _collectionView.RearrangeMatchlingPresenters(_collectionModel.MatchlingPresenters);

            if (_collectionModel.MatchlingPresenters.Count >= 6)
            {
                _eventBus.Publish(new CollectionFilledEvent());
            }
        }
    }

    private void HandleOnMatchlingPlacedInCollection(MatchlingPlacedInCollectionEvent e)
    {
        int matchCenterIndex = _collectionModel.GetMatchCenterIndex();

        if (matchCenterIndex != -1)
        {
            Match(matchCenterIndex);
        }
    }

    private void Match(int matchCenterIndex)
    {
        List<MatchlingPresenter> matchlingPresentersToMatch = _collectionModel.Match(matchCenterIndex);

        _collectionView.Match(matchlingPresentersToMatch);

        _collectionView.RearrangeMatchlingPresenters(_collectionModel.MatchlingPresenters);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<MatchlingSelectedEvent>(HandleMatchlingSelected);
        _eventBus.Unsubscribe<MatchlingPlacedInCollectionEvent>(HandleOnMatchlingPlacedInCollection);
    }
}