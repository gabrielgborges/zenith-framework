using System;

public class MovingState : IState
{
    public Action<IState> OnChangeState { get; set; }

    public void HandleCommand(ICommand command, Combatent combatent)
    {
        if (command != null)
        {
            if (command as DefendCommand != null)
            {
                combatent.StopMove();
                command.Execute(combatent);
                OnChangeState?.Invoke(new DefensiveState());
            }
            else if (command as HitCommand != null)
            {
                combatent.StopMove();
                command.Execute(combatent);
                OnChangeState?.Invoke(new AttackingState());
            }
            else
            {
                command.Execute(combatent);
            }
        }
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.StopMove();
        combatent.TakeDamage(hit.Damage, "TakeDamage");
        OnChangeState?.Invoke(new TakingDamageState());
    }
}
