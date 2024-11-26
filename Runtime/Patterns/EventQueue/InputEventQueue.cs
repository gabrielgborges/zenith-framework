using System.Collections.Generic;
using UnityEngine;

public class InputEventQueue : MonoBehaviour
{
    private Queue<ICommand> _currentCommands = new Queue<ICommand>();
    private Queue<ICommand> _currentSecondaryCommands = new Queue<ICommand>();

    public void AddCommand(ICommand command)
    {
        _currentCommands.Enqueue(command);
    }

    public ICommand GetNextCommand()
    {
        if(_currentCommands == null || _currentCommands.Count == 0)
        {
            if(_currentSecondaryCommands == null || _currentSecondaryCommands.Count == 0)
            {
                return null;
            }
            return _currentSecondaryCommands.Dequeue();
        }
        ClearSecondaryCommands();
        return _currentCommands.Dequeue();
    }

    public void AddSecondaryCommand(ICommand command)
    {
        _currentSecondaryCommands.Enqueue(command);
    }

    private void ClearSecondaryCommands()
    {
        _currentSecondaryCommands.Clear();
    }
}