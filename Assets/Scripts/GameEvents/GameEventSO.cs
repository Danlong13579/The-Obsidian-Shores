using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<string> componentListening = new List<string>();
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(Component sender, object data)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListener(GameEventListener listener, string name)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
            componentListening.Add(name);
        }
    }

    public void UnregisterListener(GameEventListener listener, string name)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
            componentListening.Remove(name);
        }
    }
}
