using UnityEngine;

public class DefendCommand : ICommand
{
    public Vector2 Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(Combatent combatent)
    {
        combatent.Defend("Defend");
    }
}