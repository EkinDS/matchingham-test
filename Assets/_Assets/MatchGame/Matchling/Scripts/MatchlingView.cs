using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchlingView : MonoBehaviour, IPointerClickHandler
{
    private Image _thisImage;
    private RectTransform _thisRectTransform;
    private MatchlingPresenter _matchlingPresenter;
    private MatchlingPlacement _matchlingPlacement;
    private Transform _parentTransform;

    public void Initialize(MatchlingPresenter matchlingPresenter, MatchlingPlacement matchlingPlacement, Sprite sprite)
    {
        _matchlingPresenter = matchlingPresenter;
        _matchlingPlacement = matchlingPlacement;
        _thisRectTransform = GetComponent<RectTransform>();
        _thisImage = GetComponent<Image>();
        _parentTransform = transform.parent;
        
        transform.localPosition = matchlingPlacement.position;
        _thisImage.sprite = sprite;
        _thisRectTransform.sizeDelta = new Vector2(matchlingPlacement.size, matchlingPlacement.size);
    }

    public void MoveToCollectionArea(Transform parent, Vector2 position, float size)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        _thisRectTransform.sizeDelta = new Vector2(size, size);
    }

    public void MoveToBackground()
    {
        transform.SetParent(_parentTransform);
        transform.localPosition = _matchlingPlacement.position;
        _thisRectTransform.sizeDelta = new Vector2(_matchlingPlacement.size, _matchlingPlacement.size);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _matchlingPresenter.OnViewClicked();
    }
}