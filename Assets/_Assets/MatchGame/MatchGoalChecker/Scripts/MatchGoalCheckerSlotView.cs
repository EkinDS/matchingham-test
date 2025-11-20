using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchGoalSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _goalImage;

    private MatchlingType _matchlingType;
    private int _count;
    
    public void ResetForLevel(Sprite sprite, MatchlingType matchlingType, int count)
    {
        _countText.text = "x" + count;
        _goalImage.sprite = sprite;
        _matchlingType  = matchlingType;
        _count = count;
    }

    public MatchlingType GetMatchlingType()
    {
        return _matchlingType;
    }


    public void ReduceCount()
    {
        _count--;
        _countText.text = "x"+ _count;
    }
}