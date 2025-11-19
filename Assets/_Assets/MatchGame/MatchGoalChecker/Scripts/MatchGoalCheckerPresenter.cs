using System.Collections.Generic;
using UnityEngine;

public class MatchGoalCheckerPresenter : MonoBehaviour
{
    [SerializeField] private List<MatchGoalSlotView> _slotImages;

    private EventBus _eventBus;
    private MatchGameData _matchGameData;

    public void Initialize(EventBus eventBus, MatchGameData matchGameData)
    {
        _eventBus = eventBus;
        _matchGameData = matchGameData;

        foreach (var slotImage in _slotImages)
        {
            slotImage.Initialize();
        }
    }

    public void ResetForLevel(LevelData levelData)
    {
        List<MatchGoal> matchGoals = levelData.matchGoalList;

        for (int i = 0; i < _slotImages.Count; i++)
        {
            if (i < matchGoals.Count)
            {
                MatchGoal goal = matchGoals[i];

                Sprite sprite = _matchGameData.GetSprite(goal.matchlingType);

                _slotImages[i].gameObject.SetActive(true);
                _slotImages[i].ResetForLevel(sprite, goal.matchlingType, goal.count
                );
            }
            else
            {
                _slotImages[i].gameObject.SetActive(false);
            }
        }
    }
}