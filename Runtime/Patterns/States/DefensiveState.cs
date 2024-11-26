using System;

public class DefensiveState : IState
{
    public Action<IState> OnChangeState { get; set; }

    public void HandleCommand(ICommand command, Combatent combatent)
    {
        if(command as ReleaseDefenseCommand != null)
        {
            command.Execute(combatent);
            OnChangeState?.Invoke(new StandardState());
            return;
        }
        else if(command as HitCommand != null)
        {
            command.Execute(combatent);
            OnChangeState?.Invoke(new AttackingState());
            return;
        }
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.TakeDamage(hit.Damage / 2, "");
    }
}
