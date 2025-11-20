using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image _darkenerImage;
    [SerializeField] private Transform _container;
    [SerializeField] private CanvasGroup _canvasGroup;

    private EventBus _eventBus;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void ContinueButton()
    {
        Disappear();

        _eventBus.Publish(new NextLevelRequestedEvent());
    }

    public void Appear(float timer)
    {
        _darkenerImage.color = new Color(0F, 0F, 0F, 0F);
        _container.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        _canvasGroup.alpha = 0F;
        
        gameObject.SetActive(true);

        _darkenerImage.DOFade(0.8F, 0.25F).SetEase(Ease.Linear);
        _container.transform.DOScale(1F, 0.25F).SetEase(Ease.OutBack);
        _canvasGroup.DOFade(1F, 0.25F);
        
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        _timerText.text = ($"{minutes:00}:{seconds:00}");
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}