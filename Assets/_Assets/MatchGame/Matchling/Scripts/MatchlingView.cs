using UnityEngine;
using UnityEngine.UI;

public class MatchlingView : MonoBehaviour
{
    private Image thisImage;
    private RectTransform thisRectTransform;
    
    
    public void Initialize(MatchlingPlacement matchlingPlacement, Sprite sprite)
    {
        thisRectTransform = GetComponent<RectTransform>();
        thisImage = GetComponent<Image>();
        
        transform.localPosition = matchlingPlacement.position;
        thisImage.sprite = sprite;
        thisRectTransform.sizeDelta = new Vector2(matchlingPlacement.size, matchlingPlacement.size); 
    }
    
    
    private void OnMouseDown()
    {
        print("sa");
    }
}