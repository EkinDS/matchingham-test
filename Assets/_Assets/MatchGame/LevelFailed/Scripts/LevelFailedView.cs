using UnityEngine;

public class LevelFailedView : MonoBehaviour
{
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
        gameObject.SetActive(true);
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}