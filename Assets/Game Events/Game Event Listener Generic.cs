using UnityEngine;
using UnityEngine.Events;

// THIS SCRIPT LISTENS FOR GAME EVENT SCRIPTABLE OBJECTS.
// IT'S ATTACHED TO A LISTENER IN THE SCENE.

public class GameEventListenerGeneric<T> : MonoBehaviour
{
    [SerializeField] private GameEventGeneric<T> Event;
    [SerializeField] private UnityEvent<T> Response;

    private void OnEnable() => Event.AddToListener(this);

    private void OnDisable() => Event.RemoveFromListener(this);

    public void OnEventRaised(T parameter) => Response.Invoke(parameter);
}