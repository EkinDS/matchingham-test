using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchGoalSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;

    private Image _image;
    private MatchlingType _matchlingType;

    
    public void Initialize()
    {
        _image = GetComponent<Image>();
    }

    public void ResetForLevel(Sprite sprite, MatchlingType matchlingType, int count)
    {
        _countText.text = count.ToString();
        _image.sprite = sprite;
        _matchlingType  = matchlingType;
        
    }
    
    
}