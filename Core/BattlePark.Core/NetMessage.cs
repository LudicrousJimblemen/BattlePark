using System;

namespace BattlePark.Core {
	public delegate void NetMessage<T>(T message) where T : NetMessage;

	public abstract class NetMessage { }
}