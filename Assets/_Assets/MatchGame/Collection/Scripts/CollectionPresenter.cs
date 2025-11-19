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
        _collectionModel = new CollectionModel();

        _eventBus.Subscribe<MatchlingSelectedEvent>(HandleMatchlingSelected);
        _eventBus.Subscribe<MatchlingDeselectedEvent>(HandleMatchlingDeselected);
    }

    private void HandleMatchlingSelected(MatchlingSelectedEvent e)
    {
        if (_collectionModel.TryToAddMatchlingToCollection(e.MatchlingPresenter))
        {
            _collectionView.RearrangeMatchlingPresenters(_collectionModel.MatchlingPresenters);
        }
    }


    private void HandleMatchlingDeselected(MatchlingDeselectedEvent e)
    {
        _collectionModel.RemoveMatchlingPresenter(e.MatchlingPresenter);
        
        _collectionView.RearrangeMatchlingPresenters(_collectionModel.MatchlingPresenters);
    }
    
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<MatchlingSelectedEvent>(HandleMatchlingSelected);
        _eventBus.Unsubscribe<MatchlingDeselectedEvent>(HandleMatchlingDeselected);
    }
}