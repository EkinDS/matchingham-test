using UnityEngine;

public class MatchGoalCheckerPresenter : MonoBehaviour
{
    private EventBus _eventBus;

    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
}