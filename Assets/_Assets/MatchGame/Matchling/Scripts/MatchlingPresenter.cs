using UnityEngine;

public class MatchlingPresenter : MonoBehaviour
{
    private EventBus _eventBus;
    private MatchlingView _matchlingView;
    private MatchlingType _matchlingType;

    public void Initialize(EventBus eventBus, MatchlingPlacement matchlingPlacement, MatchlingType matchlingType,
        Sprite sprite)
    {
        _eventBus = eventBus;
        _matchlingView = GetComponent<MatchlingView>();
        _matchlingType = matchlingType;

        _matchlingView.Initialize(this, matchlingPlacement, sprite);
    }

    public void OnViewClicked()
    {
        _eventBus.Publish(new MatchlingSelectedEvent(this));
    }

    public MatchlingType GetMatchlingType()
    {
        return _matchlingType;
    }

    public void MoveToCollectionSlot(Transform collectionParent, Vector2 anchoredPosition, float size)
    {
        _matchlingView.MoveToCollectionArea(collectionParent, anchoredPosition, size);
    }

    public void Match(Transform parent, float matchPositionX, float matchPositionY)
    {
        _matchlingView.Match(parent, matchPositionX, matchPositionY, _eventBus);
    }
}