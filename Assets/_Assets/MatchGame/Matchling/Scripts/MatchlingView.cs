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
        transform.SetParent(parent);
        _thisRectTransform.DOSizeDelta(new Vector2(size, size), 0.3F).SetEase(Ease.InBack);
        transform.DOLocalMove(position, 0.5F).SetEase(Ease.InBack).OnComplete(() =>
        {
            _matchlingPresenter.OnPlacedInCollection();
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _thisImage.raycastTarget = false;
        
        _matchlingPresenter.OnViewClicked();
    }

    public void Match(float matchPositionX, float matchPositionY)
    {
        transform.DOLocalMoveX(matchPositionX, 0.25F).SetEase(Ease.InSine);
        transform.DOLocalMoveY(matchPositionY, 0.3F).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}