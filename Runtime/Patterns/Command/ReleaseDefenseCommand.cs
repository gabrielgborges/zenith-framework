using UnityEngine;

public class ReleaseDefenseCommand : ICommand
{
    public Vector2 Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Execute(Combatent combatent)
    {
        if (combatent.CurrentState is DefensiveState)
        {
            combatent.ExitDefense("Idle");
        }
    }
}
