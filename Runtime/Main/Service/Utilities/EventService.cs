using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventService : BaseService, IEventService
{
    private Dictionary<int, List<GameEventListened>> _eventHashAndListeners = new Dictionary<int, List<GameEventListened>>();

    public override void Setup()
    {
        ServiceLocator.AddService<IEventService>(this);
    }  
    
    public override void Dispose()
    {
        ServiceLocator.RemoveService<IEventService>(this);
    }

    public void AddListener<T>(Action<T> action, int listenerHashCode) where T : GameEvent
    {
        int actionTypeHashCode = typeof(T).GetHashCode();
        Action<GameEvent> castedGameEvent = myEvent => action((T)myEvent);
        GameEventListened gameEventAndListener = new GameEventListened(listenerHashCode, castedGameEvent);

        if (_eventHashAndListeners.ContainsKey(actionTypeHashCode))
        {
            _eventHashAndListeners[actionTypeHashCode].Add(gameEventAndListener);
            return;
        }

        _eventHashAndListeners.Add(actionTypeHashCode, new List<GameEventListened>() { gameEventAndListener });
    }

    public void RemoveListener<T>(int listenerHashCode) where T : GameEvent
    {
        int hashCode = typeof(T).GetHashCode();

        if (_eventHashAndListeners.ContainsKey(hashCode))
        {
            RemoveEvent(hashCode, listenerHashCode);

            if (_eventHashAndListeners[hashCode].Count < 1)
            {
                RemoveEventReferences(hashCode);
            }
            return;
        }

        Debug.Log("There is no listener to remove from: " + typeof(T));
        Debug.Log("Dictionary keys: " + _eventHashAndListeners.Keys.Count);
    }

    public void TryInvokeEvent(GameEvent gameEvent)
    {
        int hashCode = gameEvent.GetType().GetHashCode();

        if (_eventHashAndListeners.ContainsKey(hashCode))
        {
            foreach (GameEventListened action in _eventHashAndListeners[hashCode])
            {
                Debug.Log("Invoked one");
                action.GameEvent?.Invoke(gameEvent);
            } 
            return;
        }

        Debug.Log("There is no listener to: " + gameEvent.GetType());
    }

    private void RemoveEvent(int eventHash, int listenerHashCode)
    {     
        foreach (GameEventListened gameEventByHash in _eventHashAndListeners[eventHash])
        {
            if(gameEventByHash.ListenerHashCode == listenerHashCode)
            {
                _eventHashAndListeners[eventHash].Remove(gameEventByHash);
                return;
            }
        }
    }

    private void RemoveEventReferences(int eventHash)
    {
        _eventHashAndListeners.Remove(eventHash);
    }

}

[Serializable]
internal struct GameEventListened
{
    public int ListenerHashCode;
    public Action<GameEvent> GameEvent;

    public GameEventListened(int listenerHashCode, Action<GameEvent> gameEvent)
    {
        ListenerHashCode = listenerHashCode;
        GameEvent = gameEvent;
    }
}