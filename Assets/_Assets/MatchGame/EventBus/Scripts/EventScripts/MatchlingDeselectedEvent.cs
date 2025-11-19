public struct MatchlingDeselectedEvent : IGameEvent
{
    public readonly MatchlingPresenter MatchlingPresenter;


    public MatchlingDeselectedEvent(MatchlingPresenter matchlingPresenter)
    {
        MatchlingPresenter = matchlingPresenter;
    }
}