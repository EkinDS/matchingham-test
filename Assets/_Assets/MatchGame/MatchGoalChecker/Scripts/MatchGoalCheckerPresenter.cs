using System.Collections.Generic;
using UnityEngine;

public class MatchGoalCheckerPresenter : MonoBehaviour
{
    [SerializeField] private List<MatchGoalSlotView> _matchGoalSlotViews;

    private EventBus _eventBus;
    private MatchGoalCheckerModel _matchGoalCheckerModel;
    private MatchGameData _matchGameData;
    private SpriteService _spriteService;

    public void Initialize(EventBus eventBus, MatchGameData matchGameData, SpriteService spriteService)
    {
        _eventBus = eventBus;
        _matchGameData = matchGameData;
        _spriteService = spriteService;

        foreach (var slotView in _matchGoalSlotViews)
        {
            slotView.Initialize();
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

        // Setup UI slots
        for (int i = 0; i < _matchGoalSlotViews.Count; i++)
        {
            if (i < _matchGoalCheckerModel.MatchGoals.Count)
            {
                MatchGoal goal = _matchGoalCheckerModel.MatchGoals[i];

                // ✅ Get sprite from SpriteService (addressables-backed)
                Sprite sprite = _spriteService.GetMatchlingSprite(goal.matchlingType);

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
        // Update model counts
        foreach (var matchGoal in _matchGoalCheckerModel.MatchGoals)
        {
            if (matchGoal.matchlingType == e.MatchlingPresenter.GetMatchlingType())
            {
                matchGoal.count--;
            }
        }

        // Update UI
        foreach (var matchGoalSlotView in _matchGoalSlotViews)
        {
            if (matchGoalSlotView.GetMatchlingType() == e.MatchlingPresenter.GetMatchlingType())
            {
                matchGoalSlotView.ReduceCount();
            }
        }

        // Check completion
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
