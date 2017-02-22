using System;
using System.Text.RegularExpressions;

public struct ChatMessage {
	public DateTime Time { get; private set; }
	public string Username { get; private set; }
	public string Message { get; private set; }
	
	public ChatMessage(string username, string message) {
		message = Regex.Replace(message, @"\*\*(.+?)\*\*", @"<b>$1<b>");
		message = Regex.Replace(message, @"\*(.+?)\*", @"<i>$1<i>");
		message = Regex.Replace(message, @"__(.+?)__", @"<u>$1<u>");
		
		this.Time = DateTime.Now;
		this.Username = username;
		this.Message = message;
	}
}
