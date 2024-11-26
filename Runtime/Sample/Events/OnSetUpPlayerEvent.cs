public class OnSetUpPlayerEvent : GameEvent
{
    public int Player { get; private set; }
    public Combatent CombatentModule { get; private set; }

    public OnSetUpPlayerEvent(int player, Combatent combatent)
    {
        Player = player;
        CombatentModule = combatent;
    }
}
