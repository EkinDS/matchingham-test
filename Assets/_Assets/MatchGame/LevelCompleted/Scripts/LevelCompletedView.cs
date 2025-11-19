using TMPro;
using UnityEngine;

public class LevelCompletedView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

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
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = ($"{minutes:00}:{seconds:00}");
        gameObject.SetActive(true);
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}