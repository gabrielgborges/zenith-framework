using System;

public class StandardState : IState
{
    public Action<IState> OnChangeState { get; set; }

    public void HandleCommand(ICommand command, Combatent combatent)
    {
        if(command != null)
        {
            command.Execute(combatent);
            if(command is DefendCommand)
            {
                OnChangeState?.Invoke(new DefensiveState());
            }
            else if(command is HitCommand)
            {
                OnChangeState?.Invoke(new AttackingState());
            }
            return;
        }
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.TakeDamage(hit.Damage, "TakeDamage");
        OnChangeState?.Invoke(new TakingDamageState());
    }
}
