public class OnPlayerWin : GameEvent
{
    public string WinnerPlayer { get; private set; }

    public OnPlayerWin(string winner)
    {
        WinnerPlayer = winner;
    }
}
