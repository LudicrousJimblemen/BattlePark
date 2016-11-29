using System;

namespace BattlePark.Core {
	public delegate void NetMessageHandler<T>(T message) where T : NetMessage;

	public abstract class NetMessage { }
}