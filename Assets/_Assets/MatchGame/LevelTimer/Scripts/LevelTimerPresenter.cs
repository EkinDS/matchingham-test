using UnityEngine;

public class LevelTimerPresenter : MonoBehaviour
{
    private EventBus _eventBus;
    private LevelTimerView _levelTimerView;
    private float _timeLimit;
    private bool _iFreezed;

    private void Update()
    {
        UpdateTimer();
    }
    
    public void Initialize(EventBus eventBus)
    {
        _eventBus = eventBus;
        _levelTimerView = GetComponent<LevelTimerView>();

    }

    public void StartTimer(float timeLimit)
    {
        _iFreezed = false;
        _timeLimit = timeLimit;
    }
    
    public void StopTimer()
    {
        _iFreezed = true;
    }

    public float GetTimeLimit()
    {
        return _timeLimit;
    }
    
    private void UpdateTimer()
    {
        if (_iFreezed)
        {
            return;
        }
        
        _timeLimit = Mathf.Max(0, _timeLimit - Time.deltaTime);

        if (_timeLimit < 0.1F)
        {
            StopTimer();
            
            _eventBus.Publish(new TimeRanOutEvent());
        }
        
        _levelTimerView.SetTimerText(_timeLimit);
    }
}