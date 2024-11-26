public class ConfirmedCharacterEvent : GameEvent
{
    public CombatentData CombatentData { get; private set; }
    public Combatent CombatentModule { get; private set; }

    public ConfirmedCharacterEvent(CombatentData combatentData, Combatent combatent)
    {
        CombatentData = combatentData;
        CombatentModule = combatent;
    }
}
