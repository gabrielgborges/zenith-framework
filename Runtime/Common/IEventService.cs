
using System;

public interface IEventService : IService
{
    public void AddListener<T>(Action<T> action, int listenerHashCode) where T : GameEventBase { }

    public void RemoveListener<T>(int listenerHashCode) where T : GameEventBase { }

    public void TryInvokeEvent(GameEventBase gameEvent) { }
}
