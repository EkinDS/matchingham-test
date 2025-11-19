public struct MatchlingSelectedEvent : IGameEvent
{
    public readonly MatchlingPresenter MatchlingPresenter;


    public MatchlingSelectedEvent(MatchlingPresenter matchlingPresenter)
    {
        MatchlingPresenter = matchlingPresenter;
    }
}