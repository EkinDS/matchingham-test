using System.Collections.Generic;
using UnityEngine;

public class MatchGoalCheckerPresenter : MonoBehaviour
{
    [SerializeField] private List<MatchGoalSlotView> _matchGoalSlotViews;

    private EventBus _eventBus;
    private MatchGoalCheckerModel _matchGoalCheckerModel;
    private MatchGameData _matchGameData;

    public void Initialize(EventBus eventBus, MatchGameData matchGameData)
    {
        _eventBus = eventBus;
        _matchGameData = matchGameData;

        foreach (var slotImage in _matchGoalSlotViews)
        {
            slotImage.Initialize();
        }

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

        for (int i = 0; i < _matchGoalSlotViews.Count; i++)
        {
            if (i < _matchGoalCheckerModel.MatchGoals.Count)
            {
                MatchGoal goal = _matchGoalCheckerModel.MatchGoals[i];

                Sprite sprite = _matchGameData.GetSprite(goal.matchlingType);

                _matchGoalSlotViews[i].gameObject.SetActive(true);
                _matchGoalSlotViews[i].ResetForLevel(sprite, goal.matchlingType, goal.count);
            }
            else
            {
                _matchGoalSlotViews[i].gameObject.SetActive(false);
            }
        }
    }

    private void HandleOnMatchlingSelected(MatchlingSelectedEvent e)
    {
        foreach (var matchGoal in _matchGoalCheckerModel.MatchGoals)
        {
            if (matchGoal.matchlingType == e.MatchlingPresenter.GetMatchlingType())
            {
                matchGoal.count--;
            }
        }
        
        foreach (var matchGoalSlotView in _matchGoalSlotViews)
        {
            if (matchGoalSlotView.GetMatchlingType() == e.MatchlingPresenter.GetMatchlingType())
            {
                matchGoalSlotView.ReduceCount();
            }
        }

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
        _eventBus.Unsubscribe<MatchlingSelectedEvent>(HandleOnMatchlingSelected);
    }
}