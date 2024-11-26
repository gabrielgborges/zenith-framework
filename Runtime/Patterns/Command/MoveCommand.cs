using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector2 Value { get; set; }

    public void Execute(Combatent combatent)
    {
        string context = "Walk";
        if (Value.x < 0)
        {
            context = "Walk_Back";
        }
        combatent.Move(context, Value);
    }
}