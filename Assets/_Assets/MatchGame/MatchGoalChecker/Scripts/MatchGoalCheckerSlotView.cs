using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchGoalSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;

    private Image _image;
    private MatchlingType _matchlingType;
    private int _count;

    
    public void Initialize()
    {
        _image = GetComponent<Image>();
    }

    public void ResetForLevel(Sprite sprite, MatchlingType matchlingType, int count)
    {
        _countText.text = "x" + count;
        _image.sprite = sprite;
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