using UnityEngine;

public class MatchGoalCheckerPresenter : MonoBehaviour
{
    private EventBus _eventBus;
    private MatchGoalCheckerModel _matchGoalCheckerModel;
    private MatchGoalCheckerView _matchGoalCheckerView;

    public void Initialize(EventBus eventBus, SpriteService spriteService)
    {
        _eventBus = eventBus;
        _matchGoalCheckerView = GetComponent<MatchGoalCheckerView>();
        _matchGoalCheckerView.Initialize(spriteService);
        
        _eventBus.Subscribe<MatchlingSelectedEvent>(HandleOnMatchlingSelected);
    }

    public void ResetForLevel(LevelData levelData)
    {
        _matchGoalCheckerModel = new MatchGoalCheckerModel();

        foreach (var matchGoal in levelData.matchGoalList)
        {
            MatchGoal newMatchGoal = new MatchGoal
            {
                count = matchGoal.count,
                matchlingType = matchGoal.matchlingType
            };

            _matchGoalCheckerModel.MatchGoals.Add(newMatchGoal);
        }

        _matchGoalCheckerView.ResetForLevel(_matchGoalCheckerModel.MatchGoals);
    }

    private void HandleOnMatchlingSelected(MatchlingSelectedEvent e)
    {
        var type = e.MatchlingPresenter.GetMatchlingType();

        foreach (var matchGoal in _matchGoalCheckerModel.MatchGoals)
        {
            if (matchGoal.matchlingType == type)
            {
                matchGoal.count--;
            }
        }

        _matchGoalCheckerView.OnMatchlingSelected(type);

        if (AllMatchGoalsHaveBeenFulfilled())
        {
            _eventBus.Publish(new AllMatchGoalsFulfilledEvent());
        }
    }

    private bool AllMatchGoalsHaveBeenFulfilled()
    {
        foreach (var matchGoal in _matchGoalCheckerModel.MatchGoals)
        {
            if (matchGoal.count > 0)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<MatchlingSelectedEvent>(HandleOnMatchlingSelected);
        }
    }
}
