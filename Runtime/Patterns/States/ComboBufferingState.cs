using System;

public class ComboBufferingState : IState
{
    public Action<IState> OnChangeState { get; set; }

    public void HandleCommand(ICommand command, Combatent combatent)
    {
        if (command != null)
        {
            if(command is HitCommand)
            {
                command.Execute(combatent);
                OnChangeState?.Invoke(new AttackingState());
            }
            else if (command is DefendCommand)
            {
                OnChangeState?.Invoke(new DefensiveState());
            }
        }
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.TakeDamage(hit.Damage, "TakeDamage");
    }
}
