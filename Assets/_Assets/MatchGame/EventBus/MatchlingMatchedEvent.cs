public struct MatchlingMatchedEvent : IGameEvent
{
    public readonly MatchlingPresenter MatchlingPresenter;


    public MatchlingMatchedEvent(MatchlingPresenter matchlingPresenter)
    {
        MatchlingPresenter = matchlingPresenter;
    }
}