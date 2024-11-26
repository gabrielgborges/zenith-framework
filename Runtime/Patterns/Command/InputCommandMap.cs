using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCommandMap
{
    Dictionary<string, ICommand> _commandByActionName = new Dictionary<string, ICommand>();

    public void Initialize()
    {
        _commandByActionName.Add("Defend", new DefendCommand());
        _commandByActionName.Add("ReleaseDefense", new ReleaseDefenseCommand());
        _commandByActionName.Add("Punch", new PunchCommand());
        _commandByActionName.Add("SoftPunch", new SoftPunchCommand());
        _commandByActionName.Add("HardPunch", new HardPunchCommand());
        _commandByActionName.Add("MoveTest", new MoveCommand());
    }

    public ICommand GetCommand(InputAction.CallbackContext context)
    {
        if (_commandByActionName.TryGetValue(context.action.name, out ICommand command))
        {
            return command;
        }
        Debug.Log("Released nothing");
        return null;
    }

    public ICommand GetCommand(string actionName)
    {
        if (_commandByActionName.TryGetValue(actionName, out ICommand command))
        {
            return command;
        }
        return null;
    }
}
