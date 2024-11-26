using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartedEvent : GameEvent
{
    public int _number;

    public GameStartedEvent(int number = 1)
    {
        _number = number;
    }
}
