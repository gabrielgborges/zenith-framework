using UnityEngine;

public abstract class HitCommand : ICommand
{
    public virtual Vector2 Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public virtual void Execute(Combatent combatent)
    {
    }
}
