public class OnEndGameEvent : GameEvent
{
    public string WinnerPlayer { get; private set; }

    public OnEndGameEvent(string winner)
    {
        WinnerPlayer = winner;
    }
}
