
using System;

public interface IEventService : IService
{
    public void AddListener<T>(Action<T> action, int listenerHashCode) where T : GameEvent { }

    public void RemoveListener<T>(int listenerHashCode) where T : GameEvent { }

    public void TryInvokeEvent(GameEvent gameEvent) { }
}
