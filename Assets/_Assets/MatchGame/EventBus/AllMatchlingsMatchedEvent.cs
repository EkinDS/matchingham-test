public struct AllMatchlingsMatchedEvent : IGameEvent
{
    public readonly float Timer;

    public AllMatchlingsMatchedEvent(float timer)
    {
        Timer = timer;
    }
}