using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchlingView : MonoBehaviour, IPointerClickHandler
{
    private Image _thisImage;
    private RectTransform _thisRectTransform;
    private MatchlingPresenter _matchlingPresenter;

    public void Initialize(MatchlingPresenter matchlingPresenter, MatchlingPlacement matchlingPlacement, Sprite sprite)
    {
        _matchlingPresenter = matchlingPresenter;
        _thisRectTransform = GetComponent<RectTransform>();
        _thisImage = GetComponent<Image>();

        transform.localPosition = matchlingPlacement.position;
        _thisImage.sprite = sprite;
        _thisRectTransform.sizeDelta = new Vector2(matchlingPlacement.size, matchlingPlacement.size);
    }

    public void MoveToCollectionArea(Transform parent, Vector2 position, float size)
    {
        _thisImage.raycastTarget = false;

        transform.SetParent(parent);
        _thisRectTransform.DOSizeDelta(new Vector2(size, size), 0.3F).SetEase(Ease.InBack);
        transform.DOLocalMove(position, 0.5F).SetEase(Ease.InBack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _matchlingPresenter.OnViewClicked();
    }

    public void Match(Transform parent, float matchPositionX, EventBus eventBus)
    {
        transform.SetParent(parent);

        transform.DOLocalMoveX(matchPositionX, 0.25F).SetEase(Ease.InBack).OnComplete(() =>
        {
            eventBus.Publish(new MatchCompletedEvent(_matchlingPresenter));
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}