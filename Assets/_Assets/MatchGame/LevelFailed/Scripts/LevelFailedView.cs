using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailedView : MonoBehaviour
{
    [SerializeField] private Image _darkenerImage;
    [SerializeField] private Transform _container;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private EventBus _eventBus;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void RestartButton()
    {
        Disappear();
        
        _eventBus.Publish(new NextLevelRequestedEvent());
    }

    public void Appear()
    {
        _darkenerImage.color = new Color(0F, 0F, 0F, 0F);
        _container.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        _canvasGroup.alpha = 0F;
        
        gameObject.SetActive(true);

        _darkenerImage.DOFade(0.8F, 0.25F).SetEase(Ease.Linear);
        _container.transform.DOScale(1F, 0.25F).SetEase(Ease.OutBack);
        _canvasGroup.DOFade(1F, 0.25F);
        
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}