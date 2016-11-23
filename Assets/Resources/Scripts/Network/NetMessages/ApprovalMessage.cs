public class ChatNetMessage : MatchMessage {
	public string Username { get; private set; }

	public ChatNetMessage(string username) {
		Username = username;
	}
}