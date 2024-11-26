using UnityEngine;

public class EnableInputsEvent : GameEvent
{
    public InputType Player { get; private set; }
    public bool Enable { get; private set; }

    public EnableInputsEvent(InputType playerType, bool enable)
    {
        Player = playerType;
        Enable = enable;
    }
}
