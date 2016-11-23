public class ChatNetMessage : MatchMessage {
	public string From { get; private set; }
	public string Text { get; private set; }

	public ChatNetMessage(string from, string text) {
		From = from;
		Text = text;
	}
}