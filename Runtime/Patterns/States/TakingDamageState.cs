using System;

public class TakingDamageState : IState
{
    public Action<IState> OnChangeState { get; set ; }

    public void HandleCommand(ICommand command, Combatent combatent)
    {
        OnChangeState?.Invoke(new StandardState());
        return;
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.TakeDamage(hit.Damage, "TakeDamage"); ;
    }
}