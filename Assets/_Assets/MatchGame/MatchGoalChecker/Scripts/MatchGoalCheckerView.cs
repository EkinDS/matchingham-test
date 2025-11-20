using System.Collections.Generic;
using UnityEngine;

public class MatchGoalCheckerView : MonoBehaviour
{
    [SerializeField] private List<MatchGoalSlotView> _matchGoalSlotViews;

    private SpriteService _spriteService;

    public void Initialize(SpriteService spriteService)
    {
        _spriteService = spriteService;
    }
    
    public void ResetForLevel(List<MatchGoal> matchGoals)
    {
        for (int i = 0; i < _matchGoalSlotViews.Count; i++)
        {
            if (i < matchGoals.Count)
            {
                MatchGoal goal = matchGoals[i];

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
    
    public void OnMatchlingSelected(MatchlingType type)
    {
        foreach (var slotView in _matchGoalSlotViews)
        {
            if (slotView.gameObject.activeSelf &&
                slotView.GetMatchlingType() == type)
            {
                slotView.ReduceCount();
            }
        }
    }
}