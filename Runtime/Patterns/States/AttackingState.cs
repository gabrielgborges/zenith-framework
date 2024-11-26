using System;

public class AttackingState : IState
{
    public Action<IState> OnChangeState { get; set; }
    
    public void HandleCommand(ICommand command, Combatent combatent)
    {
// Can't actually do anything while attacking ¯\_(ツ)_/¯
    }

    public void HandleTakeHit(DamageData hit, Combatent combatent)
    {
        combatent.TakeDamage(hit.Damage, "TakeDamage");
        OnChangeState?.Invoke(new TakingDamageState());
    }
}
