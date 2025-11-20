public struct MatchCompletedEvent : IGameEvent
{
    public readonly MatchlingPresenter MatchlingPresenter;

    public MatchCompletedEvent(MatchlingPresenter matchlingPresenter)
    {
        MatchlingPresenter = matchlingPresenter;
    }
}