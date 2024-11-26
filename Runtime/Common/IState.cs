using System;

public interface IState
{
    Action<IState> OnChangeState { get; set; }

    void HandleCommand(ICommand command, Combatent combatent);

    void HandleTakeHit(DamageData hit, Combatent combatent);
}
