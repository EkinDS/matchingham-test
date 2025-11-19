public struct AllMatchGoalsFulfilledEvent : IGameEvent
{
    public readonly float Timer;

    public AllMatchGoalsFulfilledEvent(float timer)
    {
        Timer = timer;
    }
}