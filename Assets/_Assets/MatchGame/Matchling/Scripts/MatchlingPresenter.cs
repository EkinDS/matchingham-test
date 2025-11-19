using UnityEngine;

public class MatchlingPresenter : MonoBehaviour
{
    private EventBus _eventBus;
    private MatchlingView _matchlingView;
    private MatchlingModel _matchlingModel;
    private MatchlingType _matchlingType;

    public void Initialize(EventBus eventBus, MatchlingPlacement matchlingPlacement, MatchlingType matchlingType,
        Sprite sprite)
    {
        _eventBus = eventBus;
        _matchlingView = GetComponent<MatchlingView>();
        _matchlingModel = new MatchlingModel();
        _matchlingType = matchlingType;

        _matchlingView.Initialize(this, matchlingPlacement, sprite);
    }

    public void OnViewClicked()
    {
        if (!_matchlingModel.IsSelected)
        {
            _matchlingModel.IsSelected = true;

            _eventBus.Publish(new MatchlingSelectedEvent(this));
        }
    }

    public void OnPlacedInCollection()
    {
        _eventBus.Publish(new MatchlingPlacedInCollectionEvent());
    }

    public MatchlingType GetMatchlingType()
    {
        return _matchlingType;
    }

    public void MoveToCollectionSlot(Transform collectionParent, Vector2 anchoredPosition, float size)
    {
        _matchlingView.MoveToCollectionArea(collectionParent, anchoredPosition, size);
    }

    public void Match(float matchPositionX, float matchPositionY)
    {
        _matchlingView.Match(matchPositionX, matchPositionY);
    }
}