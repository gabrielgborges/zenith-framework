public class SelectedCharacterEvent : GameEvent
{
    public CombatentData Combatent { get; private set; }
    public bool ConfirmedSelection { get; private set; }

    public SelectedCharacterEvent(CombatentData combatent, bool confirmed)
    {
        Combatent = combatent;
        ConfirmedSelection = confirmed;
    }
}
