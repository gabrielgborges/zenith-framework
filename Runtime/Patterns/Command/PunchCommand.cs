using UnityEngine;

public class PunchCommand : HitCommand
{
    public override Vector2 Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Execute(Combatent combatent)
    {
        combatent.Attack("Punch");
    }
}
