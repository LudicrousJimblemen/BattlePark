using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyMessageListener
{
    public System.Type MessageType { get; private set; }
    public object Handler { get; private set; }

    public LobbyMessageListener(System.Type messageType, object handler)
    {
        MessageType = messageType;
        Handler = handler;
    }
}

public abstract class LobbyMessenger
{
    protected List<LobbyMessageListener> listeners = new List<LobbyMessageListener>();

    /// <summary>
    /// Sends a message to this messenger.
    /// </summary>
    /// <param name="message"></param>
    public abstract void SendMessage<T>(T message) where T : LobbyMessage;

    public abstract void UpdateListeners();

    public abstract void Close();

    /// <summary>
    /// Creates a listener for a type of message that calls the supplied handler delegate when this message type is received.
    /// </summary>
    /// <typeparam name="T">Message type to add.</typeparam>
    /// <param name="handler">Handler to call when this message type is received.</param>
    public void CreateListener<T>(LobbyMessageHandler<T> handler) where T : LobbyMessage
    {
        listeners.Add(new LobbyMessageListener(typeof(T), handler));
    }

    /// <summary>
    /// Removes the first found listener (if any) matching to the supplied handler delegate.
    /// </summary>
    /// <typeparam name="T">Message type to look for.</typeparam>
    /// <param name="handler">Handler to remove.</param>
    /// <returns>True if a listener was removed, false if none found.</returns>
    public bool RemoveListener<T>(LobbyMessageHandler<T> handler) where T : LobbyMessage
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            LobbyMessageListener listenerEntry = listeners[i];
            if (listenerEntry.MessageType == typeof(T) && (LobbyMessageHandler<T>)listenerEntry.Handler == handler)
            {
                listeners.Remove(listenerEntry);
                return true;
            }
        }
        return false;
    }
}