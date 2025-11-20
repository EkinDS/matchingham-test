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

        _collectionView.Initialize();

        _eventBus.Subscribe<MatchlingSelectedEvent>(HandleOnMatchlingSelected);
    }

    public void ResetForLevel()
    {
        _collectionModel = new CollectionModel();
        _collectionView.ResetForLevel(_collectionModel.CollectionCapacity);
    }

    public void MatchIfPossible()
    {
        int matchCenterIndex = _collectionModel.GetMatchCenterIndex();

        if (matchCenterIndex != -1)
        {
            Match(matchCenterIndex);
        }
        else
        {
            if (_collectionModel.MatchlingPresenters.Count >= _collectionModel.CollectionCapacity)
            {
                _eventBus.Publish(new CollectionFilledEvent());
            }
        }
    }

    private void HandleOnMatchlingSelected(MatchlingSelectedEvent e)
    {
        if (!_collectionView.HasSpace())
        {
            return;
        }

        if (_collectionModel.TryToAddMatchlingToCollection(e.MatchlingPresenter))
        {
            _collectionView.AddMatchlingPresenter(e.MatchlingPresenter);

            _collectionView.PlaceMatchling(e.MatchlingPresenter,
                _collectionModel.MatchlingPresenters.IndexOf(e.MatchlingPresenter));
            _collectionView.RearrangeMatchlingPresenters(_collectionModel.MatchlingPresenters);
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
        _eventBus.Unsubscribe<MatchlingSelectedEvent>(HandleOnMatchlingSelected);
    }
}