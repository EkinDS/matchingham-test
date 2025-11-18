using UnityEngine;

public class MatchlingPresenter : MonoBehaviour
{
    private MatchlingView matchlingView;
    private MatchlingModel matchlingModel;


    public void Initialize(MatchlingPlacement matchlingPlacement, Sprite sprite)
    {
        matchlingView = GetComponent<MatchlingView>();
        matchlingModel = new MatchlingModel();
        
        matchlingView.Initialize(matchlingPlacement, sprite);
    }
}