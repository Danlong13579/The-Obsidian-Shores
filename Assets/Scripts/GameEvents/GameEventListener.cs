using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

[HelpURL("https://www.youtube.com/watch?v=7_dyDmF0Ktw")]
public class GameEventListener : MonoBehaviour
{
    [Header("Game Event")]
    [Tooltip("Drag and drop the GameEvent Assest you want to listener to here.")]
    public GameEvent gameEvent;

    [Header("Response")]
    [Tooltip("The response to invoke when the event is raised.")]
    public CustomGameEvent response;

    [Header("Debugging")]
    public bool isLogging = false;

    // This method is called when the script is enabled
    // Based on Unity's MonoBehaviour lifecycle
    private void OnEnable()
    {
        gameEvent.RegisterListener(this, transform.name);
    }

    // This method is called when the script is disabled
    // Based on Unity's MonoBehaviour lifecycle
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this, transform.name);
    }

    // This method is called when the event is raised
    public void OnEventRaised(Component sender, object data)
    {
        if (isLogging)
            Debug.Log($"Event raised by: {sender.name}, Data: {data}");
        response.Invoke(sender, data);
    }
}
