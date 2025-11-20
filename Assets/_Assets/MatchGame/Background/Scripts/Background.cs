using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Background : MonoBehaviour, IDragHandler
{
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float mouseZoomSpeed;
    [SerializeField] private Vector2 minimumPosition;
    [SerializeField] private Vector2 maximumPosition;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Image _image;

    private float _initialPinchDistance;
    private Vector3 _initialPinchScale;
    private bool _isPinching;

    private void Update()
    {
        HandlePinchZoom();
    }

    public void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _image = GetComponent<Image>();

    }

    public void ResetForLevel(Sprite sprite)
    {
        _image.sprite = sprite;
        
        transform.localPosition = Vector3.zero;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isPinching || Input.touchCount > 1)
        {
            return;
        }

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        var pos = _rectTransform.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, minimumPosition.x, maximumPosition.x);
        pos.y = Mathf.Clamp(pos.y, minimumPosition.y, maximumPosition.y);
        _rectTransform.anchoredPosition = pos;
    }

    private void HandlePinchZoom()
    {
        if (Input.touchCount < 2)
        {
            _isPinching = false;
            _initialPinchDistance = 0f;
            return;
        }

        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if (!_isPinching || t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            _isPinching = true;
            _initialPinchDistance = Vector2.Distance(t0.position, t1.position);
            _initialPinchScale = _rectTransform.localScale;
            return;
        }

        float currentDistance = Vector2.Distance(t0.position, t1.position);
        float factor = currentDistance / _initialPinchDistance;
        float newScale = Mathf.Clamp(_initialPinchScale.x * factor, minZoom, maxZoom);

        _rectTransform.localScale = new Vector3(newScale, newScale, 1f);
    }
}