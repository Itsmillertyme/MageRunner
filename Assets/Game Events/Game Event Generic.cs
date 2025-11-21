using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Event<T>")]

// GAME EVENT SCRIPTABLE OBJECTS CAN BE USED TO TRIGGER EVENTS
// THE OBJECTS ARE MADE AND ASSIGNED IN THE INSPECTOR

public class GameEventGeneric<T> : ScriptableObject
{
    // LIST OF THOSE TO BE NOTIFIED
    private readonly List<GameEventListenerGeneric<T>> listeners = new();

    public void AddToListener(GameEventListenerGeneric<T> listener) => listeners.Add(listener);

    public void RemoveFromListener(GameEventListenerGeneric<T> listener) => listeners.Remove(listener);


    public void Raise(T parameter)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(parameter);
        }
    }
}
