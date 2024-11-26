using UnityEngine;

public interface ICommand
{
    Vector2 Value { get; set; }
    
    void Execute(Combatent combatent);
}
