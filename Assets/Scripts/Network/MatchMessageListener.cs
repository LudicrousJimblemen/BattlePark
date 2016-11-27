using System;

public class NetMessageListener {
	public Type MessageType { get; private set; }
	public object Handler { get; private set; }

	public NetMessageListener(Type messageType, object handler) {
		MessageType = messageType;
		Handler = handler;
	}
}